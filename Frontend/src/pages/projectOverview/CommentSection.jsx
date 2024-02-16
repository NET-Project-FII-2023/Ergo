import React, { useState, useEffect } from 'react';
import { Card, CardContent } from '@mui/material';
import ModeCommentIcon from '@mui/icons-material/ModeComment';
import api from '@/services/api';
import { useUser } from "@/context/LoginRequired";
import { toast } from "react-toastify";
import ErgoInput from "@/widgets/form_utils/ErgoInput";
import UserAvatar from "@/common/components/UserAvatar";
import {useNavigate} from "react-router-dom";
import DeleteIcon from '@mui/icons-material/Delete';


const CommentSection = ({ task, token }) => {
    const currentUser = useUser();
    const navigate = useNavigate();
    const [comments, setComments] = useState([]);
    const [newCommentText, setNewCommentText] = useState('');

    const formatTime = (deadline) => {
        const options = { day: 'numeric', month: 'long', year: 'numeric' };
        const formattedDeadline = new Date(deadline).toLocaleDateString(undefined, options);
        return formattedDeadline;
      };

    useEffect(() => {
        fetchComments();
    }, []);

    const fetchComments = async () => {
        try {
            const response = await api.get(`/api/v1/Comments/ByTaskId/${task.taskItemId}`, {
                headers: {
                    Authorization: `Bearer ${token}`,
                },
            });
    
            if (response.status === 200) {
                setComments(response.data.comments);
            } else {
                console.error('Error fetching comments:', response);
                toast.error(response);
            }
        } catch (error) {
            console.error('Error fetching comments:', error);
            toast.error(response);
        }
    };

    const handleAddComment = async () => {
        try {
            const response = await api.post('/api/v1/Comments', {
                createdBy: currentUser.username,
                commentText: newCommentText,
                taskId: task.taskItemId,
                lastModifiedBy: currentUser.username,
            }, {
                headers: {
                    Authorization: `Bearer ${token}`,
                },
            });

            if (response.status === 200) {
                setNewCommentText('');
                toast.success("Added comment successfully!")
                fetchComments();
            } else {
                console.error('Error adding comment:', response);
                toast.error(response);
            }
        } catch (error) {
            console.error('Error adding comment:', error);
            toast.error(response);
        }
    };

    const handleOnUserAvatarClick = (e, userId) => {
        e.stopPropagation();
        userId && navigate(`/dashboard/profile/${userId}`)
    }
    const handleDeleteComment = async (commentId) => {
        try{
            const response = await api.delete(`/api/v1/Comments/${commentId}`, {
                headers: {
                    Authorization: `Bearer ${token}`,
                },
                data:{
                    commentId: commentId,
                    owner: currentUser.username
                }
            });
            if (response.status === 200) {
                toast.success('Comment deleted successfully');
                fetchComments();
            } else {
                console.error('Error deleting comment:', response);
                toast.error('Failed to delete comment');
            } 
        }catch (error) {
            console.error('Error deleting comment:', error);
            toast.error('Failed to delete comment');
        }
    }
    return (
        <div className='flex flex-col mt-4 px-1'>
            <div className='flex items-center mb-2'>
                <ModeCommentIcon className='text-secondary' fontSize='extraSmall'></ModeCommentIcon>
                <p className='text-gray-300 ml-1 text-md font-semibold'>
                    Comments
                </p>
            </div>

            <div className="text-surface-light overflow-auto max-h-[14rem]" style={{scrollbarWidth: 'thin'}}>
                {comments.map((comment, i) => (
                  <Card
                    className={`${comments.length - 1 !== i && "mb-2"} md:w-4/5 !rounded-lg`}
                    key={comment.commentId}
                  >
                      <CardContent className='p-2 bg-surface-darkest'>
                          <div className={'flex items-center'}>
                              <div
                                className={'flex items-center cursor-pointer'}
                                onClick={(e) => handleOnUserAvatarClick(e, comment.createdBy?.userId)}
                              >
                                  <UserAvatar
                                    photoUrl={comment.createdBy?.userPhoto?.photoUrl}
                                    className={"w-[1.75rem] h-[1.75rem] rounded-full"}
                                    loadingClassName={"w-[1.75rem] h-[1.75rem] bg-surface-mid-dark rounded-full"}
                                    loadingProps={{className: "w-5 h-5"}}
                                  />
                                  <p className="text-gray-300 text-sm ml-2.5">
                                      {comment.createdBy.name}
                                  </p>
                              </div>
                              <p className="text-surface-mid-light text-xs ml-auto">
                                  {formatTime(comment.createdDate)}
                              </p>
                          {comment.createdBy.name === currentUser.username && <DeleteIcon className="text-secondary hover:text-red-900" fontSize='medium' onClick={() => handleDeleteComment(comment.commentId)}/>}

                          </div>
                          <p className="text-gray-500 text-xs mt-4">
                              {comment.commentText}
                          </p>
                      </CardContent>
                  </Card>
                ))}
            </div>

            <div className="flex items-center flex-col mt-2 md:w-4/5">
                <ErgoInput
                  placeholder={"Comment"}
                  value={newCommentText}
                  onChange={(val) => setNewCommentText(val)}
                  icon={
                      <i
                        className={"fa-regular fa-paper-plane cursor-pointer text-surface-mid-light hover:text-surface-light"}
                        onClick={handleAddComment}
                      />
                  }
                />
            </div>
        </div>
    );
};

export default CommentSection;

import React, { useState, useEffect } from 'react';
import { Modal, Fade, Card, CardContent } from '@mui/material';
import ModeCommentIcon from '@mui/icons-material/ModeComment';
import { Button, Typography } from '@material-tailwind/react';
import api from '@/services/api';
import { useUser } from "@/context/LoginRequired";
import { toast } from "react-toastify";
import { Textarea } from '@material-tailwind/react';

const CommentSection = ({ task, token }) => {
    const [comments, setComments] = useState([]);
    const [newCommentText, setNewCommentText] = useState('');
    const [isCommentVisible, setIsCommentVisible] = useState(false);
    const currentUser = useUser();

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
                setComments([...comments, response.data]);
                setNewCommentText('');
                toast.success("Added comment successfully!")
                fetchComments();
                setIsCommentVisible(!isCommentVisible)
            } else {
                console.error('Error adding comment:', response);
                toast.error(response);
            }
        } catch (error) {
            console.error('Error adding comment:', error);
            toast.error(response);
        }
    };

    return (
        <div className='flex flex-col mt-4 px-1'>
            <div className='flex items-center mb-2'>
                <ModeCommentIcon className='text-secondary' fontSize='extraSmall'></ModeCommentIcon>
                <p className='text-gray-300 ml-1 text-md font-semibold'>
                    Comments
                </p>
            </div>
            
            <div className="text-surface-light overflow-auto max-h-[12rem]" style={{ scrollbarWidth: 'thin' }}>
                {comments.map(comment => (
                    <Card key={comment.commentId} className="cursor-pointer mb-2 rounded w-4/5" 
                    >
                        <CardContent className='p-2 pb-0 rounded bg-surface-darkest' >
                            <div className='flex'>
                                <div>
                                    <p className="text-gray-300  text-xs">
                                        #{comment.createdBy} commented:
                                    </p>
                                    <p className="text-gray-500 text-xs mt-2">
                                        {comment.commentText}
                                    </p>
                                    <p className="text-surface-mid-light text-xs mt-2">
                                        {formatTime(comment.createdDate)}
                                    </p>
                                </div>
                            </div>
                        </CardContent>
                    </Card>
                ))}
            </div>
            
            {isCommentVisible && (
                <div className="flex items-center flex-col mt-4">
                    <Textarea
                        placeholder="Comment"
                        className="!border-surface-mid-dark text-surface-light focus:!border-secondary"
                        labelProps={{
                            className: "before:content-none after:content-none",
                        }}
                        onChange={(e) => setNewCommentText(e.target.value)}
                        value={newCommentText}
                        rows={1}
                    />
                    
                </div>
            )}
            
            <div className="flex items-center mt-2">
                <p className="text-sm text-surface-light mt-2 hover:opacity-70 hover:cursor-pointer" 
                   onClick={() => setIsCommentVisible(!isCommentVisible)}
                >
                    {isCommentVisible ? 'Cancel' : 'Add your comment'}
                </p>
                {isCommentVisible ? (
                <p className="text-sm text-secondary hover:opacity-70 ml-2 mt-2 hover:cursor-pointer" size="sm" onClick={handleAddComment}>
                        Confirm
                </p>) : null}
                
            </div>
        </div>
    );
};

export default CommentSection;

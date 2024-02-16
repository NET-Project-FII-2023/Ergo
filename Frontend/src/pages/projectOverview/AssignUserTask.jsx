import React, { useState, useEffect } from 'react';
import {Button,Select,Option} from '@material-tailwind/react';
import api from '@/services/api';
import AssignmentIndIcon from '@mui/icons-material/AssignmentInd';
import { Card, CardContent } from '@mui/material';
import {toast} from "react-toastify";
import { useUser } from '../../context/LoginRequired';
import PersonAddIcon from '@mui/icons-material/PersonAdd';
import UserAvatar from "@/common/components/UserAvatar";
import {useNavigate} from "react-router-dom";
import DeleteIcon from '@mui/icons-material/Delete';
import { sendNotification } from '@/services/notifications/sendNotification';


const AssignUserTask = ({ token, task, project }) => {
    const navigate = useNavigate();
    const currentUser = useUser();
    const [showSelect, setShowSelect] = useState(false);
    const [users, setUsers] = useState([]);
    const [selectedUserId, setSelectedUserId] = useState('');
    const [selectedUser, setSelectedUser] = useState('');
    const [selectVisible, setSelectVisible] = useState(false);
    const [showButtons, setShowButtons] = useState(false);
    const [loadedAssignedUser, setLoadedAssignedUser] = useState({});

    const fetchCurrentTask = async () => {
        try {
          const response = await api.get(`/api/v1/TaskItems/${task.taskItemId}`, {
            headers: {
              Authorization: `Bearer ${token}`,
            },
          });
    
          if (response.status === 200) {
            setLoadedAssignedUser(response.data.taskItem.assignedUser);
          } else {
            console.error('Error fetching tasks:', response);
          }
        } catch (error) {
          console.error('Error fetching tasks:', error);
        }
      };

      useEffect(() => {
        fetchCurrentTask();
    }, []);

    const handleClick = async () => {
        try {
            const response = await api.get(`/api/v1/Users/ByProjectId/${project.projectId}`, {
                headers: {
                    Authorization: `Bearer ${token}`,
                },
            });

            if (response.status === 200) {
                setUsers(response.data.users);
                setShowSelect(true);
                setSelectVisible(true);
                setShowButtons(true);
            } else {
                console.error('Error fetching users:', response);
            }
        } catch (error) {
            console.error('Error fetching users:', error);
        }
    };

    const handleConfirmAssign = async () => {
      try {
        const response = await api.post(`/api/v1/TaskItems/AssignUser`, {
          taskItemId: task.taskItemId,
          userId: selectedUserId
        }, {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        });

        if (response.status !== 200) {
          throw new Error(response);
        }
        setShowSelect(false);
        setSelectVisible(false);
        setShowButtons(false);
        toast.success('User assigned successfully!');

        //send notification to the user
        await sendNotification(selectedUserId, `You have been assigned to task ${task.taskName}`, token);

        fetchCurrentTask();
      } catch (error) {
        console.error('Error assigning user:', error);
        toast.error('Error assigning user:' + error);
      }
    };

    const handleUserChange = (event) => {
        setSelectedUserId(event.target.value);
        setSelectedUser(event.target.name);
        setSelectVisible(false);
        setShowButtons(true);
    };

    const handleCancel = () => {
        setShowSelect(false);
        setShowButtons(false);
    };

    const handleAssignedUserClick = (e, userId) => {
        e.stopPropagation();
        userId && navigate(`/dashboard/profile/${userId}`)
    }
    const handleUnassignUser = async () => {
        try{
            const response = await api.post(`/api/v1/TaskItems/UnassignUser`, {
                taskItemId: task.taskItemId,
                owner: currentUser.username

            }, {
                headers: {
                    Authorization: `Bearer ${token}`,
                },
            });

            if (response.status === 200) {
                console.log('User unassigned successfully');
                toast.success('User unassigned successfully!');
                fetchCurrentTask();
            } else {
                console.error('Error unassigning user:', response);
                toast.error(response);
            }
        }catch (error) {
            console.error('Error unassigning user:', error);
            toast.error('Error unassigning user:' + error);
        }
    }

    return (
        <div className='mt-6'>    
            <div className='flex items-center mb-3'>
                <AssignmentIndIcon fontSize='sm' className='text-secondary mr-1'></AssignmentIndIcon>
                <p className='text-gray-300 ml-1 text-md font-semibold'>
                    Assigned Member
                </p>
            </div>
            <div>
            {!showSelect ? (
                <div>   
                    {!loadedAssignedUser ? (
                    project.createdBy === currentUser.username &&
                    <Button onClick={handleClick} className='text-surface-light bg-surface-darkest hover:opacity-70'>
                        <PersonAddIcon fontSize='small'></PersonAddIcon>
                    </Button>
                    ) : (<Card className="opacity-80 cursor-pointer mb-2 rounded"
                    style={{
                        backgroundColor: "#1a1625",
                    }}>
                        <CardContent className='rounded bg-surface-darkest !p-4'>
                            <div className='flex justify-between items-center'>
                                <div className='flex'>
                                <UserAvatar
                                  onClick={(e) => handleAssignedUserClick(e, loadedAssignedUser?.userId)}
                                  photoUrl={loadedAssignedUser?.userPhoto?.photoUrl}
                                  className={"w-[2.25rem] h-[2.25rem] rounded-full mr-3"}
                                  loadingClassName={"w-[2.25rem] h-[2.25rem] bg-surface-mid-dark rounded-full mr-3"}
                                  loadingProps={{className: "w-5 h-5"}}
                                />
                                <div>
                                    <p className="text-surface-light text-sm">
                                        {loadedAssignedUser.name}
                                    </p>
                                    <p className="text-surface-mid-light text-xs">
                                        @{loadedAssignedUser.username}
                                    </p>
                                </div>
                                </div>
                                
                            {project.createdBy === currentUser.username && <DeleteIcon className="text-secondary hover:text-red-900" onClick={handleUnassignUser}/>}
                            </div>
                        </CardContent>
                    </Card>)}
                    
                </div>
            ) : (  
                <div>
                    <Select
                        value={selectedUser}
                        onChange={handleUserChange}
                        className="!border-surface-mid-dark text-surface-light focus:!border-secondary"
                        labelProps={{
                            className: "before:content-none after:content-none",
                        }}
                        open={selectVisible}
                        onClose={() => setSelectVisible(false)}
                    >
                        {users.map((user) => (
                            <Option key={user.userId} value={user.userId} onClick={() => setSelectedUserId(user.userId)}>
                                <span >{user.name}</span> (@{user.username})
                            </Option>
                        ))}
                    </Select>
                    {showButtons && (
                        <div className='flex'>
                            <Button size="sm" className="bg-surface-darkest text-surface-light hover:opacity-70 mt-2" onClick={handleConfirmAssign}>
                                Confirm
                            </Button>
                            <Button size ="sm" className="bg-gray-400 text-surface-darkest hover:bg-gray-400 ml-1 mt-2 hover:opacity-70" onClick={handleCancel}>
                                Cancel
                            </Button>
                        </div>
                    )}
                </div>
            )}
        </div>
        </div>
    );
};

export default AssignUserTask;

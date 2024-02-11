import React, { useState, useEffect } from 'react';
import { Button } from '@material-tailwind/react';
import { Select, Option } from '@material-tailwind/react';
import api from '@/services/api';
import AssignmentIndIcon from '@mui/icons-material/AssignmentInd';
import { Card, CardContent } from '@mui/material';
import {toast} from "react-toastify";
import { useUser } from '../../context/LoginRequired';
import PersonAddIcon from '@mui/icons-material/PersonAdd';


const AssignUserTask = ({ token, task, project }) => {
    const [showSelect, setShowSelect] = useState(false);
    const [users, setUsers] = useState([]);
    const [selectedUserId, setSelectedUserId] = useState('');
    const [selectedUser, setSelectedUser] = useState('');
    const [selectVisible, setSelectVisible] = useState(false);
    const [showButtons, setShowButtons] = useState(false);
    const [loadedAssignedUser, setLoadedAssignedUser] = useState({});
    const currentUser = useUser();

    const fetchCurrentTask = async () => {
        try {
    
          const response = await api.get(`/api/v1/TaskItems/${task.taskItemId}`, {
            headers: {
              Authorization: `Bearer ${token}`,
            },
          });
    
          if (response.status === 200) {
            setLoadedAssignedUser(response.data.taskItem.assignedUser);
            console.log(response.data.taskItem.assignedUser);
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
            const response = await api.get('/api/v1/Users', {
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

            if (response.status === 200) {
                setShowSelect(false);
                setSelectVisible(false);
                setShowButtons(false);
                console.log('User assigned successfully');
                toast.success('User assigned successfully!');
                fetchCurrentTask();
            } else {
                console.error('Error assigning user:', response);
                toast.error(response);
            }
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
                        <CardContent className='rounded bg-surface-darkest'>
                            <div className='flex'>
    
                                <div>
                                    <p className="text-surface-light text-xs">
                                    @{loadedAssignedUser.username}
                                    </p>
                                    <p className="text-surface-mid-light text-xs">
                                        {loadedAssignedUser.name}
                                    </p>
                                </div>
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

import React, { useState, useEffect } from 'react';
import { Button } from '@material-tailwind/react';
import api from '@/services/api';
import AssignmentIndIcon from '@mui/icons-material/AssignmentInd';
import { Select, Option} from '@material-tailwind/react';
import { Card, CardContent } from '@mui/material';

const AssignUserTask = ({token, task, projectId}) => {
    const [showSelect, setShowSelect] = useState(false);
    const [users, setUsers] = useState([]);
    const [selectedUserId, setSelectedUserId] = useState('');
    const [selectedUser, setSelectedUser] = useState('');
    const [selectVisible, setSelectVisible] = useState(false);
    const [showButtons, setShowButtons] = useState(false);

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

    return(
        <div className='mt-6'>    
            <div className='flex items-center mb-3'>
                <AssignmentIndIcon fontSize='sm' className='text-secondary mr-1'></AssignmentIndIcon>
                <p className='text-gray-300 ml-1 text-md font-semibold'>
                    Assigned Member
                </p>
            </div>
            <div>
            {!showSelect ? (
                <>
                 <Card className="opacity-80 cursor-pointer mb-2 rounded"
                 style={{
                    backgroundColor: "#1a1625",
                  }}>
                    <CardContent className='p-2 rounded bg-surface-darkest'>
                        <div className='flex'>
                            <span className='w-[2rem] h-[2rem] rounded-full bg-surface-light mr-3'>

                            </span>
                            <div>
                                <p className="text-surface-light text-xs">
                                    #costel
                                </p>
                                <p className="text-surface-mid-light text-xs">
                                    Costel Biju
                                </p>
                            </div>
                        </div>
                
                    </CardContent>
                </Card>
                
                <Button onClick={handleClick} className='text-surface-light bg-surface-darkest hover:opacity-70'>
                    Assign Member
                </Button>
            </>
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
                                <span >{user.name}</span> (#{user.username})
                            </Option>
                        ))}
                    </Select>
                    {showButtons && (
                        <div className='flex'>
                            <Button size="sm" className="bg-surface-darkest text-surface-light hover:opacity-70 mt-2">
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
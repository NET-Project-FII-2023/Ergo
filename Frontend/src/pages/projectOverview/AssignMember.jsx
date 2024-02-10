import React, { useState, useEffect } from 'react';
import { Button } from '@material-tailwind/react';
import { Select, Option } from '@material-tailwind/react';
import api from '@/services/api';
import { toast } from "react-toastify";

const AssignMember = ({ projectId, token, onMemberAssigned }) => {
    const [showSelect, setShowSelect] = useState(false);
    const [users, setUsers] = useState([]);
    const [selectedUserId, setSelectedUserId] = useState('');
    const [selectedUser, setSelectedUser] = useState('');
    const [selectVisible, setSelectVisible] = useState(false);
    const [showButtons, setShowButtons] = useState(false);
     const [members, setMembers] = useState([]);

     const fetchUsers = async () => {
        try {
   
            const usersResponse = await api.get('/api/v1/Users', {
                headers: {
                    Authorization: `Bearer ${token}`,
                },
            });
    
            const membersResponse = await api.get(`/api/v1/Users/ByProjectId/${projectId}`, {
                headers: {
                    Authorization: `Bearer ${token}`,
                },
            });
    
            if (usersResponse.status === 200 && membersResponse.status === 200) {
                const allUsers = usersResponse.data.users;
                console.log("All USERS:", allUsers)

                const assignedUserIds = membersResponse.data.users.map(user => user.userId);
                console.log("ass user ids:", assignedUserIds)
                const nonAssignedUsers = allUsers.filter(user => !assignedUserIds.includes(user.userId));
                console.log("NONASSIGNED USERS:", nonAssignedUsers)
                setUsers(nonAssignedUsers);
                setSelectVisible(true);
                setShowButtons(true);
            } else {
                console.error('Error fetching users or members');
            }
        } catch (error) {
            console.error('Error fetching users or members:', error);
        }
    };

    useEffect(() => {
        fetchUsers();
    }, [projectId])

    const handleConfirmAssign = async () => {
        try {
            const response = await api.post(`/api/v1/Projects/AssignUserToProject`, {
                userId: selectedUserId,
                projectId: projectId
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
                toast.success('User assigned successfully');
                onMemberAssigned();
                fetchUsers();

            } else {
                console.error('Error assigning user:', response);
                toast.error('Error assigning user:' +  response);
            }
        } catch (error) {
            console.log("User id", selectedUserId);
            console.log("project id", projectId);
            console.error('Error assigning user:', error);
            toast.error('Error assigning user:' +  error);
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
        <div>
            {users.length > 0 ? (
                <>
                 {!showSelect ? (
                    <Button onClick={() => {fetchUsers(); setShowSelect(true);}} className='w-full bg-surface-dark text-surface-light hover:opacity-70 hover:text-gray-100'>
                        Assign
                    </Button>
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

                        <div className='flex items-center justify-end'>
                            <div className="text-sm text-surface-light hover:text-gray-100 hover:cursor-pointer mt-3 mr-2" onClick={handleCancel}>
                                Cancel
                            </div>
                            <Button size="sm" className="bg-secondary hover:bg-primary mt-2" onClick={handleConfirmAssign}>
                                Confirm
                            </Button>
                            
                        </div>
                </div>
                 )}
                </>
            ) : (<div>pula</div>)}
        </div>
    );
    
};

export default AssignMember;

import React, { useState } from 'react';
import { Button } from '@material-tailwind/react';
import { Select, Option } from '@material-tailwind/react';
import api from '@/services/api';
import { toast } from "react-toastify";

const AssignMember = ({ projectId, token }) => {
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

            } else {
                console.log("User id", selectedUserId);
                console.log("project id", projectId);
                console.error('Error assigning user:', response);
            }
        } catch (error) {
            console.log("User id", selectedUserId);
                console.log("project id", projectId);
            console.error('Error assigning user:', error);
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
            {!showSelect ? (
                <Button onClick={handleClick} className='text-gray-300 hover:text-surface-light'>
                    Assign Member
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
                        open={selectVisible} // Control select visibility
                        onClose={() => setSelectVisible(false)} // Close select when closed
                    >
                        {users.map((user) => (
                            <Option key={user.userId} value={user.userId} onClick={() => setSelectedUserId(user.userId)}>
                                <span >{user.name}</span> (#{user.username})
                            </Option>
                        ))}
                    </Select>
                    {showButtons && (
                        <div className='flex'>
                            <Button size="sm" className="bg-secondary hover:bg-primary mt-2" onClick={handleConfirmAssign}>
                                Confirm
                            </Button>
                            <Button size ="sm" className="bg-gray-300 text-surface-darkest hover:bg-gray-400 ml-1 mt-2" onClick={handleCancel}>
                                Cancel
                            </Button>
                        </div>
                    )}
                </div>
            )}
        </div>
    );
};

export default AssignMember;

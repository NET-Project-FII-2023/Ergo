import React from 'react';
import api from "@/services/api";
import PersonRemoveIcon from '@mui/icons-material/PersonRemove';
import { toast } from "react-toastify";


const DeleteMemberProject = ({project, token, currentUser, member, fetchMembers}) => {
  

    const handleUnassign = async (userId) => {
        try {
            const response = await api.post('/api/v1/Projects/DeleteUserFromProject', {
                ownerUsername: project.createdBy,
                userId: userId,
                projectId: project.projectId,
            }, {
                headers: {
                    Authorization: `Bearer ${token}`,
                },
            });
    
            if (response.status === 200) {
                fetchMembers();
                console.log("User unassigned successfully");
                toast.success("User removed successfully!")
            } else {
                console.error('Error unassigning user:', response);
                toast.error(response);
            }
        } catch (error) {
            console.error('Error unassigning user:', error);
            toast.error(response);
        }
    };

    return(
        <>
            {member.username !== project.createdBy && member.username !== currentUser.username && project.createdBy === currentUser.username && (
                <PersonRemoveIcon fontSize="extraSmall" className="text-gray-400 ml-auto hover:text-red-300" onClick={() => handleUnassign(member.userId)}>x</PersonRemoveIcon>
            )}
        </>
    );
}

export default DeleteMemberProject;
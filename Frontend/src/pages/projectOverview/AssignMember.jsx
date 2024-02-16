import React, {useState} from 'react';
import {Button} from '@material-tailwind/react';
import api from '@/services/api';
import {toast} from "react-toastify";
import PersonAddIcon from '@mui/icons-material/PersonAdd';
import AssignMemberSearch from "@/pages/projectOverview/AssignMemberSearch";
import { sendNotification } from "@/services/notifications/sendNotification";

const AssignMember = ({project, token, onMemberAssigned}) => {
  const [showSelect, setShowSelect] = useState(false);

  const handleConfirmAssign = async (selectedUser) => {
    setShowSelect(false);
    if (!selectedUser.userId) {
      toast.error('No user selected');
      return;
    }

    try {
      const response = await api.post(`/api/v1/Projects/AssignUserToProject`, {
        userId: selectedUser.userId,
        projectId: project.projectId
      }, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      });

      if (response.status !== 200) {
        throw new Error(response);
      }

      setShowSelect(false);
      toast.success('User assigned successfully');

      //send notification to the user
      await sendNotification(selectedUser.userId, `You have been assigned to project ${project.projectName}`, token);

      onMemberAssigned();
    } catch (error) {
      toast.error('Error assigning user:' + error);
    }
  };

  const handleCancel = () => {
    setShowSelect(false);
  };

  return (
    <div>
      {!showSelect ? (
        <Button
          onClick={() => setShowSelect(true)}
          className='w-full bg-surface-dark text-surface-light hover:opacity-70 hover:text-gray-100'
        >
          <PersonAddIcon fontSize='small'></PersonAddIcon>
        </Button>
      ) : (
        <div>
          <AssignMemberSearch
            assignedMembers={project.members}
            onUserSelect={handleConfirmAssign}
          />
          <div className='flex items-center justify-end'>
            <Button size="sm" className="bg-surface-mid-light hover:bg-surface-mid mt-2" onClick={handleCancel}>
              Cancel
            </Button>
          </div>
        </div>)}
    </div>
  );

};

export default AssignMember;

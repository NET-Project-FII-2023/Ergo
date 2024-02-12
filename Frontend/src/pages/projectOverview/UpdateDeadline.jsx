import React, { useState } from 'react';
import { Button, TextField } from '@mui/material';
import AccessTimeIcon from '@mui/icons-material/AccessTime';
import { toast } from 'react-toastify';
import api from '@/services/api';

const UpdateDeadline = ({ selectedTask, token, fetchCurrentTask }) => {
  const [updatedDeadline, setUpdatedDeadline] = useState(selectedTask.deadline);

  const handleUpdateDeadline = async () => {
    try {
      const formattedDeadline = new Date(updatedDeadline).toISOString();
      const response = await api.put(
        `/api/v1/TaskItems/${selectedTask.taskItemId}`,
        {
          ...selectedTask,
          deadline: formattedDeadline,
        },
        {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        }
      );

      if (response.status === 200) {
        toast.success('Deadline updated successfully');
        fetchCurrentTask();
      } else {
        console.error('Error updating deadline:', response);
        toast.error('Failed to update deadline');
      }
    } catch (error) {
      console.error('Error updating deadline:', error);
      toast.error('Failed to update deadline');
    }
  };

  return (
    <div className="flex items-center px-2 mt-6">
      <AccessTimeIcon className="text-secondary" fontSize="extraSmall" />
      <TextField
        type="date"
        value={updatedDeadline}
        onChange={(e) => setUpdatedDeadline(e.target.value)}
        InputLabelProps={{
          shrink: true,
        }}
        className="text-surface-light text-sm ml-1"
      />
      <Button
        onClick={handleUpdateDeadline}
        className="ml-2"
        variant="contained"
        color="primary"
      >
        Save
      </Button>
    </div>
  );
};

export default UpdateDeadline;

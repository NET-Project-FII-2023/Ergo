
import React, { useEffect, useState } from 'react';
import { Modal } from '@mui/material';
import { Button, Typography, Select, Option } from "@material-tailwind/react";
import api from '@/services/api';
import { useUser } from '../../context/LoginRequired';
import { toast } from "react-toastify";
import { Card, CardContent } from '@mui/material';

const EditStateModal = ({ open, onClose, projectId, token, selectedTask, onTaskStateUpdated  }) => {
  const [selectedState, setSelectedState] = useState('');
  const currentUser = useUser();

  const handleStateChange = (value) => {
    setSelectedState(value);
  };

  const handleSave = async () => {
    try {
      const response = await api.put(`/api/v1/TaskItems/${projectId}`, {
        taskItemId: selectedTask.taskItemId,
        taskName: selectedTask.taskName,
        description: selectedTask.description,
        deadline: selectedTask.deadline,
        createdBy: selectedTask.createdBy,
        projectId: selectedTask.projectId,
        state: selectedState,
        branch: selectedTask.branch,
      }, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      });

      if (response.status === 200) {
        onTaskStateUpdated();
        toast.success('Task state updated successfully');
        onClose();
      } else {
        console.error('Error updating task state:', response);
        toast.error('Failed to update task state');
      }
    } catch (error) {
      console.error('Error updating task state:', error);
      toast.error('Failed to update task state');
    }
  };

  return (
    <Modal open={open} onClose={onClose}>
      <div className="absolute top-1/2 left-1/2 transform -translate-x-1/2 -translate-y-1/2 w-[25rem] bg-[#2f2b3a] shadow-lg p-4 rounded">
        <Typography variant='h4' className='text-white p-2'>
          Edit State
        </Typography>
        <div className='flex flex-col justify-between h-full'>
          <div className='m-2'>
            <Typography variant='body2' className='text-surface-light mb-1'>
              Select Task State:
            </Typography>
            <Select
              value={selectedState}
              onChange={(value) => handleStateChange(value)}
              className="!border-surface-mid-dark text-surface-light focus:!border-secondary"
              labelProps={{
                className: "before:content-none after:content-none",
              }}
              placeholder='Select State'
            >
              <Option value={1} className='text-surface-mid-light'>To Do</Option>
              <Option value={2} className='text-surface-mid-light'>In Progress</Option>
              <Option value={3} className='text-surface-mid-light'>Done</Option>
            </Select>
          </div>
          <div className='m-2 self-end'>
            <Button size="sm" className="bg-secondary hover:bg-primary" onClick={handleSave}>
              Save
            </Button>
          </div>
        </div>
      </div>
    </Modal>
  );
};

export default EditStateModal;
import React, { useState, useEffect } from 'react';
import { Typography } from '@material-tailwind/react';
import DescriptionIcon from '@mui/icons-material/Description';
import { toast } from "react-toastify";
import api from '@/services/api';
import SaveAsIcon from '@mui/icons-material/SaveAs';

const TaskMainInfo = ({ selectedTask, setSelectedTask, token }) => {
  const [editMode, setEditMode] = useState({taskName: false, description: false});
  const [updatedTaskName, setUpdatedTaskName] = useState(selectedTask.taskName);
  const [updatedDescription, setUpdatedDescription] = useState(selectedTask.description);
  const [currentTask, setCurrentTask] = useState([]);

  const handleDoubleClick = (field) => {
    setEditMode(prevState => ({...prevState, [field]: true}));
  };

  const fetchCurrentTask = async () => {
    try {
      const response = await api.get(`/api/v1/TaskItems/${selectedTask.taskItemId}`, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      });

      if (response.status === 200) {
        setCurrentTask(response.data.taskItem);
        setSelectedTask(response.data.taskItem);
      } else {
        console.error('Error fetching tasks:', response);
      }
    } catch (error) {
      console.error('Error fetching tasks:', error);
    }
  };

  useEffect(() => {
    fetchCurrentTask();
  }, [editMode]);

  const handleUpdateTask = async () => {
    try {
      const response = await api.put(`/api/v1/TaskItems/${selectedTask.taskItemId}`, {
        taskItemId: selectedTask.taskItemId,
        taskName: updatedTaskName,
        description: updatedDescription,
        deadline: selectedTask.deadline,
        createdBy: selectedTask.createdBy,
        projectId: selectedTask.projectId,
        state: selectedTask.state,
        branch: selectedTask.branch,
      }, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      });

      if (response.status === 200) {
        toast.success('Task updated successfully');
        fetchCurrentTask();
      } else {
        console.error('Error updating task:', response);
        toast.error('Failed to update task');
      }
    } catch (error) {
      console.error('Error updating task:', error);
      toast.error('Failed to update task');
    }
  };

  const handleSave = () => {
    handleUpdateTask();
    setEditMode({taskName: false, description: false});
  };

  return (
    <div>
      <div className="flex flex-col px-2 mt-2">
        {editMode.taskName ? (
          <div className='flex items-center'>
          <input
            type="text"
            value={updatedTaskName}
            onChange={(e) => setUpdatedTaskName(e.target.value)}
            className="bg-transparent text-white text-3xl focus:outline-none mb-2"
          />
          <p onClick={handleSave}>
            <SaveAsIcon fontSize='medium' className='text-surface-light hover:text-secondary ml-1 cursor-pointer'/></p>
          </div>

        ) : (
          <Typography variant='h4' className='text-white py-2' onDoubleClick={() => handleDoubleClick('taskName')}>
            {currentTask.taskName}
          </Typography>
        )}
        {currentTask.state === 1 && (
          <div className="w-[3rem] text-xs text-center rounded-md text-sm text-surface-darkest bg-secondary p-1">To Do</div>
        )}
        {currentTask.state === 2 && (
          <div className="w-[5rem] text-xs text-center rounded-md text-sm text-surface-darkest bg-blue-400 p-1">In Progress</div>
        )}
        {currentTask.state === 3 && (
          <div className="w-[3rem] text-xs text-center rounded-md text-sm text-surface-darkest bg-teal-300 p-1">Done</div>
        )}
      </div>
      <div className='flex flex-row mt-4 pr-2 items-center'>
        {editMode.description ? (
          <textarea
            value={updatedDescription}
            onChange={(e) => setUpdatedDescription(e.target.value)}
            className="bg-transparent text-white text-lg focus:outline-none resize-none border-2 p-4 border-surface-mid"
            style={{ width: '80%', height: '100px', outline: 'none' }}
          />
        ) : (
          <div>
            <div className='flex flex-row items-center'>
              <DescriptionIcon className='text-secondary ml-1' fontSize='extraSmall'></DescriptionIcon>
              <p
                className='text-gray-300 ml-1 text-md font-semibold'    
              >
                Description
              </p>
            </div>
            <p variant="body2" className='text-surface-light pl-2 py-2 pr-12 text-lg' onDoubleClick={() => handleDoubleClick('description')}>
              {currentTask.description}
            </p>
          </div>
        )}
      </div>
      {editMode.taskName || editMode.description && (
        <button onClick={handleSave}>
          <SaveAsIcon fontSize='medium' className='text-surface-light mt-1 cursor-pointer hover:text-secondary'/>
        </button>
      )}
    </div>
  );
};

export default TaskMainInfo;

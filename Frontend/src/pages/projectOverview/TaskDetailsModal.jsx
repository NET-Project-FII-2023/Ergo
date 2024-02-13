import React, { useState, useEffect } from 'react';
import { Modal, Fade, MenuItem } from '@mui/material'; // Import Select and MenuItem
import AccessTimeIcon from '@mui/icons-material/AccessTime';
import { Typography, Button, TextField } from '@mui/material';
import CommentSection from './CommentSection';
import AttachmentSection from './AttachmetSection';
import TimerSection from './TimerSection';
import GithubSection from './GithubSection';
import AssignUserTask from './AssignUserTask';
import TaskMainInfo from './TaskMainInfo';
import api from '@/services/api';
import { toast } from "react-toastify";
import {Select, Option} from "@material-tailwind/react";


const formatDeadline = (deadline) => {
  const options = { day: 'numeric', month: 'long', year: 'numeric' };
  const formattedDeadline = new Date(deadline).toLocaleDateString(undefined, options);
  return formattedDeadline;
};

const TaskDetailsModal =  ({ modalOpen, handleCloseModal, selectedTask, token, project,fetchCurrentProject })  => {
  const [attachedFiles, setAttachedFiles] = useState([]);
  const [currentTask, setCurrentTask] = useState([]);
  const [updatedDeadline, setUpdatedDeadline] = useState();
  const [editMode, setEditMode] = useState(false);
  const [selectedSection, setSelectedSection] = useState('TaskMainInfo');

   const handleSectionChange = (value) => {
    setSelectedSection(value);
  };


  const handleFileInputChange = (event) => {
    const files = event.target.files;
    setAttachedFiles([...attachedFiles, ...files]);
  };

  const handleUpdateDeadline = async () => {
    try {
      const formattedDeadline = new Date(updatedDeadline).toISOString();
      const response = await api.put(`/api/v1/TaskItems/${selectedTask.taskItemId}`, {
        taskItemId: selectedTask.taskItemId,
        taskName: selectedTask.taskName,
        description: selectedTask.description,
        createdBy: selectedTask.createdBy,
        projectId: selectedTask.projectId,
        state: selectedTask.state,
        branch: selectedTask.branch,
        deadline: formattedDeadline,
      }, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      });
  
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
  

  const fetchCurrentTask = async () => {
      try {

        const response = await api.get(`/api/v1/TaskItems/${selectedTask.taskItemId}`, {
          headers: {
            Authorization: `Bearer ${token}`,
          },
        });

        if (response.status === 200) {
          setCurrentTask(response.data.taskItem);
          setUpdatedDeadline(response.data.taskItem.deadline);
          console.log(response.data.taskItem);
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

  return (
    <Modal
      open={modalOpen}
      onClose={handleCloseModal}
      aria-labelledby="modal-title"
      aria-describedby="modal-description"
      closeAfterTransition
    >
      <Fade in={modalOpen}>
        <div className="absolute top-1/2 left-1/2 transform -translate-x-1/2 -translate-y-1/2 md:w-[60rem] w-[90vw] bg-[#2f2b3a] shadow-lg p-8 rounded outline-none">
          {selectedTask && (
            <>
              <div className='hidden md:visible md:flex md:flex-row'>
                <div className='w-2/3'>
                  <TaskMainInfo selectedTask={selectedTask} token={token} onClose={handleCloseModal}/>
                  <AttachmentSection attachedFiles={attachedFiles} handleFileInputChange={handleFileInputChange} project={project}/>
                  {attachedFiles.map((file, index) => (
                    <Typography key={index} variant="body2" className="text-surface-light px-2 text-md">
                      {file.name}
                    </Typography>
                  ))}
                  <CommentSection token={token} task={selectedTask} />
                  <div className="flex items-center px-2 mt-6">
                    <AccessTimeIcon className="text-secondary mr-2" fontSize='extraSmall' />
                    <Typography className='text-surface-light text-sm ml-1'>{formatDeadline(selectedTask.deadline)}</Typography>
                  </div>
                </div>
                <div className="border-r border-1 border-surface-mid h-auto"></div>
                <div className="w-1/3 ml-4 p-4">
                  <TimerSection task={selectedTask} token={token} project={project}/>
                  <GithubSection token={token} task={selectedTask} project={project} />
                  <AssignUserTask token={token} task={selectedTask} project={project} />
                </div>
              </div>
              <div className='md:hidden visible'>
                <Select 
                  value={selectedSection} 
                  onChange={handleSectionChange}
                  className="!border-surface-mid-dark mb-3 text-surface-light focus:!border-secondary"
                  labelProps={{
                    className: "before:content-none after:content-none",
                  }}
                >
                <Option value="TaskMainInfo">Details</Option>
                <Option value="Comments">Comment Section</Option>
                <Option value="Attachments">Attachments</Option>
                <Option value="ProgressTracking">Progress Tracking</Option>

              </Select>
                {selectedSection === 'TaskMainInfo' && (
                  <>
                    <TaskMainInfo selectedTask={selectedTask} token={token}/>
                    <AssignUserTask token={token} task={selectedTask} project={project} />
                  </>)
                }
                {selectedSection === 'Comments' && <CommentSection token={token} task={selectedTask} />}
                {selectedSection === 'Attachments' && (
                  <>
                    <AttachmentSection attachedFiles={attachedFiles} handleFileInputChange={handleFileInputChange} project={project}/>
                    {attachedFiles.map((file, index) => (
                      <Typography key={index} variant="body2" className="text-surface-light px-2 text-md">
                        {file.name}
                      </Typography>
                    ))}
                  </>
                )}
                {selectedSection === 'ProgressTracking' && (
                  <div>
                    <TimerSection task={selectedTask} token={token} project={project}/>
                    <GithubSection token={token} task={selectedTask} project={project} />
                  </div>
                )}
                <div className="flex items-center px-2 mt-6">
                  <AccessTimeIcon className="text-secondary mr-2" fontSize='extraSmall' />
                  <Typography className='text-surface-light text-sm ml-1'>{formatDeadline(selectedTask.deadline)}</Typography>
                </div>
                
              </div>

            </>
          )}
        </div>
      </Fade>
    </Modal>
  );
};

export default TaskDetailsModal;

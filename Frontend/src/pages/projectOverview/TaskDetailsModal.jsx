import React, { useState, useEffect } from 'react';
import { Modal, Fade } from '@mui/material';
import AccessTimeIcon from '@mui/icons-material/AccessTime';
import { Typography } from '@material-tailwind/react';
import CommentSection from './CommentSection';
import AttachmentSection from './AttachmetSection';
import TimerSection from './TimerSection';
import GithubSection from './GithubSection';
import AssignUserTask from './AssignUserTask';
import TaskMainInfo from './TaskMainInfo';


const formatDeadline = (deadline) => {
  const options = { day: 'numeric', month: 'long', year: 'numeric' };
  const formattedDeadline = new Date(deadline).toLocaleDateString(undefined, options);
  return formattedDeadline;
};

const TaskDetailsModal =  ({ modalOpen, handleCloseModal, selectedTask, token, project })  => {
  const [attachedFiles, setAttachedFiles] = useState([]);
  const [currentTask, setCurrentTask] = useState([]);


  const handleFileInputChange = (event) => {
    const files = event.target.files;
    setAttachedFiles([...attachedFiles, ...files]);
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
        <div className="absolute top-1/2 left-1/2 transform -translate-x-1/2 -translate-y-1/2 w-[60rem] bg-[#2f2b3a] shadow-lg p-8 rounded outline-none">
          {selectedTask && (
            <div className='flex flex-row'>
              <div className='w-2/3'>
               <TaskMainInfo selectedTask={selectedTask} token={token}/>
               
                <AttachmentSection attachedFiles={attachedFiles} handleFileInputChange={handleFileInputChange} project={project}/>

                {attachedFiles.map((file, index) => (
                  <Typography key={index} variant="body2" className="text-surface-light px-2 text-md">
                    {file.name}
                  </Typography>
                ))}
                <CommentSection token={token} task={selectedTask} />
                <div className="flex items-center  px-2 mt-6">
                  <AccessTimeIcon className="text-secondary" fontSize='extraSmall' />
                  <p className='text-surface-light text-sm ml-1'> {formatDeadline(selectedTask.deadline)}</p>
                </div>

              </div>
              <div className="border-r border-1 border-surface-mid h-auto"></div>
              <div className="w-1/3 ml-4 p-4">
                <TimerSection task={selectedTask} token={token} project={project}/>
                <GithubSection token={token} task={selectedTask} project={project} />
                <AssignUserTask token={token} task={selectedTask} project={project} />
              </div>
            </div>
          )}
        </div>
      </Fade>
    </Modal>
  );
};

export default TaskDetailsModal;
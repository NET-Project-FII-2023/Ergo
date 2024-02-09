import React, { useState, useEffect } from 'react';
import { Modal, Fade } from '@mui/material';
import AccessTimeIcon from '@mui/icons-material/AccessTime'; 
import DescriptionIcon from '@mui/icons-material/Description';
import { Typography } from '@material-tailwind/react';
import api from '@/services/api';
import CommentSection from './CommentSection';
import AttachmentSection from './AttachmetSection';
import TimerSection from './TimerSection';
import GithubSection from './GithubSection';
import AssignUserTask from './AssignUserTask';


const formatDeadline = (deadline) => {
  const options = { day: 'numeric', month: 'long', year: 'numeric' };
  const formattedDeadline = new Date(deadline).toLocaleDateString(undefined, options);
  return formattedDeadline;
};

const TaskDetailsModal = ({ modalOpen, handleCloseModal, selectedTask, token, project }) => {
  const [attachedFiles, setAttachedFiles] = useState([]);
  const [photoUrl, setPhotoUrl] = useState(null);
  const [branches, setBranches] = useState([]);


  const handleFileInputChange = (event) => {
    const files = event.target.files;
    setAttachedFiles([...attachedFiles, ...files]);
  };
    
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
                <Typography variant='h4' className='text-white p-2'>
                  {selectedTask.taskName}
                </Typography>
                <div className='flex flex-row mt-4 pr-2 items-center'>
                  <DescriptionIcon className='text-secondary ml-1' fontSize='extraSmall'></DescriptionIcon>
                  <p className='text-gray-300 ml-1 text-md font-semibold'>
                    Description
                  </p>
                </div>
                
                <p variant="body2" className='text-surface-light pl-2 py-2 pr-12  text-md' component="p">
                  {selectedTask.description}
                </p>
                <AttachmentSection attachedFiles={attachedFiles} handleFileInputChange={handleFileInputChange} />

                {attachedFiles.map((file, index) => (
                  <Typography key={index} variant="body2" className="text-surface-light px-2 text-md">
                    {file.name}
                  </Typography>
                ))}
                <CommentSection token={token} task={selectedTask}/>
                <div className="flex items-center  px-2 mt-6">
                  <AccessTimeIcon className="text-secondary" fontSize='extraSmall'/>
                  <p className='text-surface-light text-sm ml-1'> {formatDeadline(selectedTask.deadline)}</p>
                </div>
                
              </div>
              <div className="border-r border-1 border-surface-mid h-auto"></div>
              <div className="w-1/3 ml-4 p-4">
                <TimerSection task={selectedTask} token={token}/>
                <GithubSection token={token} task={selectedTask} project={project}/>
                <AssignUserTask token={token} task={selectedTask}/>
              </div>
            </div>
          )}
        </div>
      </Fade>
    </Modal>
  );
};

export default TaskDetailsModal;

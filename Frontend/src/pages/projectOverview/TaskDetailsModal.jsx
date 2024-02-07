import React, { useState, useEffect } from 'react';
import { Modal, Fade, Card, CardContent } from '@mui/material';
import AccessTimeIcon from '@mui/icons-material/AccessTime'; 
import AttachFileIcon from '@mui/icons-material/AttachFile';
import DescriptionIcon from '@mui/icons-material/Description';
import ModeCommentIcon from '@mui/icons-material/ModeComment';
import UploadFileIcon from '@mui/icons-material/UploadFile';
import { Button, Typography } from '@material-tailwind/react';
import api from '@/services/api';



const formatDeadline = (deadline) => {
  const options = { day: 'numeric', month: 'long', year: 'numeric' };
  const formattedDeadline = new Date(deadline).toLocaleDateString(undefined, options);
  return formattedDeadline;
};

const TaskDetailsModal = ({ modalOpen, handleCloseModal, selectedTask, token }) => {
  const [attachedFiles, setAttachedFiles] = useState([]);
  const [photoUrl, setPhotoUrl] = useState(null);

  const handleFileInputChange = (event) => {
    const files = event.target.files;
    setAttachedFiles([...attachedFiles, ...files]);
  };


  useEffect(() => {
    if (selectedTask) {
      const fetchData = async () => {
        try {
          const response = await api.get(`https://localhost:7248/api/v1/Photos/${selectedTask.taskItemId}`, {
            headers: {
              'Authorization': `Bearer ${token}`,
            },
          });
          if (response.status === 200 && response.data.photos && response.data.photos.length > 0) {
            setPhotoUrl(response.data.photos[0].cloudURL);
          } else {
            console.error('Error fetching photo:', response);
          }
        } catch (error) {
          console.error('Error fetching photo:', error);
        }
      };
      fetchData();
    }
  }, [selectedTask]);

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
                  {/* {selectedTask.description} */}
                  Lorem ipsum, dolor sit amet consectetur adipisicing elit. Dolores tempore voluptatum sint? Incidunt unde doloribus distinctio asperiores esse hic laudantium reprehenderit accusantium nisi ex! Quisquam, modi nulla. Beatae, perferendis nulla.
                </p>
                <div className='flex flex-col mt-4 pr-2'>
                  <div className='flex flex-row items-center'> 
                    <AttachFileIcon className='text-secondary ml-1' fontSize='extraSmall'> </AttachFileIcon>
                    <p className='text-gray-300  text-md font-semibold'>
                      Attachments
                    </p>
                  </div>
                  <div>
                      <Button className='flex flex-row px-3 bg-surface-darkest py-2 ml-4 hover:opacity-70' size="sm">
                        <label htmlFor="file-upload" className="cursor-pointer text-gray-300 flex items-center">
                          <p className='text-md text-surface-light'>Upload</p>
                          <UploadFileIcon fontSize='small' className='ml-1'></UploadFileIcon>
                        </label>
                      </Button>
                    <input
                      id="file-upload"
                      type="file"
                      className="hidden"
                      onChange={handleFileInputChange}
                      multiple
                    />
                  </div>
                </div>
                {attachedFiles.map((file, index) => (
                  <Typography key={index} variant="body2" className="text-surface-light px-2 text-md">
                    {file.name}
                  </Typography>
                ))}
                <div className='flex flex-col mt-4 pr-2'>
                  <div className='flex items-center mb-2'>
                    <ModeCommentIcon className='text-secondary ml-1' fontSize='extraSmall'></ModeCommentIcon>
                    <p className='text-gray-300 ml-1 text-md font-semibold'>
                      Comments
                    </p>
                  </div>
                  
                  <Card className="opacity-80 cursor-pointer mb-2 rounded w-4/5">
                    <CardContent className='p-2 rounded bg-surface-darkest'>
                        <div className='flex'>
                            <span className='w-1/4 h-[2.3rem] rounded-full bg-surface-light mr-3'></span>
                            <div>
                                <p className="text-surface-light text-xs">
                                    Michael Jackson
                                </p>
                                <p className="text-surface-mid-light text-xs">
                                    Lorem, ipsum dolor sit amet consectetur adipisicing elit. Ullam fugit minus deleniti, nisi, consequatur itaque illo possimus ducimus, voluptas veniam animi provident? Quidem alias dolores cupiditate aliquam amet voluptates dignissimos?
                                </p>
                            </div>
                        </div>
                    </CardContent>
                  </Card>
                  <Card className="opacity-80 cursor-pointer mb-2 rounded w-4/5">
                    <CardContent className='p-2 rounded bg-surface-darkest'>
                        <div className='flex'>
                            <span className='w-1/4 h-[2.3rem] rounded-full bg-surface-light mr-3'>
                                
                            </span>
                            <div>
                                <p className="text-surface-light text-xs">
                                    Baracobama
                                </p>
                                <p className="text-surface-mid-light text-xs">
                                    Lorem, ipsum dolor sit amet consectetur adipisicing elit. Ullam fugit minus deleniti, nisi, consequatur itaque illo possimus ducimus, voluptas veniam animi provident? Quidem alias dolores cupiditate aliquam amet voluptates dignissimos?
                                </p>
                            </div>
                        </div>
                    </CardContent>
                  </Card>
                </div>
                <div className="flex items-center  px-2 mt-6">
                  <AccessTimeIcon className="text-secondary" fontSize='extraSmall'/>
                  <p className='text-surface-light text-sm ml-1'> {formatDeadline(selectedTask.deadline)}</p>
                </div>
                
              </div>
              <div className="border-r border-1 border-surface-mid h-auto"></div>
              {/* right section  */}
              <div className="w-1/3 ml-4">
                  
              </div>
            </div>
          )}
        </div>
      </Fade>
    </Modal>
  );
};

export default TaskDetailsModal;

import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import api from "@/services/api";
import { useUser } from "@/context/LoginRequired";
import { Typography, Modal, Fade } from '@mui/material';
import TaskSection from './TaskSection';

const ProjectOverview = () => {
  const { projectId } = useParams();
  const [currentProject, setCurrentProject] = useState([]);
  const [selectedTask, setSelectedTask] = useState(null);
  const [modalOpen, setModalOpen] = useState(false);
  const { token, userId } = useUser();

  useEffect(() => {
    fetchCurrentProject();
  }, [projectId, token, userId]);

  const fetchCurrentProject = async () => {
    try {
      if (!token || !userId) return;

      const response = await api.get(`/api/v1/Projects/${projectId}`, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      });

      if (response.status === 200) {
        setCurrentProject(response.data);
      } else {
        console.error('Error fetching tasks:', response);
      }
    } catch (error) {
      console.error('Error fetching tasks:', error);
    }
  };

  const handleOpenModal = (task) => {
    setSelectedTask(task);
    setModalOpen(true);
  };

  const handleCloseModal = () => {
    setModalOpen(false);
  };

  return (
    <div>
      <div>
        <Typography variant="h3" className='text-white'>
          {currentProject.projectName}
        </Typography>
        <div className='flex flex-row'>
          <Typography component="p" mr={1} className='text-surface-light'>
            Description:
          </Typography>
          <Typography variant="body1" component="p" className='text-surface-light'>
            {currentProject.description}
          </Typography>
        </div>
      </div>

      <TaskSection
        projectId={projectId}
        token={token}
        userId={userId}
        handleOpenModal={handleOpenModal}
      />
      
      <Modal
        open={modalOpen}
        onClose={handleCloseModal}
        aria-labelledby="modal-title"
        aria-describedby="modal-description"
        closeAfterTransition
      >
        <Fade in={modalOpen}>
          <div className="absolute top-1/2 left-1/2 transform -translate-x-1/2 -translate-y-1/2 w-1/2 max-h-80 overflow-y-auto bg-white rounded-md shadow-md p-8">
            <h2 id="modal-title">Task Details</h2>
            {selectedTask && (
              <>
                <Typography variant="h6" gutterBottom>
                  {selectedTask.taskName}
                </Typography>
                <Typography variant="body2" color="textSecondary" component="p">
                  {selectedTask.description}
                </Typography>
                <Typography variant="body2" color="textSecondary" component="p">
                  Deadline: {selectedTask.deadline}
                </Typography>
              </>
            )}
          </div>
        </Fade>
      </Modal>
    </div>
  );
};

export default ProjectOverview;

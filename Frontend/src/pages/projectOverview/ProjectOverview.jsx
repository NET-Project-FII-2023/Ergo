import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import api from "@/services/api";
import { useUser } from "@/context/LoginRequired";
import { Typography } from '@mui/material';
import TaskSection from './TaskSection';
import TaskDetailsModal from './TaskDetailsModal';
import ProjectSettings from './ProjectSettings';




const ProjectOverview = () => {
  const { projectId } = useParams();
  const [currentProject, setCurrentProject] = useState([]);
  const [selectedTask, setSelectedTask] = useState(null);
  const [modalOpen, setModalOpen] = useState(false);
  const { token, userId, username } = useUser();

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
      <div className='flex justify-between items-end'>
        <div >
          <div>
            <span className='text-surface-light text-sm opacity-75'>Owner:</span>
            <span className='text-surface-light text-sm opacity-50 ml-1'>@{currentProject.createdBy}</span>
          </div>
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
        {currentProject.createdBy === username && 
          <ProjectSettings project={currentProject} token={token}/>
        }
      </div>

      <div className='flex flex-row'>
        <TaskSection
          project={currentProject}
          token={token}
          userId={userId}
          handleOpenModal={handleOpenModal}
        />
      </div>
      <TaskDetailsModal
        modalOpen={modalOpen}
        handleCloseModal={handleCloseModal}
        selectedTask={selectedTask}
        token={token}
        project={currentProject}
      />
    </div>
  );
};

export default ProjectOverview;

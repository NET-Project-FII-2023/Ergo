import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import api from "@/services/api";
import { useUser } from "@/context/LoginRequired.jsx";
import { Typography, Modal, Fade } from '@mui/material';
import TaskCard from './TaskCard';

const ProjectOverview = () => {
  const { projectId } = useParams();
  const [taskItems, setTaskItems] = useState([]);
  const [currentProject, setCurrentProject] = useState([]);
  const [selectedTask, setSelectedTask] = useState(null);
  const [modalOpen, setModalOpen] = useState(false);
  const { token, userId } = useUser();
  const todoTasks = taskItems.filter(taskItem => taskItem.state === 1);
  const inProgressTasks = taskItems.filter(taskItem => taskItem.state === 2);
  const doneTasks = taskItems.filter(taskItem => taskItem.state === 3);

  useEffect(() => {
    fetchTaskItems();
    fetchCurrentProject();
  }, [projectId, token, userId]);

  const fetchTaskItems = async () => {
    try {
      if (!token || !userId) return;

      const response = await api.get(`/api/v1/TaskItems/ByProject/${projectId}`, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      });

      if (response.status === 200) {
        setTaskItems(response.data.taskItems);
        console.log("Tasks:");
        console.log(response.data.taskItems);
      } else {
        console.error('Error fetching tasks:', response);
      }
    } catch (error) {
      console.error('Error fetching tasks:', error);
    }
  };

  const fetchCurrentProject = async () => {
    try {
      if (!token || !userId) return;

      const response = await api.get(`/api/v1/Projects/${projectId}`, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      });

      if (response.status === 200) {
        setCurrentProject(response.data)
      } else {
        console.error('Error fetching tasks:', response);
      }
    } catch (error) {
      console.error('Error fetching tasks:', error);
    }
  };

  const renderTaskCards = (tasks) => {
    return tasks.map((taskItem) => (
      <TaskCard
      key={taskItem.taskItemId}
      task={taskItem}
      handleOpenModal={() => handleOpenModal(taskItem)}
    />
    ));
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
      
      <div className="border-b-2 border-surface-dark my-4"></div>
  
      <div>
        <div className="flex justify-between">
          <div className="flex-1 mr-8">
          <div className="border-b-4 border-secondary"></div>
            <div className='px-4 py-4 bg-surface-dark mb-4 flex items-center'>
              <h4 className="text-surface-light">TO DO</h4>
            </div>
            {renderTaskCards(todoTasks)}
          </div>
          <div className="flex-1 mr-8 text-surface-light">
          <div className="border-b-4 border-blue-400"></div>
            <div className='px-4 py-4 bg-surface-dark mb-4 flex items-center'>
              <h4 className="text-surface-light">IN PROGRESS</h4>
            </div>
            {renderTaskCards(inProgressTasks)}
          </div>
          <div className="flex-1 text-surface-light">
          <div className="border-b-4 border-teal-300"></div>
            <div className='px-4 py-4 bg-surface-dark mb-4 flex items-center'>
              <h4 className="text-surface-light">DONE</h4>
            </div>
            {renderTaskCards(doneTasks)}
          </div>
        </div>
      </div>
  
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

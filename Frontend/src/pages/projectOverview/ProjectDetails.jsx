import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import api from "@/services/api";
import { useUser } from "@/context/LoginRequired.jsx";
import { Card, CardContent, Typography, Modal, Backdrop, Fade } from '@mui/material';

const ProjectDetails = () => {
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
        console.log("Project:");
        console.log(response.data);
      } else {
        console.error('Error fetching tasks:', response);
      }
    } catch (error) {
      console.error('Error fetching tasks:', error);
    }
  };

  const renderTaskCards = (tasks) => {
    return tasks.map((taskItem) => (
      <Card
        key={taskItem.taskItemId}
        className={`mb-4 opacity-80 cursor-pointer`}
        style={{
          backgroundColor: getCardBackgroundColor(taskItem.state),
        }}
        onClick={() => handleOpenModal(taskItem)}
      >
        <CardContent>
          <Typography variant="h6" gutterBottom>
            {taskItem.taskName}
          </Typography>
          <Typography variant="body2" color="textSecondary" component="p">
            {taskItem.description}
          </Typography>
          <div className='flex flex-row'>
            <Typography variant="body2" color="textPrimary" component="p" mr={1}>
              Deadline:
            </Typography>
            <Typography variant="body2" color="textSecondary" component="p">
             {formatDeadline(taskItem.deadline)}
            </Typography>
          </div>
          
        </CardContent>
      </Card>
    ));
  };
  
  const formatDeadline = (deadline) => {
    const options = { day: 'numeric', month: 'long', year: 'numeric' };
    const formattedDeadline = new Date(deadline).toLocaleDateString(undefined, options);
    return formattedDeadline;
  };
  
  const getCardBackgroundColor = (state) => {
    switch (state) {
      case 1:
        return 'rgba(232, 207, 132, 0.7)';
      case 2:
        return 'rgba(111, 206, 237, 0.7)';
      case 3:
        return 'rgba(144, 238, 144, 0.7)';
      default:
        return 'white';
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
        <Typography variant="h3">
          {currentProject.projectName}
        </Typography>
        <div className='flex flex-row'>
          <Typography variant="body1" color="textPrimary" component="p" mr={1}>
            Description:   
          </Typography>
          <Typography variant="body1" color="textSecondary" component="p">
            {currentProject.description}
          </Typography>
        </div>
      </div>
      
      <div className="border-b-2 border-gray-300 my-4"></div>
  
      <div>
        <div className="flex justify-between">
          <div className="flex-1 mr-8">
            <h4 className="mb-2">To Do</h4>
            {renderTaskCards(todoTasks)}
          </div>
          <div className="flex-1 mr-8">
            <h4 className="mb-2">In Progress</h4>
            {renderTaskCards(inProgressTasks)}
          </div>
          <div className="flex-1">
            <h4 className="mb-2">Done</h4>
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

export default ProjectDetails;

import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import api from "@/services/api";
import { useUser } from "@/context/LoginRequired.jsx";
import { Card, CardContent, Typography, Modal, Backdrop, Fade } from '@mui/material';

const ProjectDetails = () => {
  const { projectId } = useParams();
  const [taskItems, setTaskItems] = useState([]);
  const [selectedTask, setSelectedTask] = useState(null);
  const [modalOpen, setModalOpen] = useState(false);
  const { token, userId } = useUser();

  useEffect(() => {
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

    fetchTaskItems();
  }, [projectId, token, userId]);

  const todoTasks = taskItems.filter(taskItem => taskItem.state === 1);
  const inProgressTasks = taskItems.filter(taskItem => taskItem.state === 2);
  const doneTasks = taskItems.filter(taskItem => taskItem.state === 3);

  const renderTaskCards = (tasks) => {
    return tasks.map(taskItem => (
      <Card
        key={taskItem.taskItemId}
        style={{
          marginBottom: '10px',
          backgroundColor: getCardBackgroundColor(taskItem.state),
          opacity: 0.8,
          cursor: 'pointer', // Add cursor pointer for interaction
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
          <Typography variant="body2" color="textSecondary" component="p">
            Deadline: {taskItem.deadline}
          </Typography>
        </CardContent>
      </Card>
    ));
  };

  const getCardBackgroundColor = (state) => {
    switch (state) {
      case 1:
        return 'rgba(181, 106, 235, 0.5)';
      case 2:
        return 'rgba(111, 206, 237, 0.5)';
      case 3:
        return 'rgba(144, 238, 144, 0.5)';
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
        <div style={{ display: 'flex', justifyContent: 'space-between' }}>
          <div style={{ flex: 1, marginRight: '20px' }}>
            <h4>To Do</h4>
            {renderTaskCards(todoTasks)}
          </div>
          <div style={{ flex: 1, marginRight: '20px' }}>
            <h4>In Progress</h4>
            {renderTaskCards(inProgressTasks)}
          </div>
          <div style={{ flex: 1 }}>
            <h4>Done</h4>
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
        BackdropProps={{
          style: {
            outline: 'none',
          },
        }}
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
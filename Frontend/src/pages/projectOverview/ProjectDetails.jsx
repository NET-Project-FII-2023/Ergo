import React, { useEffect, useState } from 'react';
import { useParams } from 'react-router-dom';
import api from "@/services/api";
import { useUser } from "@/context/LoginRequired.jsx";
import { Card, CardContent, Typography } from '@mui/material';


const ProjectDetails = () => {
  const { projectId } = useParams();
  const [taskItems, setTaskItems] = useState([]);
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
          opacity: 0.8, // Adjust the transparency as needed
        }}
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
    </div>
  );
};

export default ProjectDetails;

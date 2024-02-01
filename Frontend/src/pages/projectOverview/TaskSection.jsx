import React, { useEffect, useState } from 'react';
import api from "@/services/api";
import TaskCard from './TaskCard';

const TaskSection = ({ projectId, token, userId, handleOpenModal}) => {
    const [taskItems, setTaskItems] = useState([]);
    const todoTasks = taskItems.filter(taskItem => taskItem.state === 1);
    const inProgressTasks = taskItems.filter(taskItem => taskItem.state === 2);
    const doneTasks = taskItems.filter(taskItem => taskItem.state === 3);
  
    useEffect(() => {
      fetchTaskItems();
    }, [projectId, token, userId]);

  useEffect(() => {
    fetchTaskItems();
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

  const renderTaskCards = (tasks) => {
    return tasks.map((taskItem) => (
        <TaskCard
        key={taskItem.taskItemId}
        task={taskItem}
        handleOpenModal={() => handleOpenModal(taskItem)}
      />
    ));
  };


  return (
    <div>
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
    </div>
  );
};

export default TaskSection;

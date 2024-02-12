import React, { useEffect, useState } from 'react';
import api from "@/services/api";
import TaskCard from './TaskCard';
import AddTask from './AddTask';
import MembersList from './MembersList';
import { useUser } from '../../context/LoginRequired';

const TaskSection = ({ project, token, userId, handleOpenModal, handleCloseModal }) => {
  const [taskItems, setTaskItems] = useState([]);
  const todoTasks = taskItems.filter(taskItem => taskItem.state === 1);
  const inProgressTasks = taskItems.filter(taskItem => taskItem.state === 2);
  const doneTasks = taskItems.filter(taskItem => taskItem.state === 3);
  const currentUser = useUser();

  useEffect(() => {
    fetchTaskItems();
  }, [project.projectId, token, userId, handleCloseModal]);

  const fetchTaskItems = async () => {
    try {
      if (!token || !userId) return;

      const response = await api.get(`/api/v1/TaskItems/ByProject/${project.projectId}`, {
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

  const renderTaskCards = (tasks, column) => {
    let color;
    switch (column) {
      case 'todo':
        color = "#5e4d8c";
        break;
      case 'inProgress':
        color = "#3f6da6";
        break;
      case 'done':
        color = "#42a696";
        break;
      default:
        color = "#2dd4bf";
    }

    return tasks.map((taskItem) => (
      <TaskCard
        key={taskItem.taskItemId}
        task={taskItem}
        handleOpenModal={() => handleOpenModal(taskItem)}
        color={color}
      />
    ));
  };


  return (
    <div className='md:w-[75vw] w-full'>
      <div className="border-b-2 border-surface-dark my-4"></div>
      <div>
        <div className="flex md:flex-row flex-col md:justify-evenly justify-center">
          <div className="md:w-[25%] md:mr-4">
            <div className="border-b-4 border-secondary"></div>
            <div className='p-3 bg-surface-dark mb-4 flex items-center rounded-b'>
              <h4 className="text-surface-light">To do</h4>
            </div>
            {currentUser.username === project.createdBy &&
              <AddTask
                projectId={project.projectId}
                token={token}
                userId={userId}
                onTaskAdded={fetchTaskItems}
              />
            }
            <div className='md:overflow-none md:max-h-none overflow-auto max-h-[50vh]'>
              {renderTaskCards(todoTasks, 'todo')}
            </div>
          </div>
          <div className="md:w-[25%] md:mr-4">
            <div className="border-b-4 border-blue-400"></div>
            <div className='p-3 bg-surface-dark mb-4 flex items-center rounded-b'>
              <h4 className="text-surface-light">In progress</h4>
            </div>
            <div className='md:overflow-none md:max-h-none overflow-auto max-h-[50vh]'>
              {renderTaskCards(inProgressTasks, 'inProgress')}
            </div>
          </div>
          <div className="md:w-[25%] text-surface-light">
            <div className="border-b-4 border-teal-300"></div>
            <div className='p-3 bg-surface-dark mb-4 flex items-center rounded-b'>
              <h4 className="text-surface-light">Done</h4>
            </div>
            <div className='md:overflow-none md:max-h-none overflow-auto max-h-[50vh]'>
              {renderTaskCards(doneTasks, 'done')}
            </div>
          </div>
          <div className="md:w-[25%] text-surface-light bg-surface-darkest md:px-4 md:ml-3">

            <MembersList
              project={project}
              token={token}
            />
          </div>
        </div>
      </div>
    </div>
  );
};

export default TaskSection;
import React, { useEffect, useState,useRef } from 'react';
import api from "@/services/api";
import TaskCard from './TaskCard';
import AddTask from './AddTask';
import MembersList from './MembersList';
import { useUser } from '../../context/LoginRequired';
import { DragDropContext,Droppable,Draggable } from "@hello-pangea/dnd";
import { toast } from "react-toastify";


const TaskSection = ({ project, setProject, token, userId, handleOpenModal, handleCloseModal,modalOpen }) => {
  const [taskItems, setTaskItems] = useState([]);
  const todoTasks = taskItems.filter(taskItem => taskItem.state === 1);
  const inProgressTasks = taskItems.filter(taskItem => taskItem.state === 2);
  const doneTasks = taskItems.filter(taskItem => taskItem.state === 3);
  const currentUser = useUser();


  useEffect(() => {
    fetchTaskItems();
  }, [project.projectId, modalOpen]);

  const fetchTaskItems = async () => {
    if (!token || !userId || !project.projectId) return;

    try {
      
      const response = await api.get(`/api/v1/TaskItems/ByProject/${project.projectId}`, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      });
      
      if (response.status === 200) {
        setTaskItems(response.data.taskItems);
      } else {
        console.error('Error fetching tasks:', response);
      }
    } catch (error) {
      console.error('Error fetching tasks:', error);
    }

  };
  const handleSave = async (taskItemId,taskState) => {
    try {
      const response = await api.put(`/api/v1/TaskItems/${taskItemId}`, {
        taskItemId: taskItemId,
        state: taskState,
      }, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      });

      if (response.status === 200) {
        fetchTaskItems();
        toast.success('Task state updated successfully');
      } else {
        console.error('Error updating task state:', response);
        toast.error('Failed to update task state');
      }
    } catch (error) {
      console.error('Error updating task state:', error);
      toast.error('Failed to update task state');
    }
  };
  const getTaskStateFromDroppableId = (droppableId) => {
    const states = { 'todo': 1, 'inProgress': 2, 'done': 3 };
    return states[droppableId] || 1;
  };
  const onDragEnd = async (result) => {
    const { source, destination,draggableId  } = result;
    
    if(!destination)  return;
    const taskState = getTaskStateFromDroppableId(destination.droppableId);
    if (!destination) {
      return;
    }

    if (
      source.droppableId !== destination.droppableId ||
      source.index !== destination.index
    ) {
      
      const newTaskItems = [...taskItems];
      const taskIndex = newTaskItems.findIndex(task => task.taskItemId.toString() === draggableId);
      if (taskIndex > -1) {
        newTaskItems[taskIndex] = { ...newTaskItems[taskIndex], state: taskState };
        setTaskItems(newTaskItems);
        try {
          await handleSave(draggableId, taskState);
        } catch (error) {
          setTaskItems(taskItems);
          toast.error('Failed to update task state');
        }
      }
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

    return (
      <Droppable droppableId={column} type="list" direction="vertical">
        {(provided, snapshot) => (
          <div
            ref={provided.innerRef}
            {...provided.droppableProps}
            className={`min-h-[50px]`}
          >
            {tasks.map((taskItem, index) => (
              <Draggable key={taskItem.taskItemId} draggableId={taskItem.taskItemId.toString()} index={index}>
                {(provided, snapshot) => (
                  <div
                    ref={provided.innerRef}
                    {...provided.draggableProps}
                    {...provided.dragHandleProps}
                    className={`mb-2 ${snapshot.isDragging ? 'shadow-lg' : ''}`}
                  >
                    <TaskCard
                    task={taskItem}
                    handleOpenModal={() => handleOpenModal(taskItem)}
                    color={color}
                  />
                  </div>
                )}
              </Draggable>
            ))}
            {provided.placeholder}
          </div>
        )}
      </Droppable>
    );
  };


  return (
    <div className='w-full'>
      <div className="border-b-2 border-surface-dark my-4"></div>
      <div>
      <DragDropContext onDragEnd={onDragEnd}>

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
              setAssignedMembers={(members) => setProject({ ...project, members: members })}
              token={token}
            />
          </div>
        </div>
      </DragDropContext>
      </div>
    </div>
  );
};

export default TaskSection;
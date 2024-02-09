import React, { useEffect, useState } from 'react';
import api from "@/services/api";
import { DragDropContext, Droppable, Draggable } from 'react-beautiful-dnd';
import TaskCard from './TaskCard';
import AddTask from './AddTask';
import MembersList from './MembersList';

const TaskSection = ({ projectId, token, userId, handleOpenModal}) => {
    const [taskItems, setTaskItems] = useState([]);
    const todoTasks = taskItems.filter(taskItem => taskItem.state === 1);
    const inProgressTasks = taskItems.filter(taskItem => taskItem.state === 2);
    const doneTasks = taskItems.filter(taskItem => taskItem.state === 3);

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

    const handleTaskItemDrop = async (result) => {
      const { source, destination, draggableId } = result;
  
      if (!destination) {
          return;
      }
  
      if (source.droppableId === destination.droppableId && source.index === destination.index) {
          return;
      }
  
      const updatedTaskItems = Array.from(taskItems);
      const movedTaskItemIndex = updatedTaskItems.findIndex(taskItem => taskItem.taskItemId === draggableId);
      const movedTaskItem = updatedTaskItems[movedTaskItemIndex];
      const newColumnState = getColumnState(destination.droppableId);
      movedTaskItem.state = newColumnState;
      setTaskItems(updatedTaskItems);
  
      try {
          console.log('Request body:', movedTaskItem); // Log the request body
          const response = await api.put(`/api/v1/TaskItems/${movedTaskItem.taskItemId}`, {
              ...movedTaskItem
          }, {
              headers: {
                  Authorization: `Bearer ${token}`,
              },
          });
  
          if (response.status !== 200) {
              console.error('Error updating task item:', response);
          }
      } catch (error) {
          console.error('Error updating task item:', error);
      }
  };
  
  const getColumnState = (droppableId) => {
    switch (droppableId) {
        case 'todo': // String value matching with "1"
            return 1;
        case 'inProgress': // String value matching with "2"
            return 2;
        case 'done': // String value matching with "3"
            return 3;
        default:
            return null;
    }
};
  

    return (
        <DragDropContext onDragEnd={handleTaskItemDrop}>
            <div className='w-[75vw]'>
                <div className="border-b-2 border-surface-dark my-4"></div>
                <div>
                    <div className="flex justify-evenly">
                        <Droppable droppableId='todo'>
                            {(provided) => (
                                <div ref={provided.innerRef} {...provided.droppableProps} className="w-[25%] mr-4">
                                    <div className="border-b-4 border-secondary"></div>
                                    <div className='p-3 bg-surface-dark mb-4 flex items-center rounded-b'>
                                        <h4 className="text-surface-light">To do</h4>
                                    </div>
                                    <AddTask
                                        projectId={projectId}
                                        token={token}
                                        userId={userId}
                                        onTaskAdded={fetchTaskItems}
                                    />
                                    {todoTasks.map((taskItem, index) => (
                                        <Draggable key={taskItem.taskItemId} draggableId={taskItem.taskItemId} index={index}>
                                            {(provided) => (
                                                <div
                                                    ref={provided.innerRef}
                                                    {...provided.draggableProps}
                                                    {...provided.dragHandleProps}
                                                >
                                                    <TaskCard
                                                        task={taskItem}
                                                        handleOpenModal={() => handleOpenModal(taskItem)}
                                                        color="#5e4d8c"
                                                    />
                                                </div>
                                            )}
                                        </Draggable>
                                    ))}
                                    {provided.placeholder}
                                </div>
                            )}
                        </Droppable>
                        <Droppable droppableId='inProgress'>
                            {(provided) => (
                                <div ref={provided.innerRef} {...provided.droppableProps} className="w-[25%] mr-4">
                                    <div className="border-b-4 border-blue-400"></div>
                                    <div className='p-3 bg-surface-dark mb-4 flex items-center rounded-b'>
                                        <h4 className="text-surface-light">In progress</h4>
                                    </div>
                                    {inProgressTasks.map((taskItem, index) => (
                                        <Draggable key={taskItem.taskItemId} draggableId={taskItem.taskItemId} index={index}>
                                            {(provided) => (
                                                <div
                                                    ref={provided.innerRef}
                                                    {...provided.draggableProps}
                                                    {...provided.dragHandleProps}
                                                >
                                                    <TaskCard
                                                        task={taskItem}
                                                        handleOpenModal={() => handleOpenModal(taskItem)}
                                                        color="#3f6da6"
                                                    />
                                                </div>
                                            )}
                                        </Draggable>
                                    ))}
                                    {provided.placeholder}
                                </div>
                            )}
                        </Droppable>
                        <Droppable droppableId='done'>
                            {(provided) => (
                                <div ref={provided.innerRef} {...provided.droppableProps} className="w-[25%] text-surface-light">
                                    <div className="border-b-4 border-teal-300"></div>
                                    <div className='p-3 bg-surface-dark mb-4 flex items-center rounded-b'>
                                        <h4 className="text-surface-light">Done</h4>
                                    </div>
                                    {doneTasks.map((taskItem, index) => (
                                        <Draggable key={taskItem.taskItemId} draggableId={taskItem.taskItemId} index={index}>
                                            {(provided) => (
                                                <div
                                                    ref={provided.innerRef}
                                                    {...provided.draggableProps}
                                                    {...provided.dragHandleProps}
                                                >
                                                    <TaskCard
                                                        task={taskItem}
                                                        handleOpenModal={() => handleOpenModal(taskItem)}
                                                        color="#42a696"
                                                    />
                                                </div>
                                            )}
                                        </Draggable>
                                    ))}
                                    {provided.placeholder}
                                </div>
                            )}
                        </Droppable>
                        <div className="w-[25%] text-surface-light bg-surface-darkest px-4 ml-3">
                            <MembersList
                                projectId={projectId}
                                token={token}
                            />
                        </div>
                    </div>
                </div>
            </div>
        </DragDropContext>
    );
};

export default TaskSection;
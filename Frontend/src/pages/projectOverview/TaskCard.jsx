import React from 'react';
import { Card, CardContent, Typography } from '@mui/material';
import { useDrag } from 'react-dnd';

const formatDeadline = (deadline) => {
    const options = { day: 'numeric', month: 'long', year: 'numeric' };
    const formattedDeadline = new Date(deadline).toLocaleDateString(undefined, options);
    return formattedDeadline;
  };


  const TaskCard = ({ task, handleOpenModal, color }) => (
    <Card
    key={task.taskItemId}
    className={`mb-4 opacity-80 cursor-pointer hover:opacity-60`}
    style={{
      backgroundColor: color,
    }}
    onClick={() => handleOpenModal(task)}
  >
      <CardContent className='transition duration-200  ease-in-out'>
        <p  className='text-white mb-2 text-lg'>
          {task.taskName}
        </p>
        <p className='text-gray-200 my-1 text-sm' component="p">
          {task.description}
        </p>
        <div className='flex flex-row'>
          <p className='text-gray-200 mt-1 text-xs' component="p">
            {formatDeadline(task.deadline)}
          </p>
        </div>
      </CardContent>
    </Card>
  );
  
  export default TaskCard;
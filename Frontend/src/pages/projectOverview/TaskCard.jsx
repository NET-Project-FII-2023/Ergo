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
      <CardContent className='transition duration-200 ease-in-out '>
        <Typography gutterBottom className='text-white'>
          {task.taskName}
        </Typography>
        <Typography variant="body2" className='text-gray-200' component="p">
          {task.description}
        </Typography>
        <div className='flex flex-row'>
          <Typography variant="body2" className='text-gray-200' component="p" mr={1}>
            Deadline:
          </Typography>
          <Typography variant="body2" className='text-gray-200' component="p">
            {formatDeadline(task.deadline)}
          </Typography>
        </div>
      </CardContent>
    </Card>
  );
  
  export default TaskCard;
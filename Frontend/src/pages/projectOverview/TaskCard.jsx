import React from 'react';
import { Card, CardContent, Typography } from '@mui/material';

const formatDeadline = (deadline) => {
    const options = { day: 'numeric', month: 'long', year: 'numeric' };
    const formattedDeadline = new Date(deadline).toLocaleDateString(undefined, options);
    return formattedDeadline;
  };


const TaskCard = ({ task, handleOpenModal }) => (
    <Card
        key={task.taskItemId}
        className={`mb-4 opacity-80 cursor-pointer `}
        style={{
        backgroundColor: "#2f2b3a",
        }}
        onClick={handleOpenModal} 
    >
      <CardContent>
        <Typography variant="h6" gutterBottom className='text-white'>
          {task.taskName}
        </Typography>
        <Typography variant="body2" className='text-surface-light' component="p">
          {task.description}
        </Typography>
        <div className='flex flex-row'>
          <Typography variant="body2" className='text-surface-light' component="p" mr={1}>
            Deadline:
          </Typography>
          <Typography variant="body2" className='text-surface-light' component="p">
            {formatDeadline(task.deadline)}
          </Typography>
        </div>
      </CardContent>
    </Card>
  );

  export default TaskCard;
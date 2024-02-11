import React from 'react';
import { Card, CardContent } from '@mui/material';
import AccessTimeIcon from '@mui/icons-material/AccessTime';
import UserAvatar from "@/common/components/UserAvatar";
import {Tooltip} from "@material-tailwind/react";
import {useNavigate} from "react-router-dom";

const formatDeadline = (deadline) => {
  const options = { day: 'numeric', month: 'long', year: 'numeric' };
  const formattedDeadline = new Date(deadline).toLocaleDateString(undefined, options);
  return formattedDeadline;
};

const TaskCard = ({ task, handleOpenModal, color }) => {
  const navigate = useNavigate();
  const truncatedDescription = task.description.length > 100 ? `${task.description.slice(0, 150)}...` : task.description;

  const handleUserAvatarClick = (e, userId) => {
    e.stopPropagation();
    userId && navigate(`/dashboard/profile/${userId}`)
  }

  return (
    <Card
      key={task.taskItemId}
      className={`mb-4 opacity-80 cursor-pointer hover:opacity-60`}
      style={{
        backgroundColor: color,
      }}
      onClick={() => handleOpenModal(task)}
    >
      <CardContent className='transition duration-200 ease-in-out'>
        <p className='text-white mb-1 text-lg'>
          {task.taskName}
        </p>
        <p className='text-gray-200 my-1 text-xs' component="p">
          {truncatedDescription}
        </p>
        <div className='flex flex-row justify-between mt-4'>
          <div className='flex flex-row'>
              <p className='text-gray-200 text-xs font-medium' component="p">
                {formatDeadline(task.deadline)}
              </p>
              <AccessTimeIcon className="text-gray-100 ml-1" fontSize='extraSmall'/>
          </div>
          {task?.assignedUser && (
            <Tooltip content={task.assignedUser?.name}>
              <div
                onClick={(e) => handleUserAvatarClick(e, task.assignedUser?.userId)}
                className={"cursor-pointer mr-3"}
              >
                <UserAvatar
                  photoUrl={task.assignedUser?.userPhoto?.photoUrl}
                  className={"w-[2rem] h-[2rem] rounded-full"}
                  loadingClassName={"w-[2rem] h-[2rem] bg-surface-mid-dark rounded-full"}
                  loadingProps={{className: "w-5 h-5"}}
                />
              </div>
            </Tooltip>
          )}
        </div>
      </CardContent>
    </Card>
  );
};

export default TaskCard;

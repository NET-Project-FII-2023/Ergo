import { ChevronDoubleDownIcon } from "@heroicons/react/24/outline";
import {
  Card,
  CardHeader,
  CardBody,
  CardFooter,
  Typography,
} from "@material-tailwind/react";import React, { useState } from "react";
;

interface TasksStatsProps {
  color?: string;
  icon: React.ReactNode;
  title: string;
  count: React.ReactNode;
  footer: React.ReactNode;
}

export function TasksStats({ color, icon, title, count, footer } : TasksStatsProps) {
  return (
    <div className={`p-4 rounded-xl ${color}`}>
      <div className="flex items-center">
        <div className="flex items-center justify-center w-14 h-14 bg-white bg-opacity-5 rounded-lg shadow-lg">
          {icon}
        </div>
        <div className="ml-5">
          <Typography color="white" variant="h5">
            {title}
          </Typography>
          <Typography color="white" variant="h3">
            {count}
          </Typography>
        </div>
      </div>
      <hr className="my-2 border-black border-opacity-30" />
      <div>
        {footer}
      </div>
    </div>
  );
}

export default TasksStats;

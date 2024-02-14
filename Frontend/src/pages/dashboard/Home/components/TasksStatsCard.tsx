import {Typography} from "@material-tailwind/react";
import { TasksStatsProps } from "./types";

export function TasksStatsCard({ color, icon, title, count, footer } : TasksStatsProps) {
  return (
    <div className={`p-4 rounded-xl ${color} mb-4 xl:mb-0 w-full sm:w-3/5 sm:mx-auto md:mx-0 md:w-full col-span-3 md:col-span-1`}>
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

export default TasksStatsCard;

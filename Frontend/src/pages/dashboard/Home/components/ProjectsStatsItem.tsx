import { Typography, Tooltip, Avatar, Progress } from "@material-tailwind/react";
import { Link } from "react-router-dom";
import { ProjectsStatsItemProps } from "./types";

export function ProjectsStatsItem(props : ProjectsStatsItemProps) {
  const { name, members, deadline, totalTasksCount, completion, path, className } = props;
  return (
    <tr>
      <td className={className}>
        {/* <div className="flex items-center gap-4"> */}
          {/* <Avatar src={img} alt={name} size="sm" /> */}
        <Typography variant="small" className="font-bold text-white">
          <Link to={path} className="hover:text-primary duration-200">
            {name}
          </Link>
        </Typography>
        {/* </div> */}
      </td>
      <td className={className}>
        {members.map(({ img, name }, index) => (
          <Tooltip key={`${name}_${index}`} content={name}>
            <Avatar src={img} alt={name} size="xs" variant="circular" className={`cursor-pointer border-2 border-primary ${index === 0 ? "" : "-ml-2.5"}`}/>
          </Tooltip>
        ))}
      </td>
      <td className={className}>
        <Typography variant="small" className="text-xs font-medium text-surface-light" >
          {new Date(deadline).toLocaleString("en-US", { month: "long" })} {new Date(deadline).getDate()}, {new Date(deadline).getFullYear()}
        </Typography>
      </td>
      <td className={className}>
        <Typography variant="small" className="text-xs font-medium text-surface-light" >
          {totalTasksCount}
        </Typography>
      </td>
      <td className={className}>
        <div className="w-10/12">
          <Typography variant="small" className="mb-1 block text-xs font-medium text-surface-light" >
            {completion}%
          </Typography>
          <Progress value={completion} variant="gradient" className="h-1 bg-surface-mid [&>div]:from-primary [&>div]:to-secondary" />
        </div>
      </td>
    </tr> 
  );
}
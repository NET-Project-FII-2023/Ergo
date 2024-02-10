import React, { useEffect } from "react";
import {
  Typography,
  Card,
  CardHeader,
  CardBody,
  IconButton,
  Menu,
  MenuHandler,
  MenuList,
  MenuItem,
  Avatar,
  Tooltip,
  Progress,
} from "@material-tailwind/react";
import {
  EllipsisVerticalIcon,
} from "@heroicons/react/24/outline";
import { useUser } from "../../../context/LoginRequired";
import { TasksStats } from "../../../widgets/cards";
import { StatisticsChart } from "../../../widgets/charts";
import {
  statisticsChartsData,
  projectsTableData,
} from "../../../data";
import { CheckBadgeIcon, CogIcon, RectangleStackIcon } from "@heroicons/react/24/solid";
import { Link } from "react-router-dom";
import { TaskStats, TasksFromAllProjects } from "./types";
import api from "../../../services/api";

export function Home() {
  const user = useUser();
  const time = new Date().getHours();
  const [tasksFromAllProjects, setTasksFromAllProjects] = React.useState({} as TasksFromAllProjects);
  const [tasksStats, setTasksStats] = React.useState([] as TaskStats[]);
  const [projectsIds, setProjectsIds] = React.useState([] as string[]);

  useEffect(() => {
    (async () => {
      //get tasks from all projects
      try {
        const response = await api.get(`/api/v1/TaskItems/ByProjectsOfUser/${user.userId}`, {
          headers: {
            Authorization: `Bearer ${user.token}`,
          },
        });
        if (response.status !== 200) {
          throw new Error(response.data.message);
        }
        setTasksFromAllProjects(response.data.taskItems);
      } catch (error : any) { 
        console.log(`Error while getting notifications: ${error.response.data}`);
      }
    })();
  }, []);

  useEffect(() => {
    if(Object.keys(tasksFromAllProjects).length <= 0) return;
    const projects = Object.keys(tasksFromAllProjects);
    const stats = [{ count: 0, footerValue: `${projects.length}` }, { count: 0, footerValue: "0" }, { count: 0, footerValue: "0" }];
    projects.forEach((project) => {
      tasksFromAllProjects[project].forEach((task) => {
        stats[task.state - 1].count++;
      });
    });

    //get the number of completed projects
    stats[2].footerValue = projects.filter((project) => {
      return tasksFromAllProjects[project].every((task) => task.state === 3);
    }).length.toString();

    //get the next due date
    const nextDue = tasksFromAllProjects[projects[0]].reduce((acc, task) => {
      return acc.deadline < task.deadline ? acc : task;
    });
    stats[1].footerValue = `${new Date(nextDue.deadline).getDate()}th of ${new Date(nextDue.deadline).toLocaleString("en-US", { month: "long" })} ${new Date(nextDue.deadline).getFullYear()}`;

    setTasksStats(stats);
    setProjectsIds(projects);
  }, [tasksFromAllProjects]);
  
  return (
    <div className="mt-12 text-surface-light">
      <div className="mb-16">
        <h2 className="text-5xl font-bold">
          Good {(time < 4 || time >= 18) ? "evening" : time <= 12 ? "morning" : "afternoon"}, {user?.username}!
        </h2>
      </div>
      <div className="grid gap-y-10 gap-x-6 md:grid-cols-2 xl:grid-cols-3">
        {/* main container */}
        <div className="xl:col-span-2">
          {/* left container */}
          <div className="grid xl:grid-cols-3 gap-x-3">
            {tasksStats.length !== 0 && (
              <>
              <TasksStats
                color="bg-[#5e4d8c]"
                count={tasksStats[0].count}
                title="Tasks To Do"
                icon={React.createElement(RectangleStackIcon, {
                  className: "w-8 h-8 text-white",
                })}
                footer={
                  <Typography className="font-normal text-white">
                    Across {projectsIds.length > 1 ? "all your" : ""} <strong>{tasksStats[0].footerValue}</strong> project{projectsIds.length != 1 ? "s" : ""}
                  </Typography>
                }
              />
              <TasksStats
                color="bg-[#3f6da6]"
                count={tasksStats[1].count}
                title="Tasks In Progress"
                icon={React.createElement(CogIcon, {
                  className: "w-8 h-8 text-white",
                })}
                footer={
                  <Typography className="font-normal text-white">
                    Next due:&nbsp;
                    <strong>{tasksStats[1].footerValue}</strong>
                  </Typography>
                }
              />
              <TasksStats
                color="bg-[#42a696]"
                count={tasksStats[2].count}
                title="Tasks Completed"
                icon={React.createElement(CheckBadgeIcon, {
                  className: "w-8 h-8 text-white",
                })}
                footer={
                  <Typography className="font-normal text-white">
                    <strong>{tasksStats[2].footerValue}</strong> completed projects
                  </Typography>
                }
              />
              </>
              )
            } 

            <Card className="overflow-hidden mt-12 xl:col-span-3 bg-surface-dark shadow-sm">
              <CardHeader floated={false} shadow={false} color="transparent" className="m-0 flex items-center justify-between p-6">
                <Typography variant="h5" className="mb-1 text-surface-light">
                  Your Projects
                </Typography>
                <Menu placement="left-start">
                  <MenuHandler>
                    <IconButton size="sm" variant="text" color="white">
                      <EllipsisVerticalIcon strokeWidth={3} fill="currenColor" className="h-6 w-6"/>
                    </IconButton>
                  </MenuHandler>
                  <MenuList className="bg-surface-mid border-surface-dark text-surface-light">
                    <MenuItem>
                      Hide completed projects
                    </MenuItem>
                  </MenuList>
                </Menu>
              </CardHeader>
              <CardBody className="px-0 pt-0 pb-2">
                <table className="w-full min-w-[640px] table-auto">
                  <thead>
                   <tr>
                      {["project", "members", "deadline", "total tasks", "completion"].map((headerItem) => (
                        <th key={headerItem} className="border-b border-primary py-3 px-6 text-left">
                          <Typography variant="small"className="text-[11px] font-medium uppercase text-surface-light">
                            {headerItem}
                          </Typography>
                        </th>
                      ))}
                   </tr>
                  </thead>
                 <tbody>
                  {projectsTableData.map(({name, members, budget, completion, path }, index) => {
                    const className = `py-3 px-5 ${index === projectsTableData.length - 1 ? "" : "border-b border-secondary border-opacity-10"}`;
                    return (
                      <tr key={`${name}_${index}`}>
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
                          {members.map(({ img, name }, key) => (
                            <Tooltip key={name} content={name}>
                              <Avatar src={img} alt={name} size="xs" variant="circular" className={`cursor-pointer border-2 border-primary ${key === 0 ? "" : "-ml-2.5"}`}/>
                            </Tooltip>
                          ))}
                        </td>
                        <td className={className}>
                          <Typography variant="small" className="text-xs font-medium text-surface-light" >
                            {index + 12}th of February 2024
                          </Typography>
                        </td>
                        <td className={className}>
                          <Typography variant="small" className="text-xs font-medium text-surface-light" >
                            {budget}
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
                  })}
                 </tbody>
                </table>
              </CardBody>
            </Card>
          </div>
        </div>

        <Card className="bg-transparent shadow-none">
          {/* right container */}
          {statisticsChartsData.map((props) => (
            <StatisticsChart key={props.title} {...props}/>
          ))}
        </Card>
      </div>
    </div>
  );
}

export default Home;

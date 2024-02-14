import { RectangleStackIcon, CogIcon, CheckBadgeIcon } from "@heroicons/react/24/outline";
import { Typography } from "@material-tailwind/react";
import React, { useEffect, useState } from "react";
import api from "../../../../services/api";
import TasksStatsCard from "./TasksStatsCard";
import { TasksFromAllProjects, TaskStats } from "./types";
import { useUser } from "../../../../context/LoginRequired";

export function TasksStats({setProjectsTasksCount, setProjectsCompletion} : any) {
  const user = useUser();
  const [tasksFromAllProjects, setTasksFromAllProjects] = useState({} as TasksFromAllProjects);
  const [tasksStats, setTasksStats] = useState([{ count: 0, footerValue: "0" }, { count: 0, footerValue: "0" }, { count: 0, footerValue: "0" }] as TaskStats[]);
  const [projectsIds, setProjectsIds] = useState([] as string[]);

  //get tasks from all projects
  useEffect(() => {
    (async () => {
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
        console.error(`Error while getting tasks: ${error}`);
      }
    })();
  }, []);

  //compute the tasks stats
  useEffect(() => {
    const projects = Object.keys(tasksFromAllProjects);
    const stats = [{ count: 0, footerValue: "0" }, { count: 0, footerValue: "0" }, { count: 0, footerValue: "0" }] as TaskStats[];
    if(projects.length == 0){
      setTasksStats(stats);
      return;
    }
    let nextDue = new Date();
    nextDue.setFullYear(2100);
    projects.forEach((project) => {
      tasksFromAllProjects[project].forEach((task) => {
        //compute data for the tasks stats
        stats[task.state - 1].count++;
        if(task.state === 2 && new Date(task.deadline) < nextDue) nextDue = new Date(task.deadline); 
      });
      //compute data for the projects stats (next section of the dashboard)
      setProjectsTasksCount((prev : any) => {
        return {
          ...prev,
          [project]: tasksFromAllProjects[project].length,
        }
      });
      setProjectsCompletion((prev : any) => {
        return {
          ...prev,
          [project]: tasksFromAllProjects[project].length === 0 ? 0 :
          tasksFromAllProjects[project].filter((task) => task.state === 3).length / tasksFromAllProjects[project].length * 100,
        }
      });
    });

    //get the number of completed projects
    stats[2].footerValue = projects.filter((project) => {
      if(tasksFromAllProjects[project].length === 0) return false;
      return tasksFromAllProjects[project].every((task) => task.state === 3);
    }).length.toString();

    //get the next due date
    if(stats[1].count === 0) {
      stats[1].footerValue = "No tasks in progress";
    } else {
      stats[1].footerValue = `${new Date(nextDue).toLocaleString("en-US", { month: "long" })} ${new Date(nextDue).getDate()}, ${new Date(nextDue).getFullYear()}`;
    }

    //get the number of projects
    stats[0].footerValue = projects.length.toString();

    setTasksStats(stats);
    setProjectsIds(projects);
  }, [tasksFromAllProjects]);

  return(
    <>
      <TasksStatsCard
        color="bg-[#5e4d8c]"
        count={tasksStats[0].count}
        title="Tasks To Do"
        icon={React.createElement(RectangleStackIcon, {
          className: "w-8 h-8 text-white",
        })}
        footer={
          <Typography className="font-normal text-white">
            {tasksStats[0].count > 0 ?
              <>
                Across {projectsIds.length > 1 ? "all your" : ""} <strong>{tasksStats[0].footerValue}</strong> project{projectsIds.length != 1 ? "s" : ""}
              </>
            :
              "You have no projects"
            }
          </Typography>
        }
      />
      <TasksStatsCard
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
      <TasksStatsCard
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

export default TasksStats;
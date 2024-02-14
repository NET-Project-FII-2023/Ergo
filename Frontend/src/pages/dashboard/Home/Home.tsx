import { useState } from "react";
import { Card } from "@material-tailwind/react";
import { useUser } from "../../../context/LoginRequired";
// import { statisticsChartsData } from "../../../data/statistics-charts-data";
// import StatisticsChart from "./components/StatisticsChart";
import TasksStats from "./components/TasksStats";
import ProjectsStats from "./components/ProjectsStats";
import { UpcomingDeadlines } from "./components/UpcomingDeadlines";

export function Home() {
  const user = useUser();
  const time = new Date().getHours();
  const [projectsTasksCount, setProjectsTasksCount] = useState({} as { [projectId: string]: number});
  const [projectsCompletion, setProjectsCompletion] = useState({} as { [projectId: string]: number});
  
  return (
    <div className="mt-12 text-surface-light">
      <div className="mb-16">
        <h2 className="text-5xl font-bold">
          Good {(time < 4 || time >= 18) ? "evening" : time <= 12 ? "morning" : "afternoon"}, <span className="text-white">{user?.username}</span>!
        </h2>
      </div>
      {/* main container */}
      <div className="grid gap-y-10 2xl:gap-x-6 grid-cols-2 2xl:grid-cols-3">
        {/* left container */}
        <div className="col-span-3 2xl:col-span-2">
          <div className="grid grid-cols-3 gap-x-3">
            <TasksStats setProjectsTasksCount={setProjectsTasksCount} setProjectsCompletion={setProjectsCompletion} />
            <ProjectsStats projectsTasksCount={projectsTasksCount} projectsCompletion={projectsCompletion} />
          </div>
        </div>

        {/* right container */}
        <Card className="bg-transparent shadow-none col-span-2 lg:col-span-1">
          {/* {statisticsChartsData.map((props) => (
            <StatisticsChart key={props.title} {...props}/>
          ))} */}
          <UpcomingDeadlines />
        </Card>
      </div>
    </div>
  );
}

export default Home;

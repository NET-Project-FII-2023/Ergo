import { Card } from "@material-tailwind/react";
import { useUser } from "../../../context/LoginRequired";
import { StatisticsChart } from "../../../widgets/charts";
import {
  statisticsChartsData,
} from "../../../data";
import TasksStats from "./components/TasksStats";
import { ProjectsStats } from "./components/ProjectsStats";
import { useState } from "react";

export function Home() {
  const user = useUser();
  const time = new Date().getHours();
  const [projectsTasksCount, setProjectsTasksCount] = useState({} as { [projectId: string]: number});
  const [projectsCompletion, setProjectsCompletion] = useState({} as { [projectId: string]: number});
  
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
            <TasksStats setProjectsTasksCount={setProjectsTasksCount} setProjectsCompletion={setProjectsCompletion} />
            <ProjectsStats projectsTasksCount={projectsTasksCount} projectsCompletion={projectsCompletion} />
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

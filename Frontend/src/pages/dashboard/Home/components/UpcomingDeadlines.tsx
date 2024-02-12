import { useEffect, useState } from "react";
import StatisticsChart from "./StatisticsChart";
import api from "../../../../services/api";
import { useUser } from "../../../../context/LoginRequired";
import chartsConfig from "../../../../configs/charts-config";

export function UpcomingDeadlines(){
  const user = useUser();
  const [chart, setChart] = useState({} as object);

  useEffect(() => {
    (async () => {
      try {
        const response = await api.get(`/api/v1/Statistics/GetTasksDueThisWeek/${user.userId}`, {
          headers: {
            Authorization: `Bearer ${user.token}`,
          },
        });
        if (response.status !== 200) {
          throw new Error(response.data.message);
        }
        const tasks = response.data.tasksDueThisWeek;
        const chartData = {
          type: "bar",
          height: 200,  
          series: [
            {
              name: "Tasks",
              data: getTasksInOrder(tasks),
            },
          ],
          options: {
            ...chartsConfig,
            colors: "#ba9ffb",
            plotOptions: {
              bar: {
                columnWidth: "16%",
                borderRadius: 5,
              },
            },
            xaxis: {
              ...chartsConfig.xaxis,
              categories: getDaysInOrder(),
            },
          },
        }
        setChart(chartData);
      } catch (error : any) { 
        console.error(`Error while getting stats: ${error}`);
      }
    })();
  }, []);

  function getTasksInOrder(tasks: any) {
    const todayIndex = new Date().getDay();
    const result = Array(7).fill(0);
    tasks.forEach((task: any) => {
      if(task.state === 3) return;
      const dueDay = new Date(task.deadline).getDay();
      const daysUntilDue = (dueDay - todayIndex + 7) % 7;
      result[daysUntilDue] += 1;
    });
    return result;
  }

  function getDaysInOrder() {
    const weekdays = ['Sun', 'Mon', 'Tue', 'Wed', 'Thu', 'Fri', 'Sat'];
    const todayIndex = new Date().getDay();
    const result = [];
    
    for (let i = 0; i < 7; i++) {
        const index = (todayIndex + i) % 7;
        result.push(weekdays[index]);
    }
    
    return result;
  }

  return(
    <>
    {Object.keys(chart).length > 0 &&
      <StatisticsChart
        chart={chart}
        title="Upcoming Deadlines"
        description="Tasks due this week"
      />
    }
    </>
  )
}
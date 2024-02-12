import { chartsConfig } from "../configs/charts-config";

const totalTasksChart = {
  type: "line",
  height: 200,
  series: [
    {
      name: "Tasks",
      data: [50, 0, 30, 22, 50, 20, 40],
    },
  ],
  options: {
    ...chartsConfig,
    colors: ["#a688fa"],
    stroke: {
      lineCap: "round",
    },
    markers: {
      size: 6,
    },
    xaxis: {
      ...chartsConfig.xaxis,
      categories: [
        "Mon",
        "Tue",
        "Wed",
        "Thu",
        "Fri",
        "Sat",
        "Sun"
      ],
    },
  },
};

const upcomingDeadlinesChart = {
  type: "bar",
  height: 200,  
  series: [
    {
      name: "Tasks",
      data: [50, 20, 10, 22, 50, 10, 40],
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
      categories: ["M", "T", "W", "T", "F", "S", "S"],
    },
  },
};

export const statisticsChartsData = [
  {
    title: "Total Tasks Added",
    description: "Accross all projects this week",
    chart: totalTasksChart,
  },
  {
    title: "Upcoming Deadlines",
    description: "Tasks that are due this week",
    chart: upcomingDeadlinesChart,
  },
];

export default statisticsChartsData;

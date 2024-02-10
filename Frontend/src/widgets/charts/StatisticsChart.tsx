import {
  Card,
  CardHeader,
  CardBody,
  CardFooter,
  Typography,
} from "@material-tailwind/react";
import Chart from "react-apexcharts";

interface StatisticsChartProps {
  chart: object;
  title: React.ReactNode;
  description: React.ReactNode;
}

export function StatisticsChart({ chart, title, description } : StatisticsChartProps) {
  return (
    <Card className="bg-surface-dark shadow-sm mb-4">
      <CardHeader variant="gradient" className="bg-surface-darkest" floated={false} shadow={false}>
        <Chart {...chart} />
      </CardHeader>
      <CardBody className="mt-4 px-6 pt-0">
        <Typography variant="h6" className="text-white">
          {title}
        </Typography>
        <Typography variant="small" className="font-normal text-surface-light">
          {description}
        </Typography>
      </CardBody>
    </Card>
  );
}

export default StatisticsChart;

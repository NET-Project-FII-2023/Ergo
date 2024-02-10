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
import { StatisticsChart } from "../../../widgets/charts";
import {
  statisticsChartsData,
  projectsTableData,
} from "../../../data";
import { Link } from "react-router-dom";
import TasksStats from "./components/TasksStats";

export function Home() {
  const user = useUser();
  const time = new Date().getHours();
  
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
            <TasksStats />
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

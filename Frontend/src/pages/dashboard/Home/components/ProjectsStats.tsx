import { EllipsisVerticalIcon } from "@heroicons/react/24/outline";
import { Card, CardHeader, Typography, Menu, MenuHandler, IconButton, MenuList, MenuItem, CardBody } from "@material-tailwind/react";
import { ProjectsStatsItem } from "./ProjectsStatsItem";
import { ProjectsStatsType } from "./types";
import { useEffect, useState } from "react";
import api from "../../../../services/api";
import { useUser } from "../../../../context/LoginRequired";

export function ProjectsStats({ projectsTasksCount, projectsCompletion } : any){
  const user = useUser();
  const [projectsStats, setProjectsStats] = useState({} as ProjectsStatsType);
  useEffect(() => {
    if(Object.keys(projectsTasksCount).length <= 0) return;
    (async () => {
      console.log('getting projects')
      try{
        const response = await api.get(`/api/v1/Projects/GetProjectsByUserId/${user.userId}`, {
          headers: {
            Authorization: `Bearer ${user.token}`,
          },
        });
        if (response.status !== 200) {
          throw new Error(response.data.message);
        }
        const projects = response.data.projects;
        projects.forEach((project : any) => {
          setProjectsStats((prev : ProjectsStatsType) => {
            return {
              ...prev,
              [project.projectId]: {
                name: project.projectName,
                members: [
                  project.members.map((member : any) => {
                    // const img = getUserImage(member.userId);
                    // const name = getUserNames(member.userId);
                    return {
                      img: "https://via.placeholder.com/150",
                      name: "John Doe",
                    }
                  }),
                ],
                deadline: project.deadline,
                totalTasksCount: projectsTasksCount[project.projectId],
                completion: projectsCompletion[project.projectId],
                path: `/dashboard/project/${project.projectId}`,
              }
            }
          });
        });
      }
      catch (error : any) {
        console.log(`Error while getting projects: ${error.response.data}`);
      }
    })();
  }, [projectsTasksCount, projectsCompletion]);
  return(
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
            {projectsStats && Object.keys(projectsStats).map((projectId, index) => {
              const className = `py-3 px-5 ${index === Object.keys(projectsStats).length - 1 ? "" : "border-b border-secondary border-opacity-10"}`;
              const project = projectsStats[projectId];
              return (
                <ProjectsStatsItem 
                  key={projectId} 
                  name={project.name} 
                  members={project.members} 
                  deadline={project.deadline}
                  totalTasksCount={project.totalTasksCount}
                  completion={project.completion} 
                  path={project.path} 
                  className={className} />
              );
            })
            }
          </tbody>
        </table>
      </CardBody>
    </Card>
  )
}

export default ProjectsStats;

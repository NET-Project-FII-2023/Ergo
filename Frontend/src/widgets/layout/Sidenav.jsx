import PropTypes from "prop-types";
import { Link, NavLink } from "react-router-dom";
import { XMarkIcon } from "@heroicons/react/24/outline";
import {
  RectangleStackIcon,
  TrashIcon
} from "@heroicons/react/24/solid";
import {
  Button,
  IconButton,
  Typography,
} from "@material-tailwind/react";
import { useMaterialTailwindController, setOpenSidenav } from "@/context/MaterialTailwind.jsx";
import { useEffect, useState } from "react";
import api from "@/services/api";
import { useUser } from "@/context/LoginRequired";
import AddProject from "../../pages/projectOverview/AddProject";
import { toast } from "react-toastify";
import {useNavigate} from "react-router-dom";
import DeleteProjectModal from "../../common/components/DeleteProjectModal";

export function Sidenav({ brandImg, brandName, routes }) {
  const navigate = useNavigate();
  const [controller, dispatch] = useMaterialTailwindController();
  const { openSidenav } = controller;
  const [projects, setProjects] = useState([]);
  const { token, userId, username } = useUser();
  const [deletingProject, setDeletingProject] = useState({});
  const [isDeleteProjectModalOpen, setIsDeleteProjectModalOpen] = useState(false);

  useEffect(() => {
    fetchProjects();
  }, []);

  const fetchProjects = async () => {
    try {
      if (!token || !userId) return;

      const response = await api.get(`/api/v1/Projects/GetProjectsByUserId/${userId}`, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      });

      if (response.status === 200) {
        setProjects(response.data.projects);
      } else {
        console.error('Error fetching projects:', response);
      }
    } catch (error) {
      console.error('Error fetching projects:', error);
    }
  };

  const handleProjectAdded = () => {
    fetchProjects();
  };
  const handleDeleteProject = async (projectId) => {
    try{
        const response = await api.delete(`/api/v1/Projects/${projectId}`, {
            headers: {
              Authorization: `Bearer ${token}`,
            },
            data:{
                projectId: projectId,
                owner: username
            },
        });

        if (response.status === 200) {
            console.log('Project deleted successfully');
            toast.success('Project deleted successfully!');
            fetchProjects();
            navigate('/dashboard/home');
        } else {
            console.error('Error deleting project:', response);
            toast.error(response);
        }
    }catch (error) {
        console.error('Error deleting project:', error);
        toast.error('Error deleting project:' + error);
    }
}

  return (
    <aside
      className={`bg-surface-dark px-4 ${
        openSidenav ? "translate-x-0" : "-translate-x-80"
      } fixed inset-0 z-50 my-4 ml-4 w-72 rounded-xl transition-transform duration-300 xl:translate-x-0 `}
    >
      <div className={`relative`}>
        <Link to="/" className="pt-5 flex items-center justify-center">
          <img src={brandImg} alt="logo" className="h-24 ml-[-0.75rem]" />
          {/* <Typography
            variant="h3"
            color={"white"}
            className="w-min"
          >
            {brandName}
          </Typography> */}
        </Link>
        <IconButton
          variant="text"
          color="white"
          size="sm"
          ripple={false}
          className="absolute right-0 top-0 grid rounded-br-none rounded-tl-none xl:hidden"
          onClick={() => setOpenSidenav(dispatch, false)}
        >
          <XMarkIcon strokeWidth={2.5} className="h-5 w-5 text-white" />
        </IconButton>
      </div>
      <div className="my-4">
        {routes.map(({ layout, pages }, key) => (
          <ul key={key} className="flex flex-col gap-1 opacity-80">
            {pages
              .filter(({ name }) => layout === "dashboard" && !["project", "profile"].includes(name))
              .map(({ icon, name, path }) => (
                <li key={name} className="py-1 ![&>a]:flex-1">
                  <NavLink to={`/${layout}${path}`} className={"flex-1"}>
                    {({ isActive }) => (
                      <Button
                        className={`flex gap-2 items-center px-4 hover:bg-secondary ${isActive ? 'bg-primary' : 'bg-surface-dark shadow-none'} `}
                        fullWidth
                      >
                        {icon}
                        <Typography
                          color="inherit"
                          className={`font-medium capitalize`}
                        >
                          {name}
                        </Typography>
                      </Button>
                    )}
                  </NavLink>
                </li>
              ))}
          </ul>
        ))}
      </div>
      <hr className="border-t border-surface-mid  mt-2" />
      <h5 className="text-surface-light font-bold my-2">Projects</h5>
      <div className="overflow-y-auto max-h-[calc(100vh-22rem)]"  style={{ scrollbarWidth: 'thin', scrollbarColor: '#1a1625' }}>
        <ul className="list-none">
          {projects.map((project) => (
            <li key={project.projectId} className="py-1 opacity-90 flex items-center">
              <NavLink to={`/dashboard/project/${project.projectId}`} className={`flex-1`}>
                {({ isActive }) => (
                  <Button
                    className={`flex items-center gap-2 px-4 capitalize hover:bg-secondary group ${isActive ? 'bg-secondary' : 'bg-transparent shadow-none'}`}
                    fullWidth
                  >
                    <RectangleStackIcon className={`w-5 h-5 text-inherit text-white`} />
                    <Typography
                      color="inherit"
                      className={`font-normal text-sm text-gray-200 ${isActive ? 'text-gray-100' : 'text-gray-300'}`}
                    >
                      {project.projectName.length > 25 ? project.projectName.substring(0, 22) + '...' : project.projectName}
                    </Typography>
                    {project.createdBy === username && 
                      <TrashIcon 
                        className="text-white !hidden w-5 duration-150 h-5 group-hover:!block ml-auto hover:text-red-400" 
                        title="Delete Project"
                        onClick={(e) => {
                          e.preventDefault(); 
                          setDeletingProject({id:project.projectId, name: project.projectName}); 
                          setIsDeleteProjectModalOpen(true);
                        }} 
                      />
                    }
                  </Button>
                )}
              </NavLink>
            </li>
          ))}
        </ul>
      </div>
      <AddProject onProjectAdded={handleProjectAdded}/>
      <DeleteProjectModal
        open={isDeleteProjectModalOpen}
        onClose={() => setIsDeleteProjectModalOpen(false)}
        onConfirm={(projectId) => handleDeleteProject(projectId)}
        projectData={{id: deletingProject?.id, name: deletingProject?.name}}
      />
    </aside>
  );
}

Sidenav.defaultProps = {
  brandImg: "/img/ergo_logo.png",
  brandName: "Ergo",
};

Sidenav.propTypes = {
  brandImg: PropTypes.string,
  brandName: PropTypes.string,
  routes: PropTypes.arrayOf(PropTypes.object).isRequired,
};

Sidenav.displayName = "/src/widgets/layout/sidenav.jsx";

export default Sidenav;

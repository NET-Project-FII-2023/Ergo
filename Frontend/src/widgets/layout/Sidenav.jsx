import PropTypes from "prop-types";
import { Link, NavLink, useLocation } from "react-router-dom";
import { XMarkIcon } from "@heroicons/react/24/outline";
import {
  RectangleStackIcon,
} from "@heroicons/react/24/solid";
import {
  Button,
  IconButton,
  Typography,
} from "@material-tailwind/react";
import { useMaterialTailwindController, setOpenSidenav } from "@/context";
import { useEffect, useState } from "react";
import api from "@/services/api";
import {useUser} from "@/context/LoginRequired.jsx";

export function Sidenav({ brandImg, brandName, routes }) {
  const [controller, dispatch] = useMaterialTailwindController();
  const { openSidenav } = controller;
  const { pathname } = useLocation();
  const [projects, setProjects] = useState([]);
  const {token, userId} = useUser();

  useEffect(() => {
    (async () => {
      if (!token) return;
      if (!userId) return;

      const userProjects = await getProjectsByUserId(userId, token);

      if (userProjects?.success) {
        setProjects(userProjects.projects);
        console.log("Projects:")
        console.log(userProjects.projects);
      } else {
        console.log("Error while fetching user projects");
      }
    })();
  }, []);

  async function getProjectsByUserId(userId, token) {
    try {
      const response = await api.get(`/api/v1/Projects/GetProjectsByUserId/${userId}`, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      });
      if (response.status !== 200) {
        throw new Error(response);
      }
      return response.data;
    } catch (error) {
      console.log(`Error while getting user projects: ${error.response.data}`);
    }
  }

  return (
    <aside
      className={`bg-white shadow-sm ${
        openSidenav ? "translate-x-0" : "-translate-x-80"
      } fixed inset-0 z-50 my-4 ml-4 h-[calc(100vh-32px)] w-72 rounded-xl transition-transform duration-300 xl:translate-x-0 border border-blue-gray-100`}
    >
      <div
        className={`relative`}
      >
        <Link to="/" className="py-6 px-8 text-center">
          <Typography
            variant="h6"
            color={"blue-gray"}
          >
            {brandName}
          </Typography>
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
      <div className="m-4">
        {routes.map(({ layout, title, pages }, key) => (
          <ul key={key} className="mb-4 flex flex-col gap-1">
            {title && (
              <li className="mx-3.5 mt-4 mb-2">
                <Typography
                  variant="small"
                  color={"blue-gray"}
                  className="font-black uppercase opacity-75"
                >
                  {title}
                </Typography>
              </li>
            )}
            {pages.slice(0,-1).map(({ icon, name, path }) => (
              <li key={name}>
                <NavLink to={`/${layout}${path}`}>
                  {({ isActive }) => (
                    <Button
                      variant={isActive ? "gradient" : "text"}
                      color={
                        isActive ? "blue-gray" : undefined
                      }
                      className="flex items-center gap-4 px-4 capitalize"
                      fullWidth
                    >
                      {icon}
                      <Typography
                        color="inherit"
                        className={`font-medium capitalize ${isActive ? 'opacity-100' : 'opacity-75'}`}
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
        <ul className={"list-none"}>
        {projects.map((project) => (
        <li key={project.projectId}>
          <NavLink to={`/dashboard/project/${project.projectId}`}>
            <Link to={`/dashboard/project/${project.projectId}`}>
              <Button
                variant="text"
                color="blue-gray"
                className="flex items-center gap-4 px-4 capitalize"
                fullWidth
              >
                <RectangleStackIcon className="w-5 h-5 text-inherit" />
                <Typography
                  color="inherit"
                  className="font-medium capitalize"
                >
                  {project.projectName}
                </Typography>
              </Button>
            </Link>
          </NavLink>
        </li>
      ))}
        </ul>
      </div>
    </aside>
  );
}

Sidenav.defaultProps = {
  brandImg: "/img/logo-ct.png",
  brandName: "Material Tailwind React",
};

Sidenav.propTypes = {
  brandImg: PropTypes.string,
  brandName: PropTypes.string,
  routes: PropTypes.arrayOf(PropTypes.object).isRequired,
};

Sidenav.displayName = "/src/widgets/layout/sidenav.jsx";

export default Sidenav;

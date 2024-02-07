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
import { useMaterialTailwindController, setOpenSidenav } from "@/context/MaterialTailwind.jsx";
import { useEffect, useState } from "react";
import api from "@/services/api";
import {useUser} from "@/context/LoginRequired";

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
      className={`bg-surface-mid ${
        openSidenav ? "translate-x-0" : "-translate-x-80"
      } fixed inset-0 z-50 my-4 ml-4 h-[calc(100vh-32px)] w-64 rounded-xl transition-transform duration-300 xl:translate-x-0`}
    >
      <div
        className={`relative`}
      >
        <Link to="/" className="py-5 flex items-center justify-center">
          <img src={brandImg} alt="logo" className="h-12 ml-[-0.75rem]" />
            <Typography
              variant="h3"
              color={"white"}
              className="w-min"
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
        {routes.map(({ layout, pages }, key) => (
          <ul key={key} className=" flex flex-col gap-1 opacity-80">
            {pages
            .filter(({ name }) => name !== "project" && layout =="dashboard" && name !== 'profile')
            .map(({ icon, name, path }) => (
              <li key={name} className="py-1">
                <NavLink to={`/${layout}${path}`}>
                  {({ isActive }) => (
                    <Button
                      className={`flex items-center gap-2 px-4 hover:bg-secondary ${isActive ? 'bg-secondary' : 'bg-transparent shadow-none'} `}
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
        <ul className={"list-none"}>
        {projects.map((project) => (
        <li key={project.projectId} className="py-1 opacity-80">
          <NavLink to={`/dashboard/project/${project.projectId}`} >
          {({ isActive }) => (
            <Link to={`/dashboard/project/${project.projectId}`}>
              <Button
                className={`flex items-center gap-2 px-4 capitalize hover:bg-secondary ${isActive ? 'bg-secondary' : 'bg-transparent shadow-none'} `}
                fullWidth
              >
                 <RectangleStackIcon className={`w-4 h-4 text-inherit text-white`} />
                <Typography
                  color="inherit"
                  className="font-normal text-white"
                >
                  {project.projectName}
                </Typography>
              </Button>
            </Link>
             )}
          </NavLink>
        </li>
      ))}
        </ul>
      </div>
    </aside>
  );
}

Sidenav.defaultProps = {
  brandImg: "/img/logo.png",
  brandName: "Ergo",
};

Sidenav.propTypes = {
  brandImg: PropTypes.string,
  brandName: PropTypes.string,
  routes: PropTypes.arrayOf(PropTypes.object).isRequired,
};

Sidenav.displayName = "/src/widgets/layout/sidenav.jsx";

export default Sidenav;

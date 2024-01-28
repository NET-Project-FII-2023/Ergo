import { useLocation, Link } from "react-router-dom";
import {
  Navbar,
  Typography,
  Button,
  IconButton,
  Breadcrumbs,
  Input,
  Menu,
  MenuHandler,
  MenuList,
  MenuItem,
  Avatar,
} from "@material-tailwind/react";
import {
  UserCircleIcon,
  BellIcon,
  ClockIcon,
  CreditCardIcon,
  Bars3Icon,
} from "@heroicons/react/24/solid";
import {
  useMaterialTailwindController,
  setOpenSidenav,
} from "@/context";
import { useEffect, useState } from "react";
import api from "@/services/api";

export function DashboardNavbar() {
  const [controller, dispatch] = useMaterialTailwindController();
  const { openSidenav } = controller;
  const { pathname } = useLocation();
  const [layout, page] = pathname.split("/").filter((el) => el !== "");

  const [notifications, setNotifications] = useState([]);

  useEffect(() => {
    (async () => {
      //========= HARDCODED LOGIN AND USER INFORMATION
      const token = await hardcodedLogin("marcel", "Alin24218!");
      if(!token) return;
      const { userId } = (await getUserWithEmail("marcel@gmail.com", token))?.user;
      if(!userId) return;
      //========= END OF HARDCODED SHIT

      //get notifications
      const inboxItems = await fetchNotifications(userId, token);
      // setNotifications(inboxItems);
      console.log(inboxItems);
    })();
  }, []);

  async function hardcodedLogin(username, password) {
    try {
      const response = await api.post("/api/v1/Authentication/login", {
        username,
        password,
      });
      if (response.status !== 200) {
        throw new Error(response);
      }
      return response.data;
    } catch (error) {
      console.log(`Error in hardcoded login: ${error.response.data}`);
    }
  }

  async function getUserWithEmail(email, token) {
    try {
      const response = await api.get(`/api/v1/Users/ByEmail/${email}`, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      });
      if (response.status !== 200) {
        throw new Error(response);
      }
      return response.data;
    } catch (error) {
      console.log(`Error while getting user id: ${error.response.data}`);
    }
  }

  async function fetchNotifications(userId, token){
    try {
      const response = await api.get(`/api/v1/InboxItem/${userId}`, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      });
      if (response.status !== 200) {
        throw new Error(response);
      }
      return response.data.inboxItems;
    } catch (error) { 
      console.log(`Error while getting notifications: ${error.response.data}`);
    }
  } 

  return (
    <Navbar
      color={"transparent"}
      className={"rounded-xl transition-all px-0 py-1"}
      fullWidth
    >
      <div className="flex flex-col-reverse justify-between gap-6 md:flex-row md:items-center">
        <div className="capitalize">
          <Breadcrumbs
            className={"bg-transparent p-0 transition-all"}
          >
            <Link to={`/${layout}`}>
              <Typography
                variant="small"
                color="blue-gray"
                className="font-normal opacity-50 transition-all hover:text-blue-500 hover:opacity-100"
              >
                {layout}
              </Typography>
            </Link>
            <Typography
              variant="small"
              color="blue-gray"
              className="font-normal"
            >
              {page}
            </Typography>
          </Breadcrumbs>
          <Typography variant="h6" color="blue-gray">
            {page}
          </Typography>
        </div>
        <div className="flex items-center">
          <IconButton
            variant="text"
            color="blue-gray"
            className="grid xl:hidden"
            onClick={() => setOpenSidenav(dispatch, !openSidenav)}
          >
            <Bars3Icon strokeWidth={3} className="h-6 w-6 text-blue-gray-500" />
          </IconButton>
          <Link to="/auth/sign-in">
            <Button
              variant="text"
              color="blue-gray"
              className="hidden items-center gap-1 px-4 xl:flex normal-case"
            >
              <UserCircleIcon className="h-5 w-5 text-blue-gray-500" />
              Sign In
            </Button>
            <IconButton
              variant="text"
              color="blue-gray"
              className="grid xl:hidden"
            >
              <UserCircleIcon className="h-5 w-5 text-blue-gray-500" />
            </IconButton>
          </Link>

          {/*Notifications*/}
          <Menu>
            <MenuHandler>
              <IconButton variant="text" color="blue-gray">
                <BellIcon className="h-5 w-5 text-blue-gray-500" />
              </IconButton>
            </MenuHandler>
            <MenuList className="w-max border-0">
              {notifications.length === 0 ?
                <MenuItem className="flex items-center gap-3" disabled>
                  <strong>No new notifications</strong>
                </MenuItem> 
                : 
                notifications.map((notification) => (
                  <MenuItem className="flex items-center gap-3">
                    <Avatar
                      src="https://demos.creative-tim.com/material-dashboard/assets/img/team-2.jpg"
                      alt="item-1"
                      size="sm"
                      variant="circular"
                    />
                    <div>
                      <Typography
                        variant="small"
                        color="blue-gray"
                        className="mb-1 font-normal"
                      >
                        <strong>New message</strong> from {notification.senderName}
                      </Typography>
                      <Typography
                        variant="small"
                        color="blue-gray"
                        className="flex items-center gap-1 text-xs font-normal opacity-60"
                      >
                        <ClockIcon className="h-3.5 w-3.5" /> {notification.date}
                      </Typography>
                    </div>
                  </MenuItem>
                ))
              }
            </MenuList>
          </Menu>
        </div>
      </div>
    </Navbar>
  );
}

DashboardNavbar.displayName = "/src/widgets/layout/dashboard-navbar.jsx";

export default DashboardNavbar;

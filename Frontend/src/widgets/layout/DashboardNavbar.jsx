import { useLocation, Link } from "react-router-dom";
import {
  Navbar,
  Typography,
  Button,
  IconButton,
  Breadcrumbs,
  Menu,
  MenuHandler,
  MenuList,
  ListItem
} from "@material-tailwind/react";
import {
  UserCircleIcon,
  BellIcon,
  ClockIcon,
  Bars3Icon,
  CheckIcon
} from "@heroicons/react/24/solid";
import {
  useMaterialTailwindController,
  setOpenSidenav,
} from "@/context";
import { useEffect, useState } from "react";
import api from "@/services/api";
import { toast } from "react-toastify";

export function DashboardNavbar() {
  const [controller, dispatch] = useMaterialTailwindController();
  const { openSidenav } = controller;
  const { pathname } = useLocation();
  const [layout, page] = pathname.split("/").filter((el) => el !== "");

  const [notifications, setNotifications] = useState([]);
  const [hardcodedToken, setHardcodedToken] = useState("");

  useEffect(() => {
    (async () => {
      //========= HARDCODED LOGIN AND USER INFORMATION
      const token = await hardcodedLogin("tudstk", "Abc123!");
      if (!token) return;
      setHardcodedToken(token);
      const { userId } = (await getUserWithEmail("tudorstroescu@yahoo.com", token))?.user;
      if (!userId) return;
      //========= END OF HARDCODED SHIT

      //get notifications
      const inboxItems = await fetchNotifications(userId, token);

      //sort them by createdDate
      inboxItems.sort((a, b) => new Date(b.createdDate) - new Date(a.createdDate));

      setNotifications(inboxItems);
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

  async function markAsRead(e, notification) {
    e.stopPropagation();
    console.log(notification);
    try {
      const response = await api.put(`/api/v1/InboxItem/${notification.inboxItemId}`, {
        isRead: true,
        createdDate: notification.createdDate,
        message: notification.message,
        userId: notification.userId,
      }, {
        headers: {
          Authorization: `Bearer ${hardcodedToken}`,
        },
      });
      if (response.status !== 200) {
        throw new Error(response);
      }
      setNotifications(prev => prev.map(notif => notif.inboxItemId === notification.inboxItemId ? {...notif, isRead: true} : notif));
      toast.success("Notification marked as read!");
    } catch (error) {
      console.log(`Error while marking notification as read: ${error.response.data}`);
      toast.error("Couldn't update notification");
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
            <MenuList className="border-0 max-w-sm max-h-[80vh] minimal-scrollbar">
              {notifications.length === 0 ?
                <ListItem disabled className="text-black opacity-70">
                  <strong>No new notifications</strong>
                </ListItem> 
                : 
                notifications.map((notification, index) =>
                  <ListItem className="flex flex-col items-start w-[20rem]" key={`notif-${index}`}>
                    <Typography variant="small" color="blue-gray" className="mb-1 font-normal">
                      <strong>{notification.message}</strong>
                    </Typography>

                    <div className="flex justify-between w-full">
                      <Typography variant="small" color="blue-gray" className="flex items-center gap-1 text-xs font-normal opacity-60">
                        <ClockIcon className="h-3.5 w-3.5" /> {formatDate(notification.createdDate)}
                      </Typography>
                      {notification.isRead === false ?
                        <Button variant="text" color="red" size="sm" className="text-xs px-2 py-1 opacity-70 font-normal"
                          onClick={(e) => markAsRead(e, notification)}
                        >
                          Mark as read
                        </Button>
                        :
                        <Typography variant="small" color="blue-gray" className="flex items-center gap-1 text-xs font-normal opacity-60">
                          <CheckIcon className="h-3.5 w-3.5" /> Read
                        </Typography>
                      }
                    </div>
                  </ListItem>
                )
              }
            </MenuList>
          </Menu>
        </div>
      </div>
    </Navbar>
  );
}

DashboardNavbar.displayName = "/src/widgets/layout/dashboard-navbar.jsx";

function formatDate(date) {
  return new Intl.DateTimeFormat("ro", {
    day: "numeric",
    month: "numeric",
    year: "numeric",
    hour: "numeric",
    minute: "numeric",
    hour12: false,
    timeZone: "Europe/Bucharest",
  }).format(new Date(date));
}

export default DashboardNavbar;

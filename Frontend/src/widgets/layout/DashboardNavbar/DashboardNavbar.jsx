import { useLocation, Link } from "react-router-dom";
import { Navbar, Typography, Button, IconButton, Breadcrumbs, Menu, MenuHandler, MenuList, ListItem } from "@material-tailwind/react";
import { UserCircleIcon, BellIcon, ClockIcon, Bars3Icon, CheckIcon} from "@heroicons/react/24/solid";
import { useMaterialTailwindController, setOpenSidenav } from "../../../context/MaterialTailwind.jsx";
import { useEffect, useState } from "react";
import api from "../../../services/api";
import { toast } from "react-toastify";
import { useUser } from "../../../context/LoginRequired";
import DashboardSearch from "./DashboardSearch"
import { Checkbox } from '@material-tailwind/react';
import { readNotification } from "../../../services/notifications/readNotification";

export function DashboardNavbar() {
  const [controller, dispatch] = useMaterialTailwindController();
  const { userId, username, token } = useUser();
  const { openSidenav } = controller;
  const { pathname } = useLocation();
  const [layout, page] = pathname.split("/").filter((el) => el !== "");
  const [notifications, setNotifications] = useState([]);
  const [isReadHidden, setIsReadHidden] = useState(true);

  useEffect(() => {
    (async () => {
      //get notifications
      const inboxItems = await fetchNotifications(userId, token);
      inboxItems.sort((a, b) => new Date(b.createdDate) - new Date(a.createdDate));

      setNotifications(inboxItems);
    })();
  }, []);

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
      console.error(`Error while getting notifications: ${error.response.data}`);
    }
  }

  async function markAsRead(e, notification) {
    e.stopPropagation();
    const readStatus = await readNotification(notification, token);
    if (readStatus) {
      setNotifications(prev => prev.map(notif => notif.inboxItemId === notification.inboxItemId ? {...notif, isRead: true} : notif));
      toast.success("Notification marked as read!");
    } else {
      toast.error("Couldn't update notification");
    }
  }

  return (
    <Navbar
      color={"transparent"}
      className={"rounded-xl  transition-all px-0 py-1"}
      fullWidth
    >
      <div className="flex flex-col-reverse justify-between gap-6 md:flex-row md:items-center">
        <div className="capitalize text-surface-light">
          <Breadcrumbs
            className={"bg-transparent p-0 transition-all"}
          >
            <Link to={`/${layout}`}>
              <Typography
                variant="small"
                className="font-normal opacity-60 transition-all text-primary hover:opacity-100 hover:text-primary"
              >
                {layout}
              </Typography>
            </Link>
            <Typography
              variant="small"
              className="font-normal text-surface-light "
            >
              {page}
            </Typography>
          </Breadcrumbs>
        </div>
        <div className="flex items-center">
          <div className="mr-auto md:mr-4 md:w-56">
            <DashboardSearch />
          </div>
          <IconButton
            variant="text"
            color="blue-gray"
            className="grid xl:hidden"
            onClick={() => setOpenSidenav(dispatch, !openSidenav)}
          >
            <Bars3Icon strokeWidth={3} className="h-6 w-6 text-blue-gray-500"/>
          </IconButton>
          <Link to="/dashboard/profile">
            <Button
              variant="text"
              color="blue-gray"
              className="hidden items-center gap-1  text-primary px-4 xl:flex normal-case"
            >
              <UserCircleIcon className="h-5 w-5 text-primary"/>
              {username}
            </Button>
            <IconButton
              variant="text"
              color="blue-gray"
              className="grid xl:hidden"
            >
              <UserCircleIcon className="h-5 w-5 text-blue-gray-500"/>
            </IconButton>
          </Link>

          {/*Notifications*/}
          <Menu>
            <MenuHandler>
              <IconButton variant="text" color="blue-gray">
                <BellIcon className="h-5 w-5 text-primary"/>
              </IconButton>
            </MenuHandler>
            <MenuList className="border-0 bg-surface-dark max-w-sm max-h-[80vh] minimal-scrollbar text-surface-light">
              <div className="px-2 pb-1 flex justify-between items-center focus:outline-0">
                <Typography variant="h5" className={`${notifications.length === 0 || notifications.some(notification => notification.isRead === true) ? "w-[200px]" : ""}`}>
                  Notifications
                </Typography>
                {notifications.length > 0 && notifications.some(notification => notification.isRead === true) &&
                  <Checkbox 
                    label={
                    <Typography variant="small" className="text-surface-light opacity-80" onClick={(e) => e.stopPropagation()}>
                      Hide read
                    </Typography>
                    }
                    checked={isReadHidden}
                    onChange={()=> {setIsReadHidden(!isReadHidden)}}
                    onClick={(e) => {e.stopPropagation()}}
                    className="checked:bg-primary"
                    size="small"
                    containerProps={{className: "flex-row-reverse"}}
                  />
                }
              </div>
              {notifications.length === 0 ?
                <ListItem disabled className="!text-surface-light opacity-70">
                  <strong>No new notifications</strong>
                </ListItem>
                :
                notifications.map((notification, index) => {
                  if (isReadHidden && notification.isRead === true) {
                    return null;
                  }
                  return (
                    <ListItem key={`notif-${index}`} className="flex flex-col items-start w-full md:w-[20rem] focus:bg-surface-mid-dark focus:text-surface-light active:bg-surface-mid-dark active:text-surface-light" >
                      <Typography variant="small" className="mb-1 font-normal ">
                        <strong>{notification.message}</strong>
                      </Typography>

                      <div className="flex justify-between w-full">
                        <Typography variant="small" className="flex items-center gap-1 text-xs font-normal opacity-60">
                          <ClockIcon className="h-3.5 w-3.5 text-secondary"/> {formatDate(notification.createdDate)}
                        </Typography>
                        {notification.isRead === false ?
                          <Button variant="text" color="red" size="sm"
                                  className="text-xs px-2 py-1 opacity-70 font-normal"
                                  onClick={(e) => markAsRead(e, notification)}
                          >
                            Mark as read
                          </Button>
                          :
                          <Typography variant="small" className="flex items-center gap-1 text-xs font-normal opacity-60">
                            <CheckIcon className="h-3.5 w-3.5"/> Read
                          </Typography>
                        }
                      </div>
                    </ListItem>
                  )
                }
                )
              }
            </MenuList>
          </Menu>
        </div>
      </div>
    </Navbar>
  );
}

DashboardNavbar.displayName = "/src/widgets/layout/DashboardNavbar.jsx";

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

import {
  HomeIcon,
  UserCircleIcon,
  TableCellsIcon,
  InformationCircleIcon,
  ServerStackIcon,
  RectangleStackIcon,
} from "@heroicons/react/24/solid";
import { Home, Profile } from "./pages/dashboard/index";
import { SignIn, SignUp } from "@/pages/auth";
import ProjectOverview from "./pages/projectOverview/ProjectOverview";
import ResetPassword from "./pages/auth/ResetPassword";
import VerifyResetCode from "./pages/auth/VerifyResetCode";
import SendResetCode from "./pages/auth/SendResetCode";

const icon = {
  className: "w-5 h-5 text-inherit",
};

export const routes = [
  {
    layout: "dashboard",
    pages: [
      {
        icon: <HomeIcon {...icon} />,
        name: "dashboard",
        path: "/home",
        element: <Home />,
      },
      {
        icon: <UserCircleIcon {...icon} />,
        name: "profile",
        path: "/profile/:userId?",
        dynamic: true,
        element: <Profile />,
      },
      {
        name: "project",
        path: "/project/:projectId", 
        dynamic: true, 
        element: <ProjectOverview />,
      },
    ],
  },
  {
    title: "auth pages",
    layout: "auth",
    pages: [
      {
        icon: <ServerStackIcon {...icon} />,
        name: "sign in",
        path: "/sign-in",
        element: <SignIn />,
      },
      {
        icon: <RectangleStackIcon {...icon} />,
        name: "sign up",
        path: "/sign-up",
        element: <SignUp />,
      },
      {
        icon: <InformationCircleIcon {...icon} />,
        name: "send reset code",
        path: "/send-reset-code",
        element: <SendResetCode />,
      },
      {
        icon: <TableCellsIcon {...icon} />,
        name: "verify reset",
        path: "/verify-reset",
        element: <VerifyResetCode />,
      },
      {
        icon: <TableCellsIcon {...icon} />,
        name: "reset password",
        path: "/reset-password",
        element: <ResetPassword />,
      }
    ],
  },
];

export default routes;

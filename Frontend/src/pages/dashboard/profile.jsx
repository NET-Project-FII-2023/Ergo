import {
  Card,
  CardBody,
  Avatar,
  Typography,
  Tooltip, Button,
} from "@material-tailwind/react";
import {
  PencilIcon,
} from "@heroicons/react/24/solid";
import { ProfileInfoCard } from "@/widgets/cards";
import {useUser} from "@/context/LoginRequired.jsx";
import {Link} from "react-router-dom";

export function Profile() {
    const user = useUser();

  return (
    <>
      <div className="relative mt-8 h-72 w-full overflow-hidden rounded-xl bg-[url('/img/background-image.png')] bg-cover	bg-center">
        <div className="absolute inset-0 h-full w-full bg-gray-900/75" />
      </div>
      <Card className="mx-3 -mt-16 mb-6 lg:mx-4 border border-blue-gray-100">
        <CardBody className="p-4">
          <div className="mb-10 flex items-center justify-between flex-wrap gap-6">
            <div className="flex items-center gap-6">
              <Avatar
                src="/img/bruce-mars.jpeg"
                alt="bruce-mars"
                size="xl"
                variant="rounded"
                className="rounded-lg shadow-lg shadow-blue-gray-500/40"
              />
              <div>
                <Typography variant="h5" color="blue-gray" className="mb-1">
                    {user?.name || "John Doe"}
                </Typography>
                <Typography
                  variant="small"
                  className="font-normal text-blue-gray-600"
                >
                    {user?.role || "Unknown role"}
                </Typography>
              </div>
            </div>
          </div>
          <div className="gird-cols-1 mb-12 grid gap-12 px-4 lg:grid-cols-2 xl:grid-cols-3">
            <ProfileInfoCard
              title="Profile Information"
              description="Hi, I'm Alec Thompson, Decisions: If you can't decide, the answer is no. If two equally difficult paths, choose the one more painful in the short term (pain avoidance is creating an illusion of equality)."
              details={{
                mobile: "(44) 123 1234 123",
                email: user?.email || "",
                // location: "USA",
                // social: (
                //   <div className="flex items-center gap-4">
                //     <i className="fa-brands fa-facebook text-blue-700" />
                //     <i className="fa-brands fa-twitter text-blue-400" />
                //     <i className="fa-brands fa-instagram text-purple-500" />
                //   </div>
                // ),
              }}
              action={
                <Tooltip content="Edit Profile">
                  <PencilIcon className="h-4 w-4 cursor-pointer text-blue-gray-500" />
                </Tooltip>
              }
            />
          </div>
            <div className="flex items-center justify-between flex-wrap gap-6">
              <Link to="/auth/sign-in" className="ml-auto">
                <Button
                    color="red"
                    className="shadow-md"
                    ripple
                >Log out</Button>
              </Link>
            </div>
        </CardBody>
      </Card>
    </>
  );
}

export default Profile;

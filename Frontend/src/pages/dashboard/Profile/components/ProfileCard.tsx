import {Avatar, Input, Typography} from "@material-tailwind/react";
import ProfileInfoCard from "./ProfileInfoCard";
import {useState} from "react";
import api from "../../../../services/api";
import {useUser} from "../../../../context/LoginRequired";
import {toast} from "react-toastify";
import {ProfileCardProps, PutUserResponseType, UserDataType} from "./types";

export function ProfileCard({userData, setUserData, isEditable = false}: ProfileCardProps) {
  const currentUser = useUser();
  const [isInEditMode, setIsInEditMode] = useState(false);
  const [editedUserData, setEditedUserData] = useState<UserDataType>(userData);

  const onSaveEdit = async () => {
    setIsInEditMode(false);
    try {
      const response = await api.put<PutUserResponseType>(`/api/v1/Users/${currentUser.userId}`, {
        id: currentUser.userId,
        name: editedUserData.name,
        email: editedUserData.email,
        bio: editedUserData.bio,
        mobile: editedUserData.mobile,
        company: editedUserData.company,
        location: editedUserData.location,
        social: editedUserData.social && {
          facebook: editedUserData.social.facebook,
          instagram: editedUserData.social.instagram,
          twitterX: editedUserData.social.twitterX,
          linkedIn: editedUserData.social.linkedIn,
          gitHub: editedUserData.social.gitHub,
        },
      }, {
        headers: {
          Authorization: `Bearer ${currentUser.token}`
        }
      });

      if (response.status === 200) {
        setUserData({
          name: response.data.user?.name || "John Doe",
          username: response.data.user?.username || "Unknown username",
          email: response.data.user?.email,
          bio: response.data.user?.bio,
          mobile: response.data.user?.mobile,
          company: response.data.user?.company,
          location: response.data.user?.location,
          social: response.data.user?.social || {},
          roles: userData.roles,
        })
        toast.success("User data updated successfully");
      }

    } catch (error) {
      console.error(error);
      toast.error("Failed to update user data")
    }
  }

  const onCancelEdit = () => {
    setIsInEditMode(false);
    setEditedUserData(userData);
  }

  return <>
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
          {!isInEditMode ? (
            <Typography variant="h5" className="mb-1 text-surface-light">
              {userData.name}
            </Typography>
          ) : (
            <Input
              value={editedUserData.name}
              onChange={(e) => setEditedUserData({...editedUserData, name: e.target.value})}
              variant={"outlined"}
              label={"Name"}
              size={"md"}
              color={"deep-purple"}
              className={"text-white"}
              crossOrigin={undefined}
            />
          )}
          <Typography
            variant="small"
            className="font-normal text-surface-mid-light"
          >
            {"#" + userData?.username || "Unknown username"}
          </Typography>
        </div>
      </div>
    </div>
    <div
      className="gird-cols-1 mb-12 grid gap-12 px-4 lg:grid-cols-2 xl:grid-cols-3 text-surface-darkest">
      <ProfileInfoCard
        bio={userData?.bio}
        details={{
          mobile: userData?.mobile,
          email: userData?.email,
          social: userData?.social,
          company: userData?.company,
          location: userData?.location,
        }}
        isEditable={isEditable}
        isInEditMode={isInEditMode}
        onEnterEditMode={() => setIsInEditMode(true)}
        editedUserData={editedUserData}
        setEditedUserData={setEditedUserData}
        onSaveEdit={onSaveEdit}
        onCancelEdit={onCancelEdit}
      />
    </div>
  </>
}
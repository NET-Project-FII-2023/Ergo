import {Input, Typography} from "@material-tailwind/react";
import ProfileInfoCard from "./ProfileInfoCard";
import {useEffect, useState} from "react";
import api from "../../../../services/api";
import {useUser} from "../../../../context/LoginRequired";
import {toast} from "react-toastify";
import {ProfileCardProps, PutUserResponseType, SetUserPhotoResponseType, UserDataType} from "./types";
import ProfileUserAvatar from "./ProfileUserAvatar";
import Badges from "./Badges";
import { useParams } from "react-router-dom";

const isFormValid = (editedUserData: UserDataType) => {
  const emailRegex = /^[\w-\.]+@([\w-]+\.)+[\w-]{2,4}$/i;
  const urlRegex = /^https?:\/\/(www\.)?[-a-zA-Z0-9@:%._\+~#=]{1,256}\.[a-zA-Z0-9()]{1,6}\b([-a-zA-Z0-9()@:%_\+.~#?&//=]*)$/i;

  if (!editedUserData.name || editedUserData.name.replaceAll(" ", "").length === 0) {
    toast.error("Name is required");
    return false;
  }

  if (!editedUserData.email || !emailRegex.test(editedUserData.email)) {
    toast.error("Invalid email");
    return false;
  }

  if(editedUserData.social?.facebook && !urlRegex.test(editedUserData.social.facebook)) {
    toast.error("Invalid Facebook URL");
    return false;
  }

  if(editedUserData.social?.instagram && !urlRegex.test(editedUserData.social.instagram)) {
    toast.error("Invalid Instagram URL");
    return false;
  }

  if(editedUserData.social?.twitterX && !urlRegex.test(editedUserData.social.twitterX)) {
    toast.error("Invalid X URL");
    return false;
  }

  if(editedUserData.social?.linkedIn && !urlRegex.test(editedUserData.social.linkedIn)) {
    toast.error("Invalid LinkedIn URL");
    return false;
  }

  if(editedUserData.social?.gitHub && !urlRegex.test(editedUserData.social.gitHub)) {
    toast.error("Invalid GitHub URL");
    return false;
  }

  return true;
}

export function ProfileCard({userData, setUserData, isEditable = false}: ProfileCardProps) {
  const currentUser = useUser();
  const {userId} = useParams();
  const isOwnProfile = userId === currentUser.userId || !userId;
  const [isInEditMode, setIsInEditMode] = useState(false);
  const [editedUserData, setEditedUserData] = useState<UserDataType>(userData);
  const [editedUserPhoto, setEditedUserPhoto] = useState<File | null>(null);

  useEffect(() => {
    setEditedUserData(userData);
  }, [userData]);

  const onSaveEdit = async () => {
    if(!isFormValid(editedUserData)) {
      return;
    }

    setIsInEditMode(false);
    if(editedUserPhoto) {
      const formData = new FormData();
      formData.append('File', editedUserPhoto);

      try {
        let response;
        if (userData.userPhoto?.userPhotoId && userData.userPhoto?.photoUrl) {
          response = await api.put<SetUserPhotoResponseType>(`/api/v1/Cloud/update-user-photo?UserPhotoId=${userData.userPhoto.userPhotoId}&CloudUrl=${userData.userPhoto.photoUrl}`, formData, {
            headers: {
              Authorization: `Bearer ${currentUser.token}`,
              'Content-Type': undefined
            }
          });
        } else {
          response = await api.post<SetUserPhotoResponseType>(`/api/v1/Cloud/upload-user-photo?UserId=${currentUser.userId}`, formData, {
            headers: {
              Authorization: `Bearer ${currentUser.token}`,
              'Content-Type': undefined
            }
          });
        }

        if (response.status === 200) {
          setUserData({
            ...userData,
            userPhoto: {
              userPhotoId: response.data.userPhoto?.userPhotoId,
              photoUrl: response.data.userPhoto?.photoUrl
            }
          })
        }
      } catch (err) {
        console.error(err);
        toast.error("Failed to upload user photo");
      }

      setEditedUserPhoto(null)
    }

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
          userPhoto: response.data.user?.userPhoto,
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
    setEditedUserPhoto(null)
  }

  return <>
    <div className="mb-10 flex items-center justify-between flex-wrap gap-6">
      <div className="flex items-center gap-6">
        <ProfileUserAvatar
          photoUrl={userData?.userPhoto?.photoUrl}
          editedUserPhoto={editedUserPhoto}
          setEditedUserPhoto={setEditedUserPhoto}
          isInEditMode={isInEditMode}
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
            {"@" + userData?.username || "Unknown username"}
          </Typography>
        </div>
      </div>
    </div>
    <div className="grid-cols-1 mb-12 grid gap-12 px-4 lg:grid-cols-2 xl:grid-cols-3 text-surface-darkest">
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
      {userData && 
        <div className="col-span-1 xl:col-span-2 lg:border-l border-surface-mid">
          <Typography variant="h5" className="mb-4 text-surface-light ml-6">
            Badges
          </Typography>
          <Badges currentViewedId={currentUser.userId} isOwnProfile={isOwnProfile} />
        </div>
      }
    </div>
  </>
}
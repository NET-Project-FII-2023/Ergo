import {Avatar, Spinner} from "@material-tailwind/react";
import {useEffect, useRef, useState} from "react";
import {s3} from "../../../../services/api";
import {toast} from "react-toastify";
import {UserAvatarProps} from "./types";

export default function ProfileUserAvatar({photoUrl, editedUserPhoto, setEditedUserPhoto, isInEditMode = false}: UserAvatarProps) {
  const [userPhoto, setUserPhoto] = useState<string | null>(null);
  const fileInputRef = useRef<HTMLInputElement>(null);

  useEffect(() => {
    setUserPhoto(null)

    if(photoUrl) {
      s3.getObject({
        Bucket: 'ergo-project',
        Key: photoUrl
      }, function (err, data) {
        if (err) {
          console.error("Error", err);
          toast.error("Failed to fetch user photo")
        } else if(data.Body) {
          setUserPhoto(URL.createObjectURL(new Blob([data.Body])));
        }
      })
    }
  }, [photoUrl]);

  if (isInEditMode) {
    return (
      <div className="relative">
        <input
          type="file"
          accept="image/*"
          onChange={p => setEditedUserPhoto(p.target.files?.[0] || null)}
          ref={fileInputRef}
          style={{display: 'none'}}
        />
        {!editedUserPhoto ? (
          <div
            className="grid h-[4.625rem] w-[4.625rem] place-items-center rounded-lg bg-surface-light cursor-pointer"
            onClick={() => fileInputRef.current?.click()}
          >
            <i className={"fa-regular fa-plus text-surface-darkest"}/>
          </div>
        ) : (
          <Avatar
            src={URL.createObjectURL(editedUserPhoto)}
            alt="User photo"
            size="xl"
            variant="rounded"
            className="rounded-lg cursor-pointer"
            onClick={() => fileInputRef.current?.click()}
          />
        )}
      </div>
    )
  }

  if (photoUrl && !userPhoto) {
    return (
      <div className="grid h-[4.625rem] w-[4.625rem] place-items-center rounded-lg bg-surface-light">
        <Spinner color={"deep-purple"} />
      </div>
    )
  }

  return (
    <Avatar
      src={userPhoto || "/img/bruce-mars.jpeg"}
      alt="bruce-mars"
      size="xl"
      variant="rounded"
      className="rounded-lg shadow-md shadow-blue-gray-500/30"
    />
  )
}
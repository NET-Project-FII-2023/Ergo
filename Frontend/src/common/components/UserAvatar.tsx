import {Avatar , Spinner} from "@material-tailwind/react";
import {useEffect, useRef, useState} from "react";
import {s3} from "../../services/api";
import {toast} from "react-toastify";
import {AvatarProps} from "@material-tailwind/react/components/Avatar";
import {SpinnerProps} from "@material-tailwind/react/components/Spinner";

type ProfileUserAvatar = {
  photoUrl: string | undefined,
  loadingClassName?: string,
  loadingProps?: Omit<SpinnerProps, 'ref'>,
} & Omit<AvatarProps, 'ref'>;

export default function UserAvatar({photoUrl, loadingClassName, loadingProps, ...props}: ProfileUserAvatar) {
  const [userPhoto, setUserPhoto] = useState<string | null>(null);
  const [hasErrors, setHasErrors] = useState(false);

  useEffect(() => {
    setUserPhoto(null)

    if(photoUrl) {
      s3.getObject({
        Bucket: 'ergo-project',
        Key: photoUrl
      }, function (err, data) {
        if (err) {
          console.error("Error", err);
          setHasErrors(true);
        } else if(data.Body) {
          setUserPhoto(URL.createObjectURL(new Blob([data.Body])));
        }
      })
    }
  }, [photoUrl]);

  if (photoUrl && !userPhoto && !hasErrors) {
    return (
      <div className={`flex items-center justify-center rounded-full ${loadingClassName || ""}`}>
        <Spinner color={"deep-purple"} {...loadingProps}/>
      </div>
    )
  }

  return (
    <Avatar
      src={userPhoto || "/img/bruce-mars.jpeg"}
      alt="bruce-mars"
      size="xl"
      variant="rounded"
      className="rounded-lg"
      {...props}
    />
  )
}
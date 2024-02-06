import {Input, Textarea, Tooltip, Typography} from "@material-tailwind/react";
import {BioTextFieldProps, DataFieldProps, SocialFieldProps} from "./types";
import {Link} from "react-router-dom";

export function DataField({label, value, isInEditMode, editedValue, onChangeEditedValue}: DataFieldProps) {
  return (
    <li className="flex items-center gap-4">
      {!isInEditMode ? value && (
        <>
          <Typography
            variant="small"
            className="font-semibold capitalize text-surface-light"
          >
            {label}:
          </Typography>
          <Typography
            variant="small"
            className="font-normal text-surface-mid-light"
          >
            {value}
          </Typography>
        </>
      ) : (
        <Input
          value={editedValue}
          onChange={(e) => onChangeEditedValue(e.target.value)}
          type={"text"}
          variant={"outlined"}
          label={label}
          size={"md"}
          color={"deep-purple"}
          className={"text-white"}
          crossOrigin={undefined}
        />
      )}
    </li>
  )
}

export function SocialField ({social, onChangeSocial, isInEditMode}: SocialFieldProps) {
  if(isInEditMode) {
    return (
      <>
        <li className="flex items-center gap-4">
          <Input
            value={social?.facebook}
            onChange={(e) => onChangeSocial({...social, facebook: e.target.value})}
            label={"Facebook"}
            color={"deep-purple"}
            className={"text-white"}
            icon={<i className="fa-brands fa-facebook text-blue-700"/>}
            crossOrigin={undefined}
          />
        </li>
        <li className="flex items-center gap-4">
          <Input
            value={social?.instagram}
            onChange={(e) => onChangeSocial({...social, instagram: e.target.value})}
            label={"Instagram"}
            color={"deep-purple"}
            className={"text-white"}
            icon={<i className="fa-brands fa-instagram text-purple-500"/>}
            crossOrigin={undefined}
          />
        </li>
        <li className="flex items-center gap-4">
          <Input
            value={social?.twitterX}
            onChange={(e) => onChangeSocial({...social, twitterX: e.target.value})}
            label={"X"}
            color={"deep-purple"}
            className={"text-white"}
            icon={<i className="fa-brands fa-x-twitter text-gray-50 "/>}
            crossOrigin={undefined}
          />
        </li>
        <li className="flex items-center gap-4">
          <Input
            value={social?.linkedIn}
            onChange={(e) => onChangeSocial({...social, linkedIn: e.target.value})}
            label={"LinkedIn"}
            color={"deep-purple"}
            className={"text-white"}
            icon={<i className="fa-brands fa-linkedin text-blue-700"/>}
            crossOrigin={undefined}
          />
        </li>
        <li>
          <Input
            value={social?.gitHub}
            onChange={(e) => onChangeSocial({...social, gitHub: e.target.value})}
            label={"GitHub"}
            color={"deep-purple"}
            className={"text-white"}
            icon={<i className="fa-brands fa-github text-gray-50 "/>}
            crossOrigin={undefined}
          />
        </li>
      </>
    )
  }

  return (
    <div className={"flex gap-4"}>
      {social?.facebook && (
        <Tooltip content="Facebook">
          <Link to={social?.facebook}>
            <i className="fa-brands fa-facebook text-blue-700"/>
          </Link>
        </Tooltip>
      )}
      {social?.instagram && (
        <Tooltip content="Instagram">
          <Link to={social?.instagram}>
            <i className="fa-brands fa-instagram text-purple-500"/>
          </Link>
        </Tooltip>
      )}
      {social?.twitterX && (
        <Tooltip content="X">
          <Link to={social?.twitterX}>
            <i className="fa-brands fa-x-twitter text-gray-50 "/>
          </Link>
        </Tooltip>
      )}
      {social?.linkedIn && (
        <Tooltip content="LinkedIn">
          <Link to={social?.linkedIn}>
            <i className="fa-brands fa-linkedin text-blue-700"/>
          </Link>
        </Tooltip>
      )}
      {social?.gitHub && (
        <Tooltip content="GitHub">
          <Link to={social?.gitHub}>
            <i className="fa-brands fa-github text-gray-50"/>
          </Link>
        </Tooltip>
      )}
    </div>
  )
}

export function BioTextField({bio, editedBio, onChange, isInEditMode}: BioTextFieldProps) {
  if (isInEditMode) {
    return (
      <Textarea
        value={editedBio}
        onChange={(e) => onChange(e.target.value)}
        label={"Bio"}
        color={'deep-purple'}
        containerProps={{ className: "mt-2" }}
        className={"text-white"}
      />
    )
  }

  if (bio) {
    return (
      <Typography
        variant="small"
        className="font-normal text-surface-light mt-4"
      >
        {bio}
      </Typography>
    )
  }
}

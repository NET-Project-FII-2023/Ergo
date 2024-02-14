import {Button, Card, CardBody, CardHeader, Tooltip, Typography,} from "@material-tailwind/react";
import {PencilIcon} from "@heroicons/react/24/solid";
import {EditActionButtonsProps, ProfileInfoCardProps} from "./types";
import {BioTextField, DataField, SocialField} from "./ProfileInfoCardFields";

function EditActionButtons({onSaveEdit, onCancelEdit}: EditActionButtonsProps) {
  return (
    <div className="flex items-center gap-2 mt-2">
      <Button
        onClick={onSaveEdit}
        className={"bg-green-600 hover:bg-green-900"}
        size={"sm"}
      >
        Save
      </Button>
      <Button
        onClick={onCancelEdit}
        className={"bg-surface-mid-light hover:bg-surface-mid"}
        size={"sm"}
      >
        Cancel
      </Button>
    </div>
  )
}

export function ProfileInfoCard({
                                  bio,
                                  details = {},
                                  isEditable,
                                  isInEditMode,
                                  onEnterEditMode,
                                  editedUserData,
                                  setEditedUserData,
                                  onSaveEdit,
                                  onCancelEdit
                                }: ProfileInfoCardProps) {

  return (
    <Card color="transparent" shadow={false}>
      <CardHeader
        color="transparent"
        shadow={false}
        floated={false}
        className="mx-0 mt-0 flex items-center justify-between gap-4"
      >
        <Typography variant="h5" className="text-surface-light">
          Profile Information
        </Typography>
        {isEditable && !isInEditMode && (
          <Tooltip content="Edit Profile">
            <PencilIcon
              onClick={onEnterEditMode}
              className="h-4 w-4 cursor-pointer text-surface-light"
            />
          </Tooltip>
        )}
      </CardHeader>
      <CardBody className="p-0">
        <BioTextField
          bio={bio}
          editedBio={editedUserData.bio}
          onChange={(newBio) => setEditedUserData({...editedUserData, bio: newBio})}
          isInEditMode={isInEditMode}
        />
        <hr className="my-2 border-surface-mid"/>
        <ul className="flex flex-col gap-4 p-0 mt-4">
          <DataField
            label={"Email"}
            value={details.email}
            isInEditMode={isInEditMode}
            editedValue={editedUserData.email}
            onChangeEditedValue={(value) => setEditedUserData({...editedUserData, email: value})}
          />
          <DataField
            label={"Mobile"}
            value={details.mobile}
            isInEditMode={isInEditMode}
            editedValue={editedUserData.mobile}
            onChangeEditedValue={(value) => setEditedUserData({...editedUserData, mobile: value})}
          />
          <DataField
            label={"Company"}
            value={details.company}
            isInEditMode={isInEditMode}
            editedValue={editedUserData.company}
            onChangeEditedValue={(value) => setEditedUserData({...editedUserData, company: value})}
          />
          <DataField
            label={"Location"}
            value={details.location}
            isInEditMode={isInEditMode}
            editedValue={editedUserData.location}
            onChangeEditedValue={(value) => setEditedUserData({...editedUserData, location: value})}
          />
          <SocialField
            isInEditMode={isInEditMode}
            social={editedUserData.social}
            onChangeSocial={(value) => setEditedUserData({...editedUserData, social: value})}
          />

          {isInEditMode && <EditActionButtons onSaveEdit={onSaveEdit} onCancelEdit={onCancelEdit}/>}
        </ul>
      </CardBody>
    </Card>
  );
}

export default ProfileInfoCard;

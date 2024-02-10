type SocialType = {
  facebook?: string,
  instagram?: string,
  twitterX?: string,
  linkedIn?: string,
  gitHub?: string,
}

type UserPhotoType = {
  userPhotoId?: string,
  photoUrl?: string,
} | null;

type BaseResponseType = {
  success: boolean,
  message: string | null,
  validationErrors: string[] | null,
}

export type UserDataType = {
  name: string,
  username: string,
  userPhoto?: UserPhotoType,
  bio?: string,
  email?: string,
  mobile?: string,
  company?: string,
  location?: string,
  social?: SocialType,
  roles: string[],
};

export type GetUserByIdResponseType = {
  user?: {
    userId?: string,
    username?: string,
    name?: string,
    userPhoto?: UserPhotoType,
    email?: string,
    bio?: string,
    mobile?: string,
    company?: string,
    location?: string,
    social?: SocialType,
    roles?: string[],
  }
} & BaseResponseType;

export type PutUserResponseType = {
  user?: {
    userId?: string,
    username?: string,
    userPhoto?: UserPhotoType,
    name?: string,
    email?: string,
    bio?: string,
    mobile?: string,
    company?: string,
    location?: string,
    social?: SocialType,
  }
} & BaseResponseType;

export type SetUserPhotoResponseType = {
  userPhoto?: UserPhotoType,
} & BaseResponseType;

export type ProfileCardProps = {
  userData: UserDataType,
  setUserData: (userData: UserDataType) => void,
  isEditable?: boolean,
};

export type ProfileInfoCardProps = {
  bio?: string,
  details: {
    email?: string,
    mobile?: string,
    company?: string,
    location?: string,
    social?: SocialType,
  },
  isEditable: boolean,
  isInEditMode: boolean,
  onEnterEditMode: () => void,
  editedUserData: UserDataType,
  setEditedUserData: (data: UserDataType) => void,
  onSaveEdit: () => void,
  onCancelEdit: () => void,
};

export type EditActionButtonsProps = {
  onSaveEdit: () => void,
  onCancelEdit: () => void
}

export type DataFieldProps = {
  label: string,
  value?: string,
  isInEditMode: boolean,
  editedValue?: string,
  onChangeEditedValue: (value: string) => void,
}

export type SocialFieldProps = {
  social?: SocialType,
  onChangeSocial: (val: SocialType) => void,
  isInEditMode: boolean,
}

export type BioTextFieldProps = {
  bio?: string,
  editedBio?: string,
  onChange: (val: string) => void,
  isInEditMode: boolean,
}

export type UserAvatarProps = {
  photoUrl?: string,
  editedUserPhoto: File | null,
  setEditedUserPhoto: (file: File | null) => void,
  isInEditMode: boolean,
}
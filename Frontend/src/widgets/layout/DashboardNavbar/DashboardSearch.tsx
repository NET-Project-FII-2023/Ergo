import {Input, Menu, MenuHandler, MenuItem, MenuList} from "@material-tailwind/react";
import {useEffect, useRef, useState} from "react";
import {debounce} from "@mui/material";
import api from "../../../services/api";
import {useUser} from "../../../context/LoginRequired";
import {toast} from "react-toastify";
import {useNavigate} from "react-router-dom";
import UserAvatar from "../../../common/components/UserAvatar";

type UserPhotoType = {
  userPhotoId?: string,
  photoUrl?: string,
} | null;

type UserType = {
  userId: string,
  username: string,
  name: string,
  email: string,
  userPhoto?: UserPhotoType,
}

type SearchUserResponseType = {
  users?: UserType[],
  message?: string,
  success?: boolean,
  validationErrors?: string[]
}

export default function UserSearch() {
  const searchRef = useRef<HTMLDivElement>(null);
  const inputRef = useRef<HTMLInputElement>(null);
  const navigate = useNavigate();
  const {token} = useUser();
  const [foundUsers, setFoundUsers] = useState<UserType[]>([]);
  const [searchValue, setSearchValue] = useState("")
  const [isMenuOpen, setIsMenuOpen] = useState(false);

  useEffect(() => {
    const searchUsers = async () => {
      try {
        if (searchValue) {
          const response = await api.get<SearchUserResponseType>(`/api/v1/Users/search/${searchValue}`, {
            headers: {
              Authorization: `Bearer ${token}`
            }
          });

          if (response.status === 200) {
            setFoundUsers(response.data.users || []);
            setIsMenuOpen(true)
          }
        } else {
          setFoundUsers([])
          setIsMenuOpen(false)
        }
      } catch (error) {
        console.error(error)
        toast.error("Failed to search users")
        setIsMenuOpen(false)
      }
    }

    const debouncedSearch = debounce(() => {
      searchUsers();
    }, 500);

    debouncedSearch();

    return () => {
      debouncedSearch.clear();
    };
  }, [searchValue])

  useEffect(() => {
    document.addEventListener('click', handleClickOutside, true);
    return () => {
      document.removeEventListener('click', handleClickOutside, true);
    };
  }, []);

  const handleMenuItemClick = (userId: string) => {
    setIsMenuOpen(false);
    setSearchValue("");
    navigate(`/dashboard/profile/${userId}`);
  }

  const handleClickOutside = (event: MouseEvent) => {
    if (searchRef.current && !searchRef.current.contains(event.target as Node)) {
      setIsMenuOpen(false);
    }
  };

  return (
    <div className="relative" ref={searchRef}>
      <Input
        value={searchValue}
        onChange={(e) => setSearchValue(e.target.value)}
        onFocus={() => searchValue && setIsMenuOpen(true)}
        label="Search"
        color="deep-purple"
        crossOrigin={undefined}
        className="text-white w-56"
        icon={<i className="fa-solid fa-search"/>}
        inputRef={inputRef}
      />
      <Menu
        placement="bottom-start"
        open={isMenuOpen}
      >
        <MenuHandler>
          {/*Needed for positioning*/}
          <div></div>
        </MenuHandler>
        <MenuList
          className="max-h-[20rem] w-56 bg-surface-dark border-surface-darkest"
          onFocus={() => inputRef?.current?.focus()}
        >
          {foundUsers.length ? (
            foundUsers.map((user) => (
              <MenuItem
                key={user.userId}
                className="hover:bg-surface-mid"
                onClick={() => handleMenuItemClick(user.userId)}
              >
                <div className={"flex items-center"}>
                  <UserAvatar
                    photoUrl={user.userPhoto?.photoUrl}
                    className="h-9 w-9 rounded-full object-cover"
                    loadingClassName="h-9 w-9 bg-surface-mid-dark"
                    loadingProps={{className: "h-4 w-4"}}
                  />
                  <div className={'ml-2'}>
                    <p className={"text-white text-base"}>{user.name}</p>
                    <p className={"text-surface-light text-xs"}>#{user.username}</p>
                  </div>
                </div>
              </MenuItem>
            ))
          ) : (
            <MenuItem className="text-surface-light" disabled>
              No users found
            </MenuItem>
          )}
        </MenuList>
      </Menu>
    </div>
  );
}
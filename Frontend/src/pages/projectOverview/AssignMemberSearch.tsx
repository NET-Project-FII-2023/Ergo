import {useEffect, useRef, useState} from "react";
import {useUser} from "../../context/LoginRequired";
import api from "../../services/api";
import {toast} from "react-toastify";
import {debounce} from "@mui/material";
import {Menu, MenuHandler, MenuItem, MenuList} from "@material-tailwind/react";
import UserAvatar from "../../common/components/UserAvatar";
import ErgoInput from "../../widgets/form_utils/ErgoInput";

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

type AssignMemberSearchProps = {
  onUserSelect: (user: UserType) => void,
  assignedMembers: UserType[]
}

export default function AssignMemberSearch({onUserSelect, assignedMembers}: AssignMemberSearchProps) {
  const searchRef = useRef<HTMLDivElement>(null);
  const inputRef = useRef<HTMLInputElement>(null);
  const menuRef = useRef<HTMLUListElement>(null);
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
            const filteredUsers = response.data.users?.filter(u => {
              return !assignedMembers.some(m => m.userId === u.userId)
            }) || [];
            setFoundUsers(filteredUsers);
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

  useEffect(() => {
    const adjustMenuWidth = () => {
      const inputWidth = inputRef.current?.offsetWidth;
      if (menuRef.current && inputWidth) {
        menuRef.current.style.width = `${inputWidth}px`;
      }
    };

    adjustMenuWidth();
    window.addEventListener('resize', adjustMenuWidth);

    return () => window.removeEventListener('resize', adjustMenuWidth);
  }, [inputRef, menuRef, isMenuOpen]);

  const handleMenuItemClick = (user: UserType) => {
    setIsMenuOpen(false);
    setSearchValue("");
    onUserSelect(user);
  }

  const handleClickOutside = (event: MouseEvent) => {
    if (searchRef.current && !searchRef.current.contains(event.target as Node)) {
      setIsMenuOpen(false);
    }
  };

  return (
    <div className="relative" ref={searchRef}>
      <ErgoInput
        value={searchValue}
        onChange={(e) => setSearchValue(e)}
        onFocus={() => searchValue && setIsMenuOpen(true)}
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
          className="max-h-[20rem] bg-surface-dark border-surface-darkest"
          ref={menuRef}
          onFocus={() => inputRef?.current?.focus()}
        >
          {foundUsers.length ? (
            foundUsers.map((user) => (
              <MenuItem
                key={user.userId}
                className="hover:bg-surface-mid"
                onClick={() => handleMenuItemClick(user)}
              >
                <div className={"flex items-center justify-between"}>
                  <div className={"flex items-center"}>
                    <UserAvatar
                      photoUrl={user.userPhoto?.photoUrl}
                      className="h-9 w-9 rounded-full object-cover"
                      loadingClassName="h-9 w-9 bg-surface-mid-dark"
                      loadingProps={{className: "h-4 w-4"}}
                    />
                    <div className={'ml-2'}>
                      <p className={"text-white text-base"}>{user.name}</p>
                      <p className={"text-surface-light text-xs truncate"}>#{user.username}</p>
                    </div>
                  </div>
                  <i className={"fa-regular fa-plus text-white"}/>
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
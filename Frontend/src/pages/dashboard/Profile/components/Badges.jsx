import {useEffect, useState} from 'react'
import axios from "axios";
import {toast} from "react-toastify";
import { useUser } from '../../../../context/LoginRequired';
import api from '../../../../services/api';
import { Avatar } from '@material-tailwind/react';
const Badges = () => {
  const [userBadges, setUserBadges] = useState([])
  const user = useUser()
  const getUserBadges = async () => {
    try {
      const response = await api.get(`/api/v1/Badges/${user.userId}`,
        {
          headers: {
            Authorization: `Bearer ${user.token}`
          }
        })
        if(response.status === 200){
          setUserBadges(response.data.badges)
        }

    } catch (error) {
      if (axios.isAxiosError(error) && error.response) {
        if (error.response.status === 404 || error.response.status === 500) {
          navigate('/404');
        } else {
          toast.error("Failed to fetch user data");
        }
      } else {
        console.log(error);
        toast.error("An unexpected error occurred");
      }
    }
    }
  useEffect(() => {
    getUserBadges()
  }, [])
    

  return (
    <div className="w-[30rem] grid grid-rows-2 grid-flow-col p-4  rounded-lg shadow-xs dark:bg-gray-800">
      {
      userBadges.map((badge, index) => {
        return (
          <div key={index} className="w-[8rem] h-[8rem] flex flex-col items-center">
              <Avatar  className="rounded-full object-cover" src={badge.name} alt="badge" size="xl"/>
              <div className="flex flex-col items-center">
                <p className="text-sm font-semibold text-gray-400 dark:text-white">{badge.type}</p>
                <p className="text-center text-sm text-gray-200 dark:text-gray-300 border rounded-full w-8 h-5 bg-secondary ">x{badge.count}</p>
              </div>
            </div>

        )
      }
      )
    }
    </div>
  )
}

export default Badges;
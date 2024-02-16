import {useEffect, useState} from 'react'
import axios from "axios";
import { PlusIcon } from "@heroicons/react/24/solid";
import {toast} from "react-toastify";
import { useUser } from '../../../../context/LoginRequired';
import api from '../../../../services/api';
import { Avatar } from '@material-tailwind/react';
import badgesData from './badges-data';
import confetti from 'canvas-confetti';
import { useNavigate} from "react-router-dom";
import { sendNotification } from "@/services/notifications/sendNotification";

const Badges = ({currentViewedId,isOwnProfile}) => {
  const navigate = useNavigate();
  const [userBadges, setUserBadges] = useState([])
  const [userStats, setUserStats] = useState([])
  const [badgesUpdated, setBadgesUpdated] = useState(false);

  const user = useUser()

  const getUserBadges = async () => {
    try {
      const response = await api.get(`/api/v1/Badges/${currentViewedId}`,
        {
          headers: {
            Authorization: `Bearer ${user.token}`
          }
        })
        if(response.status === 200){
          setUserBadges(response.data.badges.sort((a, b) => a.type.localeCompare(b.type)))
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
  const getUserStats = async () => {
    try {
      console.log("current",currentViewedId)
      const response = await api.get(`api/v1/Users/stats/${currentViewedId}`,
        {
          headers: {
            Authorization: `Bearer ${user.token}`
          }
        })
        if(response.status === 200){
          setUserStats(response.data.userStats)
        }
      }catch (error) {
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
    const updateBadgeRequest = async (badge) => {
      try{
        const response = await api.post(`/api/v1/Badges`, badge,
        {
          headers: {
            Authorization: `Bearer ${user.token}`
          }
        })
        if(response.status === 201){
          console.log(response.data)
        }
      }catch (error) {
        if (axios.isAxiosError(error) && error.response) {
          if (error.response.status === 404 || error.response.status === 500) {
            navigate('/404');
          } else {
            console.log(error);
          }
        } else {
          console.log(error);
        }
      }
    }
    const updateBadges = async () => {  
      let requestBadgeData = {}
      for(let i = 0; i < badgesData.length; i++){    
        requestBadgeData.name = badgesData[i].name
        requestBadgeData.count = userStats[badgesData[i].countName]
        requestBadgeData.userId = currentViewedId
        requestBadgeData.type = badgesData[i].type
        await updateBadgeRequest(requestBadgeData);
      }
      setBadgesUpdated(true); 
    }
    const handleVote = async (badgeType) => {
      try{
        const response = await api.put(`/api/v1/Badges`, {voterId: user.userId,votedId: currentViewedId, type: badgeType},
        {
          headers: {
            Authorization: `Bearer ${user.token}`
          }
        })
        if(response.status === 200){
          getUserBadges();
          toast.success("Endorsment sent successfully!");
          confetti({
            particleCount: 200,
            spread: 100,
            origin: { y: 0.7 }
          });
          //send notification
          await sendNotification(currentViewedId, `You have received an endorsment from ${user.username}`, user.token);
        } 
      }catch (error) {
        let errorMessage = "";
        if (axios.isAxiosError(error) && error.response) {
          if (error.response.data) {
            errorMessage += error.response.data.validationsErrors[0];
          }
        } else if (error instanceof Error) {
          errorMessage += error.message;
        }
        toast.error(errorMessage);
      }
    }
    
  useEffect(() => {
    getUserBadges().then(getUserStats().then(updateBadges()))
  }, [currentViewedId])

  return (
    <div className="w-[30rem] grid grid-rows-2 grid-flow-col p-4 rounded-lg shadow-xs dark:bg-gray-800">
      {userBadges.map((badge, index) => {
        return (
          badge.active &&
          <div key={index} className="w-[8rem] h-[8rem] flex flex-col items-center">
            <Avatar className="rounded-full select-none object-cover" src={badge.name} alt="badge" size="xl"/>
            <div className="flex flex-col items-center">
              <p className="text-sm font-semibold text-gray-400 dark:text-white">{badge.type}</p>
              <div className="flex flex-row items-center gap-1">
                <p className="text-center text-sm text-surface-light px-2 rounded-sm bg-opacity-20 bg-secondary ">
                  {badge.count}
                </p>
                {(badge.type == "Innovator" || badge.type == "Quality-Master" || badge.type == "Team-Player" || badge.type == "Problem-Solver") && !isOwnProfile ? 
                  <PlusIcon title="Endorse Skill" className="text-surface-dark w-5 h-5 cursor-pointer px-0.5 rounded-full bg-secondary" onClick={() => handleVote(badge.type)}/> 
                  :
                  ""
                }
              </div>
            </div>
          </div>
        )
      })}
    </div>
  )
}

export default Badges;
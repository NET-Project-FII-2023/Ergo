import React, { useEffect, useState } from 'react';
import { Card, CardContent } from '@mui/material';
import api from "@/services/api";
import AssignMember from './AssignMember';
import { useUser } from '../../context/LoginRequired';
import DeleteMemberProject from './DeleteMemberProject';
import UserAvatar from "@/common/components/UserAvatar";
import { Tooltip } from "@material-tailwind/react";
import { useNavigate } from "react-router-dom";

const MembersList = ({ project, token }) => {
    const currentUser = useUser();
    const navigate = useNavigate();
    const [members, setMembers] = useState([]);

    useEffect(() => {
        fetchMembers();
    }, [project.projectId, token]);

    const fetchMembers = async () => {
        try {
            const response = await api.get(`/api/v1/Users/ByProjectId/${project.projectId}`, {
                headers: {
                    Authorization: `Bearer ${token}`,
                },
            });

            if (response.status === 200) {
                setMembers(response.data.users);
                console.log("users", response.data.users)
            } else {
                console.error('Error fetching members:', response);
            }
        } catch (error) {
            console.error('Error fetching members:', error);
        }
    };

    return (
      <div className='py-2'>
        <p className='text-surface-light text-lg my-2 px-2'>
          Assigned Members
        </p>
        {members.map(member => (
          <Card
            style={{backgroundColor: "#2f2b3a"}}
            key={member.userId}
            className="bg-surface-darkest opacity-80 mb-2 rounded"
          >
            <CardContent className='p-2 rounded bg-surface-dark'>
              <div className='flex'>
                <Tooltip content={member.name}>
                  <div
                    onClick={() => navigate(`/dashboard/profile/${member.userId}`)}
                    className={"cursor-pointer mr-3"}
                  >
                    <UserAvatar
                      // photoUrl={member.userPhoto.photoUrl}
                      className={"w-[2rem] h-[2rem] rounded-full"}
                      loadingClassName={"w-[2rem] h-[2rem] bg-surface-mid-dark rounded-full"}
                      loadingProps={{className: "w-5 h-5"}}
                    />
                  </div>
                </Tooltip>
                <div>
                  <p className="text-surface-light text-xs">
                    #{member.username}
                  </p>
                  <p className="text-surface-mid-light text-xs">
                    {member.email}
                  </p>
                </div>
                <DeleteMemberProject
                  project={project}
                  token={token}
                  currentUser={currentUser}
                  member={member}
                  fetchMembers={fetchMembers}/>
              </div>
            </CardContent>
          </Card>
        ))}

        {currentUser.username === project.createdBy &&
          <AssignMember
            project={project}
            token={token}
            onMemberAssigned={fetchMembers}
          />
        }
      </div>
    );
}

export default MembersList;

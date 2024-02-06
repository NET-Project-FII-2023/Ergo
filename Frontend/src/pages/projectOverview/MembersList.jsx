import React, { useEffect, useState } from 'react';
import { Card, CardContent, Typography } from '@mui/material';
import api from "@/services/api";

const MembersList = ({projectId, token}) => {
    const [members, setMembers] = useState([]);

    useEffect(() => {
        fetchMembers();
    }, [projectId, token]);

    const fetchMembers = async () => {
        try {
            const response = await api.get(`/api/v1/Users/ByProjectId/${projectId}`, {
                headers: {
                    Authorization: `Bearer ${token}`,
                },
            });

            if (response.status === 200) {
                setMembers(response.data.users);
            } else {
                console.error('Error fetching members:', response);
            }
        } catch (error) {
            console.error('Error fetching members:', error);
        }
    };

    return(
        <div className='py-2'>
            <p className='text-surface-light text-lg my-2 px-2'>
                Assigned Members
            </p>
            {members.map(member => (
                <Card
                style={{
                  backgroundColor: "#2f2b3a",
                }}
                key={member.userId} 
                className="bg-surface-darkest opacity-80 cursor-pointer mb-2 rounded"
                >
                    <CardContent className='p-2 rounded bg-surface-dark'>
                        <div className='flex'>
                            <span className='w-[2rem] h-[2rem] rounded-full bg-surface-light mr-3'>

                            </span>
                            <div>
                                <p className="text-surface-light text-xs">
                                    #{member.username}
                                </p>
                                <p className="text-surface-mid-light text-xs">
                                    {member.email}
                                </p>
                            </div>
                        </div>
                       
                    </CardContent>
                </Card>
            ))}
        </div>
    );
}

export default MembersList;

import {React, useEffect, useState} from 'react';
import GitHubIcon from '@mui/icons-material/GitHub';
import {Link} from '@mui/material';
import api from '@/services/api';
import { set } from 'date-fns';


const GithubSection = ({token, task, project}) => {
    const [branches, setBranches] = useState([]);
    const [commits, setCommits] = useState([]);

    const fetchBranches = async () => {
        try{
            const response = await api.post('/api/v1/GitHub/branches', {"projectId": project.projectId }, {
                headers: {
                    Authorization: `Bearer ${token}`,
                },
            })

            if(response.status = 200){
                setBranches(response.data);
                console.log("mydata", response.data);
            }
            else
            {
                console.error("Fail");
            }

        }catch(err){
            console.error(err);
        }
    }

    const fetchCommits = async () => {
        try{
            const response = await api.post(`/api/v1/GitHub/commits?Branch=filtering_data`, { "projectId": project.projectId},
            {
                headers:{
                    'Authorization': `Bearer ${token}`
                }
            })

            if(response.status == 200){
                console.log("commit result", response.data);
                setCommits(response.data);
            }
            else{
                console.err("Error")
            }
        }catch(err){ 
            console.error(err);
        }

    }

    useEffect(()=> {

        fetchBranches();
        fetchCommits();
    }, [])

    const modifyUrl = function(apiUrl){
        const newUrl =  apiUrl.replace("https://api.github.com/repos/", "https://github.com/").replace("git/commits", "commit");
        console.log("NEW URL:", newUrl);
        return newUrl;
    }

    return (
        <div className='mt-6'>    
            <div className='flex items-center mb-3'>
                <GitHubIcon fontSize='sm' className='text-secondary mr-1'></GitHubIcon>
                <p className='text-gray-300 ml-1 text-md font-semibold'>
                    Github Activity
                </p>
            </div>
            <div className="text-surface-light overflow-auto max-h-[16rem]" style={{ scrollbarWidth: 'thin', scrollbarColor: 'var(--color-surface-darkest) var(--color-surface)' }}>
                <ul>
                    {commits.map(item => (
                        <li key={item.commitName} className="relative pl-4 py-2 text-sm">
                            <a href={modifyUrl(item.url)} className='text-surface-light hover:underline' target="_blank">{item.commitName}</a>
                            <span className="absolute top-0 left-0 w-1 h-full bg-surface-mid"></span>
                        </li>
                    ))}
                </ul>
            </div>
        </div>
    );
};

export default GithubSection;

import { React, useEffect, useState } from 'react';
import GitHubIcon from '@mui/icons-material/GitHub';
import api from '@/services/api';

const GithubSection = ({ token, task, project }) => {
    const [commits, setCommits] = useState([]);

    const fetchCommits = async () => {
        try {
            if (!task.branch) {
                console.log("No associated branch");
                return;
            }

            const response = await api.post(`/api/v1/GitHub/commits?Branch=${task.branch}`, { "projectId": project.projectId }, {
                headers: {
                    'Authorization': `Bearer ${token}`
                }
            })

            if (response.status === 200) {
                console.log("commit result", response.data);
                setCommits(response.data);
            } else {
                console.error("Error")
            }
        } catch (err) {
            console.error(err);
        }
    }

    useEffect(() => {
        fetchCommits();
    }, [])

    const modifyUrl = function (apiUrl) {
        const newUrl = apiUrl.replace("https://api.github.com/repos/", "https://github.com/").replace("git/commits", "commit");
        return newUrl;
    }
    const formatTime = (deadline) => {
        const options = { day: 'numeric', month: 'long', year: 'numeric' };
        const formattedDeadline = new Date(deadline).toLocaleDateString(undefined, options);
        return formattedDeadline;
      };

    return (
        <div className='mt-6'>
            <div className='flex items-center mb-3'>
                <GitHubIcon fontSize='sm' className='text-secondary mr-1'></GitHubIcon>
                <p className='text-gray-300 ml-1 text-md font-semibold'>
                    Github Activity
                </p>
            </div>
            
            {task.branch ? (
                <>
                    <p className='text-surface-light font-sm mb-2'>Displaying commit activity for '{task.branch}':</p>
                    <div className="text-surface-light overflow-auto max-h-[16rem]" style={{ scrollbarWidth: 'thin' }}>
                        <ul>
                            {commits.map(item => (
                                <li key={item.commitName} className="relative pl-4 py-2 text-sm">
                                    <a href={modifyUrl(item.url)} className='text-surface-light hover:underline' target="_blank">{item.commitName}</a>
                                    <div>
                                        <span className='text-xs text-surface-mid-light font-semibold'>
                                            {item.author} -
                                        </span>
                                        <span className='text-xs text-surface-mid-light ml-1'>
                                            {formatTime(item.date)}
                                        </span>
                                    </div>
                                    <span className="absolute top-0 left-0 w-1 h-full bg-surface-mid"></span>
                                </li>
                            ))}
                        </ul>
                    </div>
                </>
            ) : (
                <p className="text-surface-light font-sm mb-2">No associated branch</p>
            )}
        </div>
    );
};

export default GithubSection;

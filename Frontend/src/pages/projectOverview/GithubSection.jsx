import { React, useEffect, useState } from 'react';
import GitHubIcon from '@mui/icons-material/GitHub';
import api from '@/services/api';
import { Select, Option, Button } from '@material-tailwind/react';
import ErgoLabel from '../../widgets/form_utils/ErgoLabel';
import MediationIcon from '@mui/icons-material/Mediation';
import { toast } from 'react-toastify';
import { useUser } from '../../context/LoginRequired';


const GithubSection = ({ token, task, project }) => {
    const [commits, setCommits] = useState([]);
    const [branches, setBranches] = useState([]);
    const [selectedBranch, setSelectedBranch] = useState('');
    const [isSelectVisible, setIsSelectVisible] = useState(false);
    const [displayText, setDisplayText] = useState('');
    const currentUser = useUser();

    const fetchBranches = async () => {
        try {
            const response = await api.post('/api/v1/GitHub/branches', { "projectId": project.projectId }, {
                headers: {
                    Authorization: `Bearer ${token}`,
                },
            })

            if (response.status === 200) {
                setBranches(response.data);
            }
            else {
                console.error("Fail");
            }
        } catch (err) {
            console.error(err);
        }
    }

    const fetchCommitsForBranch = async (branch) => {
        try {
            if (!branch) {
                console.log("No associated branch");
                return;
            }

            const response = await api.post(`/api/v1/GitHub/commits?Branch=${branch}`, { "projectId": project.projectId }, {
                headers: {
                    'Authorization': `Bearer ${token}`
                }
            })

            if (response.status === 200) {
                setCommits(response.data);
                setDisplayText(`Displaying commit activity for '${branch}':`);
            } else {
                console.error("Error")
            }
        } catch (err) {
            console.error(err);
        }
    };

    const handleOpenModal = () => {
        setIsSelectVisible(true);
        fetchCommitsForBranch(selectedBranch);
    };


    const handleUpdateTask = async () => {
        try {
            const response = await api.put(`/api/v1/TaskItems/${task.taskItemId}`, {
                taskItemId: task.taskItemId,
                taskName: task.taskName,
                description: task.description,
                deadline: task.deadline,
                createdBy: task.createdBy,
                projectId: task.projectId,
                state: task.state,
                branch: selectedBranch,
            }, {
                headers: {
                    Authorization: `Bearer ${token}`,
                },
            });

            if (response.status === 200) {
                toast.success('Task updated successfully');
                fetchCommitsForBranch(selectedBranch);
                // Hide the select dropdown after confirm is clicked
                setIsSelectVisible(false);
            } else {
                console.error('Error updating task:', response);
                toast.error('Failed to update task');
            }
        } catch (error) {
            console.error('Error updating task:', error);
            toast.error('Failed to update task');
        }
    };

    useEffect(() => {
        fetchBranches();
        fetchCommitsForBranch(task.branch);
    }, []);

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

            <>
                <div className='flex flex-row'>
                    {isSelectVisible ?
                        <div>
                            <Select
                                value={selectedBranch}
                                onChange={(value) => { setSelectedBranch(value); }}
                                className="!border-surface-mid-dark mb-3 text-surface-light focus:!border-secondary"
                                labelProps={{
                                    className: "before:content-none after:content-none",
                                }}
                                placeholder='Select branch'
                            >
                                {branches.map(branch => (
                                    <Option key={branch} value={branch} className='text-surface-mid-light'>{branch}</Option>
                                ))}
                            </Select>
                            <div className='flex mb-2'>
                                <p className="text-xs text-surface-light mt-2 hover:opacity-70 hover:cursor-pointer" onClick={() => setIsSelectVisible(false)}>
                                    Cancel
                                </p>
                                <p className="text-xs text-secondary hover:opacity-70 ml-2 mt-2 hover:cursor-pointer" onClick={handleUpdateTask}>
                                    Confirm
                                </p>
                            </div>

                        </div>
                        :
                        (task.assignedUser ? (
                            (task.assignedUser.username === currentUser.username || project.createdBy === currentUser.username) &&
                            <Button size='sm' className='flex text-center mb-2 bg-surface-darkest items-center justify-center text-surface-light hover:opacity-80 ' onClick={handleOpenModal}>Select Branch <MediationIcon className='ml-1' fontSize='extraSmall'></MediationIcon></Button>
                        ) : project.createdBy === currentUser.username ? <Button size='sm' className='flex text-center mb-2 bg-surface-darkest items-center justify-center text-surface-light hover:opacity-80 ' onClick={handleOpenModal}>Select Branch <MediationIcon className='ml-1' fontSize='extraSmall'></MediationIcon></Button> :
                            <></>
                        )


                    }
                </div>
                {displayText && <p className='text-surface-light font-sm mb-2'>{displayText}</p>}
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
                {!task.branch && !displayText && (
                    <p className="text-surface-light font-sm mb-2">No associated branch</p>
                )}
            </>
        </div>
    );
};


export default GithubSection;

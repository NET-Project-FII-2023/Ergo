import React, { useEffect, useState } from 'react';
import { Modal } from '@mui/material';
import { Button, Typography } from "@material-tailwind/react";
import ErgoInput from '../../widgets/form_utils/ErgoInput';
import ErgoTextarea from '../../widgets/form_utils/ErgoTextArea';
import ErgoDatePicker from '../../widgets/form_utils/ErgoDatePicker';
import api from '@/services/api';
import { toast } from "react-toastify";
import SettingsIcon from '@mui/icons-material/Settings';
import ErgoLabel from '../../widgets/form_utils/ErgoLabel';
import { useUser } from '../../context/LoginRequired';


const ProjectSettings = ({ project, token }) => {
    const [open, setOpen] = useState(false);
    const [githubToken, setGithubToken] = useState('');
    const [gitRepository, setGitRepository] = useState('');
    const [projectName, setProjectName] = useState('');
    const [projDescription, setProjDescription] = useState('');
    const [projDeadline, setProjDeadline] = useState(null);
    const currentUser = useUser();

    useEffect(() => {
        if (open) {
            fetchCurrentProject();
        }
    }, [open]);

    const fetchCurrentProject = async () => {
        try {
            if (!token || !project || !project.projectId) return;

            const response = await api.get(`/api/v1/Projects/${project.projectId}`, {
                headers: {
                    Authorization: `Bearer ${token}`,
                },
            });

            if (response.status === 200) {
                const data = response.data;
                setProjectName(data.projectName);
                setProjDescription(data.description);
                setProjDeadline(new Date(data.deadline));
                setGithubToken(data.githubToken);
                setGitRepository(data.gitRepository);
            } else {
                console.error('Error fetching project:', response);
            }
        } catch (error) {
            console.error('Error fetching project:', error);
        }
    };

    const handleOpen = () => {
        setOpen(true);
    };

    const handleClose = () => {
        setOpen(false);
    };

    const handleInputChange = (name, value) => {
        switch (name) {
            case 'githubToken':
                setGithubToken(value);
                break;
            case 'gitRepository':
                setGitRepository(value);
                break;
            case 'projectName':
                setProjectName(value);
                break;
            case 'description':
                setProjDescription(value);
                break;
            case 'deadline':
                setProjDeadline(value);
                break;
            default:
                break;
        }
    };
    
    const handleSaveSettings = async () => {
        try {
            const response = await api.put(`/api/v1/Projects/${project.projectId}`, {
                projectId: project.projectId,
                githubOwner: currentUser.username,
                projectOwner: currentUser.username,
                projectName: projectName,
                description: projDescription,
                deadline: projDeadline,
                githubToken: githubToken,
                gitRepository: gitRepository,
                state: 0
            }, {
                headers: {
                    Authorization: `Bearer ${token}`,
                },
            });

            if (response.status === 200) {
                handleClose();
                toast.success('Project settings saved successfully');
            } else {
                console.error('Error saving project settings:', response);
                toast.error('Failed to save project settings');
            }
        } catch (error) {
            console.error('Error saving project settings:', error);
            toast.error('Failed to save project settings');
        }
    };

    return (
        <div className='mr-8'>
             <Button className="w-full bg-surface-dark hover:bg-surface-mid-dark flex flex-row flex items-center" onClick={handleOpen} size="sm">
                <p className='text-center text-xs'>Settings</p>
                <SettingsIcon fontSize='extraSmall' className='text-center text-surface-light shadow-lg ml-1'></SettingsIcon>
             </Button>
            <Modal open={open} onClose={handleClose}>
                <div className="absolute top-1/2 left-1/2 transform -translate-x-1/2 -translate-y-1/2 w-[30rem] bg-[#2f2b3a] shadow-lg p-4 rounded">
                    <Typography variant='h4' className='text-white p-2'>
                        Project Settings
                    </Typography>
                    <div className='flex flex-col justify-between h-full'>
                         <div className='m-2'>
                            <ErgoLabel labelName="Project name" />
                            <ErgoInput
                                placeholder="Project name"
                                label='Project Name'
                                onChange={(value) => handleInputChange('projectName', value)}
                                value={projectName}
                            />
                        </div>
                        <div className='m-2'>
                        <ErgoLabel labelName="Description" />
                            <ErgoTextarea
                                placeholder="Description"
                                label='Description'
                                onChange={(value) => handleInputChange('description', value)}
                                value={projDescription}
                            />
                        </div>
                        <div className='m-2'>
                        <ErgoLabel labelName="Github Token" />
                            <ErgoInput
                                placeholder="Github Token"
                                label='GitHub Token'
                                onChange={(value) => handleInputChange('githubToken', value)}
                                value={githubToken}
                            />
                        </div>
                        <div className='m-2'>
                        <ErgoLabel labelName="Github Repository" />
                            <ErgoInput
                                placeholder="Github Repository"
                                label='Git Repository'
                                onChange={(value) => handleInputChange('gitRepository', value)}
                                value={gitRepository}
                            />
                        </div>
                        <div className='m-2'>
                        <ErgoLabel labelName="Deadline" />
                            <ErgoDatePicker
                                label='Deadline'
                                selectedDate={projDeadline}
                                onChange={(value) => handleInputChange('deadline', value)}
                            />
                        </div>
                        <div className='m-2 self-end'>
                            <Button size="sm" className="bg-secondary hover:bg-primary" onClick={handleSaveSettings}>
                                Save
                            </Button>
                        </div>
                    </div>
                </div>
            </Modal>
        </div>
    );
};

export default ProjectSettings;

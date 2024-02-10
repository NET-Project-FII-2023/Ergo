import React, { useState } from 'react';
import { Modal } from '@mui/material';
import { Button, Typography } from "@material-tailwind/react";
import ErgoInput from '../../widgets/form_utils/ErgoInput';
import api from '@/services/api';
import { toast } from "react-toastify";
import SettingsIcon from '@mui/icons-material/Settings';

const ProjectSettings = ({ project }) => {
    const [open, setOpen] = useState(false);
    const [githubToken, setGithubToken] = useState(project.githubToken);
    const [gitRepository, setGitRepository] = useState(project.gitRepository);

    const handleOpen = () => {
        setOpen(true);
    };

    const handleClose = () => {
        setOpen(false);
    };

    const handleInputChange = (name, value) => {
        if (name === 'githubToken') {
            setGithubToken(value);
        } else if (name === 'gitRepository') {
            setGitRepository(value);
        }
    };

    // const handleSaveSettings = async () => {
    //     try {
    //         // Perform API call to save settings
    //         const response = await api.put(`/api/v1/Projects/${project.id}`, {
    //             githubToken: githubToken,
    //             gitRepository: gitRepository
    //         });

    //         if (response.status === 200) {
    //             handleClose();
    //             toast.success('Project settings saved successfully');
    //         } else {
    //             console.error('Error saving project settings:', response);
    //             toast.error('Failed to save project settings');
    //         }
    //     } catch (error) {
    //         console.error('Error saving project settings:', error);
    //         toast.error('Failed to save project settings');
    //     }
    // };

    return (
        <div className='mr-8'>
             <Button className="w-full bg-surface-mid-dark hover:bg-surface-mid-dark" onClick={handleOpen} size="sm">
                <SettingsIcon fontSize='extraSmall'></SettingsIcon>
             </Button>
            <Modal open={open} onClose={handleClose}>
                <div className="absolute top-1/2 left-1/2 transform -translate-x-1/2 -translate-y-1/2 w-[25rem] bg-[#2f2b3a] shadow-lg p-4 rounded">
                    <Typography variant='h4' className='text-white p-2'>
                        Project Settings
                    </Typography>
                    <div className='flex flex-col justify-between h-full'>
                        <div className='m-2'>
                            <ErgoInput
                                label='GitHub Token'
                                placeholder="Enter GitHub Token"
                                onChange={(value) => handleInputChange('githubToken', value)}
                                value={githubToken}
                            />
                        </div>
                        <div className='m-2'>
                            <ErgoInput
                                label='Git Repository'
                                placeholder="Enter Git Repository"
                                onChange={(value) => handleInputChange('gitRepository', value)}
                                value={gitRepository}
                            />
                        </div>
                        <div className='m-2 self-end'>
                            <Button size="sm" className="bg-secondary hover:bg-primary">
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

import React, { useState } from 'react';
import { Modal } from '@mui/material';
import { Button, Typography } from "@material-tailwind/react";
import api from '@/services/api';
import { toast } from "react-toastify";
import ErgoInput from '../../widgets/form_utils/ErgoInput';
import ErgoTextarea from '../../widgets/form_utils/ErgoTextArea';
import ErgoDatePicker from '../../widgets/form_utils/ErgoDatePicker';
import AddTaskIcon from '@mui/icons-material/AddTask';
import { Card, CardContent} from '@mui/material';
import { useUser } from "@/context/LoginRequired";


const AddProject = ({ token, onProjectAdded }) => {
    const [open, setOpen] = useState(false);
    const currentUser = useUser()
    const [projectDetails, setProjectDetails] = useState({
        projectName: '',
        description: '',
        githubOwner: currentUser.username,
        githubToken: '',
        gitRepository: '',
        fullName: currentUser.username,
        deadline: null
    });

    const handleOpen = () => {
        setOpen(true);
    };

    const handleClose = () => {
        setOpen(false);
    };

    const handleInputChange = (name, value) => {
        setProjectDetails(prevDetails => ({ ...prevDetails, [name]: value }));
    };

    const handleDateChange = (date) => {
        setProjectDetails(prevDetails => ({ ...prevDetails, deadline: date }));
    };

    const handleAddProject = async () => {
        try {
            const response = await api.post('/api/v1/Projects', projectDetails, {
                headers: {
                    Authorization: `Bearer ${token}`,
                },
            });

            if (response.status === 200) {
                handleClose();
                toast.success(`Project added successfully: '${projectDetails.projectName}'`);
                onProjectAdded();
            } else {
                console.error('Error adding project:', response);
                toast.error(response);
            }
        } catch (error) {
            console.error('Error adding project:', error);
            toast.error(error.message);
        }
    };

    return (
        <div>
             <Button className="w-full bg-surface-mid-dark mt-2 hover:bg-surface-mid-light" onClick={handleOpen} size="sm">
                +
             </Button>
            <Modal open={open} onClose={handleClose}>
                <div className="absolute top-1/2 left-1/2 transform -translate-x-1/2 -translate-y-1/2 w-[25rem] bg-[#2f2b3a] shadow-lg p-4 rounded">
                    <Typography variant='h4' className='text-white p-2'>
                        Add Project
                    </Typography>
                    <div className='flex flex-col justify-between h-full'>
                        <div className='m-2'>
                            <ErgoInput
                                label='Project Name'
                                placeholder="Enter Project Name"
                                onChange={(value) => handleInputChange('projectName', value)}
                                value={projectDetails.projectName}
                            />
                        </div>
                        <div className='m-2'>
                            <ErgoTextarea
                                label='Description'
                                placeholder="Enter Description"
                                onChange={(value) => handleInputChange('description', value)}
                                value={projectDetails.description}
                            />
                        </div>
                        <div className='m-2'>
                            <ErgoInput
                                label='GitHub Token'
                                placeholder="Enter GitHub Token"
                                onChange={(value) => handleInputChange('githubToken', value)}
                                value={projectDetails.githubToken}
                            />
                        </div>
                        <div className='m-2'>
                            <ErgoInput
                                label='Git Repository'
                                placeholder="Enter Git Repository"
                                onChange={(value) => handleInputChange('gitRepository', value)}
                                value={projectDetails.gitRepository}
                            />
                        </div>
                        <div className='m-2'>
                            <ErgoDatePicker
                                label='Deadline'
                                selectedDate={projectDetails.deadline}
                                onChange={handleDateChange}
                            />
                        </div>
                        <div className='m-2 self-end'>
                            <Button size="sm" className="bg-secondary hover:bg-primary" onClick={handleAddProject}>
                                <AddTaskIcon fontSize='small'>
                                </AddTaskIcon>
                            </Button>
                        </div>
                    </div>
                </div>
            </Modal>
        </div>
    );
};

export default AddProject;
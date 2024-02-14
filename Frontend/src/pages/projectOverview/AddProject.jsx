import React, { useState } from 'react';
import { Modal } from '@mui/material';
import { Button, Typography,Checkbox } from "@material-tailwind/react";
import api from '@/services/api';
import { toast } from "react-toastify";
import ErgoInput from '../../widgets/form_utils/ErgoInput';
import ErgoTextarea from '../../widgets/form_utils/ErgoTextArea';
import ErgoDatePicker from '../../widgets/form_utils/ErgoDatePicker';
import ErgoLabel from '../../widgets/form_utils/ErgoLabel';
import AddTaskIcon from '@mui/icons-material/AddTask';
import CircularProgress from '@mui/material/CircularProgress';
import TipsAndUpdatesOutlinedIcon from '@mui/icons-material/TipsAndUpdatesOutlined';
import { useUser } from "@/context/LoginRequired";


const AddProject = ({ onProjectAdded }) => {
    const [open, setOpen] = useState(false);
    const currentUser = useUser()
    const [aiActive, setAiActive] = useState(false);
    const [openAiIsLoading, setOpenAiIsLoading] = useState(false);

    const [projectDetails, setProjectDetails] = useState({
        projectName: '',
        description: '',
        githubOwner: '',
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
    const fetchAiTask = async (projectDetailsAi) => {
        try {
            const response = await api.post("api/v1/OpenAi", {
               title: projectDetails.projectName,
                description: projectDetails.description,
                type: "project-tasks" 
            }, {
                headers: {
                    Authorization: `Bearer ${currentUser.token}`,
                },
                });
            if (response.status === 200) {
                setAiActive(false);
                return response.data.tasks;
                
            }else{
                console.error('Error creating AI Task:', response);
                toast.error(response);
            }
        }catch (error) {
            console.error('Error creating AI Task:', error);
            toast.error(error.message);
        }   
    };
    const handleAddTask = async (taskWithProjectId) => {
        try {
          const response = await api.post('/api/v1/TaskItems', taskWithProjectId, {
            headers: {
              Authorization: `Bearer ${currentUser.token}`,
            },
          });
    
          if (response.status === 200) {
            return;
          } else {
            console.error('Error adding task:', response);
            toast.error(response);
          }
        } catch (error) {
          console.error('Error adding task:', error);
        }
        
      };
    const handleAddProjectWithAI = async (projectId,projectDetailsAi) => {
        setOpenAiIsLoading(true);  
        try {
            let tasks = await fetchAiTask(projectDetailsAi);
            if(tasks != null){
                tasks.forEach(async (task) => {
                    const taskWithProjectId = { ...task, "createdBy": currentUser.username,projectId, "state": 1 };
                    await handleAddTask(taskWithProjectId);
                });
            } 
            setOpenAiIsLoading(false)
        } catch (error) {
            console.error('Error adding project with AI:', error);
        }

    };
    const handleAddProject = async () => {
        
        try {
            const response = await api.post('/api/v1/Projects', projectDetails, {
                headers: {
                    Authorization: `Bearer ${currentUser.token}`,
                },
            });

            if (response.status === 200) {
                handleClose();
                toast.success(`Project added successfully: '${projectDetails.projectName}'`);
                if(aiActive){
                    handleAddProjectWithAI(response.data.project.projectId,projectDetails);
                }
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
        <Button className="w-full h-12 bg-surface-mid-dark/20 mt-2 hover:bg-surface-mid-light" onClick={handleOpen} size="sm" title="Create a new project">
          <p className='text-white opacity-100'>+</p>
        </Button>
        <Modal open={open} onClose={handleClose}>
          <div className="absolute top-1/2 left-1/2 transform -translate-x-1/2 -translate-y-1/2 md:w-[30rem] w-[90vw] bg-[#2f2b3a] shadow-lg p-4 rounded">
            <Typography variant='h4' className='text-white p-2'>
              Add Project
            </Typography>
            <div className='flex flex-col justify-between h-full'>
              <div className='m-2'>
                <ErgoLabel labelName="Project name" />
                <ErgoInput
                  placeholder="Enter Project Name"
                  onChange={(value) => handleInputChange('projectName', value)}
                  value={projectDetails.projectName}
                />
              </div>
              <div className='m-2'>
                <ErgoLabel labelName="Description" />
                <ErgoTextarea
                  placeholder="Enter Description"
                  onChange={(value) => handleInputChange('description', value)}
                  value={projectDetails.description}
                />
              </div>
              <div className='m-2'>
                <ErgoLabel labelName="Github Owner" />
                <ErgoInput
                  placeholder="Github Owner"
                  onChange={(value) => handleInputChange('githubOwner', value)}
                  value={projectDetails.githubOwner}
                />
              </div>
              <div className='m-2'>
                <ErgoLabel labelName="Github Owner" />
                <ErgoInput
                  placeholder="Enter GitHub Token"
                  onChange={(value) => handleInputChange('githubToken', value)}
                  value={projectDetails.githubToken}
                />
              </div>
              <div className='m-2'>
                <ErgoLabel labelName="Git Repository" />
                <ErgoInput
                  placeholder="Enter Git Repository"
                  onChange={(value) => handleInputChange('gitRepository', value)}
                  value={projectDetails.gitRepository}
                />
              </div>
              <div className='m-2'>
                <div>
                  <ErgoLabel labelName="Deadline" />
                  <ErgoDatePicker
                    label='Deadline'
                    selectedDate={projectDetails.deadline}
                    onChange={handleDateChange}
                  />
                </div>
              </div>
              <div className='m-2 flex justify-start'>
                <div className='flex items-center'>   
                <Checkbox id="public" name="public" value="public" onChange={() => setAiActive(!aiActive)}/>
                  <p className='text-surface-light'>AI Assistance</p>
                  <TipsAndUpdatesOutlinedIcon fontSize="small" className='text-surface-light text-center ml-2'/>
                </div>
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
        {openAiIsLoading && (
          <div className="fixed inset-0 bg-black bg-opacity-50 z-50 flex flex-col  justify-center items-center">
            <CircularProgress />
            <p className='text-surface-light mt-2'>Project loading...</p>
          </div>
        )}
      </div>
    );
};

export default AddProject;

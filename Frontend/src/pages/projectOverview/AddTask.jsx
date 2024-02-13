import React, { useEffect ,useState } from 'react';
import { Modal } from '@mui/material';
import {
  Button,
  Typography,
  Checkbox
} from "@material-tailwind/react";
import api from '@/services/api';
import { useUser } from '../../context/LoginRequired';
import ErgoInput from '../../widgets/form_utils/ErgoInput';
import ErgoLabel from '../../widgets/form_utils/ErgoLabel';
import ErgoDatePicker from '../../widgets/form_utils/ErgoDatePicker';
import {toast} from "react-toastify";
import ErgoTextarea from '../../widgets/form_utils/ErgoTextArea';
import { Card, CardContent} from '@mui/material';
import AddTaskIcon from '@mui/icons-material/AddTask';
import TipsAndUpdatesOutlinedIcon from '@mui/icons-material/TipsAndUpdatesOutlined';
import { Select, Option } from '@material-tailwind/react';



const AddTask = ({ projectId, token, userId, onTaskAdded }) => {
  const [open, setOpen] = useState(false);
  const user = useUser();
  const [selectedBranch, setSelectedBranch] = useState('');
  const [branches, setBranches] = useState([]);
  const [aiActive, setAiActive] = useState(false);
  const [taskDetails, setTaskDetails] = useState({
    taskName: '',
    description: '',
    deadline: null,
    createdBy: user?.username,
    projectId: projectId,
    state: 1,
    branch: ''
  });

  const handleOpen = () => {
    fetchBranches();
    setOpen(true);
    setTaskDetails({
      taskName: '',
      description: '',
      deadline: null,
      createdBy: user?.username,
      projectId: projectId,
      state: 1,
      branch: ''
    });
  };

  const fetchBranches = async () => {
    try {
        const response = await api.post('/api/v1/GitHub/branches', { "projectId": projectId }, {
            headers: {
                Authorization: `Bearer ${token}`,
            },
        })

        if (response.status === 200) {
            setBranches(response.data);
            console.log("mydata", response.data);
        }
        else {
            console.error("Fail");
        }
    } catch (err) {
        console.error(err);
    }
}

  const handleClose = () => {
    setOpen(false);
  };

  const handleInputChange = (name, value) => {
    setTaskDetails((prevDetails) => ({ ...prevDetails, [name]: value }));
  };

  const handleDateChange = (date) => {
    if (date < new Date()) {
      toast.error('The deadline must be at least tomorrow');
      return;
    }
    setTaskDetails((prevDetails) => ({ ...prevDetails, deadline: date }));
  };
  const fetchAiTaskDescription = async () => {
  try{
    const response = await api.post("api/v1/OpenAi", {
      title: taskDetails.taskName,
      description: taskDetails.description,
      type: "task" 
  }, {
      headers: {
          Authorization: `Bearer ${token}`,
      },
      });
  if (response.status === 200) {
      setAiActive(false);
      return response.data;;
  }else{
      console.error('Error creating AI Task:', response);
      toast.error(response);
  }
  }catch (error) {
    console.error('Error creating AI Task:', error);
    toast.error(error.message);
}   
  }

  const handleAddTask = async () => {
    try {
      let updatedTaskDetails = taskDetails; 
      if(aiActive){
        const description = await fetchAiTaskDescription();
        updatedTaskDetails = { ...updatedTaskDetails, description: description };
      }
      const response = await api.post('/api/v1/TaskItems', updatedTaskDetails, {
        headers: {
          Authorization: `Bearer ${token}`,
        },
      });

      if (response.status === 200) {
        onTaskAdded();
        handleClose();
        toast.success(`Added task succesfully: '${taskDetails.taskName}'`);
      } else {
        console.error('Error adding task:', response);
        toast.error(response);
      }
    } catch (error) {
      console.error('Error adding task:', error);
    }
  };
  
  const handleBranchSelectionChange = (value) => {
    // setSelectedBranch(value);
    setTaskDetails((prevDetails) => ({ ...prevDetails, branch: value }));
  }

  return (
    <div>
      <Card
      className={`mb-4 opacity-80 cursor-pointer`}
      style={{
        backgroundColor: "#2f2b3a",
      }}
      onClick={handleOpen}
    >
      <CardContent className='flex justify-center items-center transition duration-200 ease-in-out hover:bg-surface-mid'>
        <Typography gutterBottom className='text-white'>
          +
        </Typography>
      </CardContent>
    </Card>
      <Modal open={open} onClose={handleClose}>
        <div className="absolute top-1/2 left-1/2 transform -translate-x-1/2 -translate-y-1/2 md:w-[25rem] w-[90vw] bg-[#2f2b3a] shadow-lg p-4 rounded">
          <Typography variant='h4' className='text-white p-2'>
              Add Task
          </Typography>
          <div className='flex flex-col justify-between h-full'>
            <div className='m-2'>
              <ErgoLabel labelName="Task Name" />
              <ErgoInput
                placeholder="Enter Task Name"
                onChange={(value) => handleInputChange('taskName', value)}
                value={taskDetails.taskName}
              />
            </div>
            <div className='m-2'>
              <ErgoLabel labelName="Description" />
              <ErgoTextarea
                placeholder={aiActive ? "Enter a phrase for AI to generate a description":"Enter Description"}
                onChange={(value) => handleInputChange('description', value)}
                value={taskDetails.description}
              />
            </div>
            <div className='m-2'>          
              <ErgoLabel labelName="Deadline" />
              <ErgoDatePicker
                selectedDate={taskDetails.deadline}
                onChange={handleDateChange}
              />
            </div>
            <div className='m-2'>
              <ErgoLabel labelName="Repository branch" />
              <Select
                value={selectedBranch} 
                onChange={(value) => handleBranchSelectionChange(value)}
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
            </div>
            <div className='m-2 flex justify-start'>
                            <div className='flex items-center'>   
                            <Checkbox id="public" name="public" value="public" onChange={() => setAiActive(!aiActive)}/>
                               <p className='text-surface-light'>AI Assistance</p>
                                <TipsAndUpdatesOutlinedIcon fontSize="small" className='text-surface-light text-center ml-2'/>
                            </div>
                        </div>  
            <div className='m-2 self-end'>
              <Button size="sm" className="bg-secondary hover:bg-primary" onClick={handleAddTask}>
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

export default AddTask;

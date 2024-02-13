import {React,useState} from 'react'
import { Modal, Fade, MenuItem } from '@mui/material';
import api from '@/services/api';
import {toast} from "react-toastify";
import {Button,Input, Typography, Textarea} from "@material-tailwind/react";
const MachineLearningModal = ({handleCloseMLModal,modalOpen,token}) => {

  const [dataToAnalyze, setDataToAnalyze] = useState({
    taskComplexity: 0,
    taskUrgency: 0,
    numberOfParticipants: 0,
  })
  const [analizedTime, setAnalizedTime] = useState(0);
  const handleAnalyze = async() => {
    if(dataToAnalyze.taskComplexity < 1 || dataToAnalyze.taskComplexity > 3){
      toast.error('Task Complexity must be between 1 and 3');
      return;
    }
    if(dataToAnalyze.taskUrgency < 1 || dataToAnalyze.taskUrgency > 3){
      toast.error('Task Urgency must be between 1 and 3');
      return;
    }
    if(dataToAnalyze.numberOfParticipants < 1 || dataToAnalyze.numberOfParticipants > 50){
      toast.error('Number of Participants must be between 1 and 50');
      return;
    }
    try{
      const response = await api.post('/api/v1/ML', dataToAnalyze,{
        headers: {
          Authorization: `Bearer ${token}`,
        },
      });
      if(response.status === 200){
        toast.success('Analysis Successful');
        setAnalizedTime(response.data.predictedResolutionTime);
      }
    }catch(error){
      console.error('Error analyzing:', error);
      toast.error('Error analyzing');
    }
  }

  return (
    <Modal
      open={modalOpen}
      onClose={handleCloseMLModal}
      aria-labelledby="modal-title"
      aria-describedby="modal-description"
      closeAfterTransition
    >
    <Fade in={modalOpen}>
    <div className="absolute top-1/2 left-1/2 transform -translate-x-1/2 -translate-y-1/2 md:w-[30rem] w-[80vw] bg-[#2f2b3a] shadow-lg p-4 rounded">
    <div className='flex flex-col gap-2 items-center'>
      <Typography variant="h5" className="mb-1 text-surface-light">Predict time</Typography>

          <div className='flex flex-col gap-2 w-[16rem]'>
            <Input
              value={dataToAnalyze.taskComplexity}
              onChange={(e) => setDataToAnalyze({...dataToAnalyze, taskComplexity: e.target.value})}
              variant={"outlined"}
              label={"Task Complexity"}
              size={"md"}
              color={"deep-purple"}
              className={"text-white"}
              crossOrigin={undefined}
            />
            <Input
              value={dataToAnalyze.taskUrgency}
              onChange={(e) => setDataToAnalyze({...dataToAnalyze, taskUrgency: e.target.value})}
              variant={"outlined"}
              label={"Task Urgency"}
              size={"md"}
              color={"deep-purple"}
              className={"text-white"}
              crossOrigin={undefined}
            />
            <Input
              value={dataToAnalyze.numberOfParticipants}
              onChange={(e) => setDataToAnalyze({...dataToAnalyze, numberOfParticipants: e.target.value})}
              variant={"outlined"}
              label={"Number of Participants"}
              size={"md"}
              color={"deep-purple"}
              className={"text-white"}
              crossOrigin={undefined}
            />
          </div>
          {analizedTime > 0 && <p className='text-surface-light'>Predicted Time: {analizedTime}</p>}
          <Button onClick={handleAnalyze} className='bg-primary' buttonType="filled" size="regular" rounded={false} block={false} iconOnly={false} ripple="light">
            Analyze
          </Button>
      </div>
      
      </div>
    </Fade>
    </Modal>
  )
}

export default MachineLearningModal
import React, { useState, useEffect } from 'react';
import { Button } from '@material-tailwind/react';
import api from '@/services/api';
import TimerIcon from '@mui/icons-material/Timer';
import PauseIcon from '@mui/icons-material/Pause';
import MovingIcon from '@mui/icons-material/Moving';
import { useUser } from "@/context/LoginRequired";

const TimerSection = ({ task, token }) => {
    const [startTime, setStartTime] = useState(null);
    const [elapsedTime, setElapsedTime] = useState(0);
    const [isTimerRunning, setIsTimerRunning] = useState(false);
    const currentUser = useUser();

    useEffect(() => {
        const fetchRecordedTime = async () => {
            try {
                const response = await api.get(`/api/v1/TaskItems/GetTaskItemTime/${task.taskItemId}`, {
                    headers: {
                        Authorization: `Bearer ${token}`,
                    },
                });

                if (response.status === 200) {
                    const { recordedTime } = response.data;
                    // Convert recorded time to milliseconds
                    const recordedTimeMilliseconds = parseTimeToMilliseconds(recordedTime);
                    setElapsedTime(recordedTimeMilliseconds);
                } else {
                    console.error('Error fetching recorded time:', response);
                }
            } catch (error) {
                console.error('Error fetching recorded time:', error);
            }
        };

        fetchRecordedTime();
    }, [task.taskItemId, token]);

    useEffect(() => {
        let interval;
        if (isTimerRunning) {
            interval = setInterval(() => {
                const currentTime = new Date().getTime();
                const timePassed = currentTime - startTime;
                setElapsedTime(prevElapsedTime => prevElapsedTime + timePassed);
                setStartTime(currentTime);
            }, 1000);
        } else {
            clearInterval(interval);
        }

        return () => clearInterval(interval);
    }, [isTimerRunning, startTime]);

    const handleStartTimer = async () => {
        try {
            const response = await api.post('/api/v1/TaskItems/StartTimer', {
                taskItemId: task.taskItemId,
                userId: currentUser.userId
            }, {
                headers: {
                    Authorization: `Bearer ${token}`,
                },
            });

            if (response.status === 200) {
                setStartTime(new Date().getTime());
                setIsTimerRunning(true);
            } else {
                console.error('Error starting timer:', response);
            }
        } catch (error) {
            console.error('Error starting timer:', error);
        }
    };

    const handlePauseTimer = async () => {
        try {
            const response = await api.post('/api/v1/TaskItems/PauseTimer', {
                taskItemId: task.taskItemId,
                userId: currentUser.userId
            }, {
                headers: {
                    Authorization: `Bearer ${token}`,
                },
            });

            if (response.status === 200) {
                setIsTimerRunning(false);
            } else {
                console.error('Error pausing timer:', response);
            }
        } catch (error) {
            console.error('Error pausing timer:', error);
        }
    };

    const formatTime = (milliseconds) => {
        const totalSeconds = Math.floor(milliseconds / 1000);
        const hours = Math.floor(totalSeconds / 3600);
        const minutes = Math.floor((totalSeconds % 3600) / 60);
        const seconds = totalSeconds % 60;

        return `${String(hours).padStart(2, '0')}:${String(minutes).padStart(2, '0')}:${String(seconds).padStart(2, '0')}`;
    };

    // Function to convert time string (HH:MM:SS) to milliseconds
    const parseTimeToMilliseconds = (timeString) => {
        const timeComponents = timeString.split(':');
        const hours = parseInt(timeComponents[0]);
        const minutes = parseInt(timeComponents[1]);
        const seconds = parseFloat(timeComponents[2]);
        return (hours * 3600 + minutes * 60 + seconds) * 1000;
    };

    return (
        <div>
            <div className='flex items-center mb-3'>
                <MovingIcon fontSize='small' className='text-secondary'></MovingIcon>
                <p className='text-gray-300 ml-1 text-md font-semibold'>
                    Progress Tracking
                </p>
            </div>
            <div className='flex'>
                <Button size="sm" className='bg-surface-darkest flex items-center mr-2' onClick={handleStartTimer}>
                    <p className='text-md text-surface-light'>Start</p>
                    <TimerIcon fontSize='small' className='ml-1 text-secondary'></TimerIcon>
                </Button>
                <Button size="sm" className='bg-surface-darkest flex items-center' onClick={handlePauseTimer}>
                    <p className='text-md text-surface-light'>Pause</p>
                    <PauseIcon fontSize='small' className='ml-1'></PauseIcon>
                </Button>
            </div>
            <div className='mt-2 flex items-center'>
                <p className='text-surface-light mr-1 text-sm'>Elapsed Time:</p>
                <p className='text-gray-100 text-sm'>{formatTime(elapsedTime)}</p>
            </div>
        </div>
    );
};

export default TimerSection;

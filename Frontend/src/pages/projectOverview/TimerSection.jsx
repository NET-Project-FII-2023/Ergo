import React, { useState, useEffect } from 'react';
import { Button } from '@material-tailwind/react';
import api from '@/services/api';
import TimerIcon from '@mui/icons-material/Timer';
import PauseIcon from '@mui/icons-material/Pause';
import MovingIcon from '@mui/icons-material/Moving';


const TimerSection = (() => {
    return(
        <div>  
            <div className='flex items-center mb-3'>
                <MovingIcon fontSize='small' className='text-secondary'></MovingIcon>   
                <p className='text-gray-300 ml-1 text-md font-semibold'>
                    Progress Tracking
                </p>
                 
            </div>  
            <div className='flex'>
                <Button size="sm" className='bg-surface-darkest flex items-center mr-2'>
                    <p className='text-md text-surface-light'>Start</p>
                    <TimerIcon fontSize='small' className='ml-1 text-secondary'></TimerIcon>
                </Button>
                <Button size="sm" className='bg-surface-darkest flex items-center'>
                    <p className='text-md text-surface-light'>Pause</p>
                    <PauseIcon fontSize='small' className='ml-1'></PauseIcon>
                </Button>
            </div>
            <div className='mt-2 flex items-center'> 
                <p className='text-surface-light mr-1 text-sm'>Elapsed Time:</p>
                <p className='text-gray-100 text-sm'>01:42:24</p>
            </div>
        </div>
    );
});

export default TimerSection;
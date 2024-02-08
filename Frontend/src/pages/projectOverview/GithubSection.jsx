import React, { useState, useEffect } from 'react';
import { Button } from '@material-tailwind/react';
import api from '@/services/api';
import GitHubIcon from '@mui/icons-material/GitHub';

const GithubSection = ({}) => {
    const githubActivity = [
        { id: 1, message: "Added navbar" },
        { id: 2, message: "Implemented Github integration" },
        { id: 3, message: "Implemented register functionality" },
        { id: 4, message: "Designed home page" },
        { id: 5, message: "Redesigned profile page" },
        { id: 6, message: "Added badges to profile" },
        { id: 7, message: "Implemented a feature" },
    ];

    return (
        <div className='mt-6'>    
            <div className='flex items-center mb-3'>
                <GitHubIcon fontSize='sm' className='text-secondary mr-1'></GitHubIcon>
                <p className='text-gray-300 ml-1 text-md font-semibold'>
                    Github Activity
                </p>
            </div>
            <div className="text-surface-light">
                <ul className="list-none">
                    {githubActivity.map(item => (
                        <li key={item.id} className="relative pl-4 py-1">
                            {item.message}
                            <span className="absolute top-0 left-0 w-1 h-full bg-surface-mid"></span>
                        </li>
                    ))}
                </ul>
            </div>
        </div>
    );
};

export default GithubSection;

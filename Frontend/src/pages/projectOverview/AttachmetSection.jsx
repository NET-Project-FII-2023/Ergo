import React, { useState, useEffect } from 'react';
import AttachFileIcon from '@mui/icons-material/AttachFile';
import UploadFileIcon from '@mui/icons-material/UploadFile';
import { Button } from '@material-tailwind/react';
import api from '@/services/api';
import { useUser } from '../../context/LoginRequired';

  const AttachmentSection = ({ attachedFiles, handleFileInputChange, project }) => {
    const currentUser = useUser();

    return (
        <div className='flex flex-col mt-4 pr-2'>
            <div className='flex flex-row items-center'> 
                <AttachFileIcon className='text-secondary ml-1' fontSize='extraSmall'> </AttachFileIcon>
                <p className='text-gray-300  text-md font-semibold'>
                    Attachments
                </p>
            </div>
            <div>
                {project.createdBy === currentUser.username && 
                <Button className='flex flex-row px-3 bg-surface-darkest py-2 ml-4 hover:opacity-70' size="sm">
                    <label htmlFor="file-upload" className="cursor-pointer text-gray-300 flex items-center">
                        <p className='text-md text-surface-light'>Upload</p>
                        <UploadFileIcon fontSize='small' className='ml-1'></UploadFileIcon>
                    </label>
                </Button>}
                
                <input
                    id="file-upload"
                    type="file"
                    className="hidden"
                    onChange={handleFileInputChange}
                    multiple
                />
            </div>
        </div>
    );
}


export default AttachmentSection;
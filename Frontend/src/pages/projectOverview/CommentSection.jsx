import React, { useState, useEffect } from 'react';
import { Modal, Fade, Card, CardContent } from '@mui/material';
import ModeCommentIcon from '@mui/icons-material/ModeComment';
import { Button, Typography } from '@material-tailwind/react';
import api from '@/services/api';

const CommentSection = ({ task, token }) => {
    return(
        <div className='flex flex-col mt-4 px-2'>
            <div className='flex items-center mb-2'>
                <ModeCommentIcon className='text-secondary ml-1' fontSize='extraSmall'></ModeCommentIcon>
                <p className='text-gray-300 ml-1 text-md font-semibold'>
                    Comments
                </p>
            </div>
            
            <Card className="opacity-80 cursor-pointer mb-2 rounded w-4/5 ml-2">
                <CardContent className='p-2 rounded bg-surface-darkest'>
                    <div className='flex'>
                        <div>
                            <p className="text-surface-light text-xs">
                                Michael Jackson
                            </p>
                            <p className="text-surface-mid-light text-xs">
                                Lorem, ipsum dolor sit amet consectetur adipisicing elit. Ullam fugit minus deleniti, nisi, consequatur itaque illo possimus ducimus.
                            </p>
                        </div>
                    </div>
                </CardContent>
            </Card>
            <Card className="opacity-80 cursor-pointer mb-2 rounded w-4/5 ml-2">
                <CardContent className='p-2 rounded bg-surface-darkest'>
                    <div className='flex'>
                        <div>
                            <p className="text-surface-light text-xs">
                                Baracobama
                            </p>
                            <p className="text-surface-mid-light text-xs">
                                Lorem, ipsum dolor sit amet consectetur adipisicing elit. Ullam fugit minus deleniti, nisi, consequatur itaque illo possimus ducimus.
                            </p>
                        </div>
                    </div>
                </CardContent>
            </Card>
        </div>
    );
};

export default CommentSection; 
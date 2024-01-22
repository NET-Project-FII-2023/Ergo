import React from "react";
import {Navigate, Outlet} from 'react-router-dom';
import axios from "axios";
import {api_path} from "../../api/APIUtils";

export default function LoginRequired() {
    const checkIsLoggedIn = () => {
        const token = localStorage.getItem('token');

        if (!token) {
            return false;
        }

        // check if token is valid
    };

    if (!checkIsLoggedIn()) {
        return <Navigate to='/login' replace />;
    }

    const userInfo = {}

    return <Outlet />;

}
import React from "react";
import {Navigate, Outlet} from 'react-router-dom';

export default function LoginRequired() {
    const isLoggedIn = true;

    if (!isLoggedIn) {
        return <Navigate to='/login' replace />;
    }

    return <Outlet />;

}
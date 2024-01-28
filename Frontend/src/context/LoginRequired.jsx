import {Navigate, Outlet} from "react-router-dom";
import api from "@/services/api.jsx";
import {useEffect, useState, createContext, useContext} from "react";

const UserContext = createContext({
    token: null,
    userId: null,
    username: null,
    name: null,
    email: null,
    role: null,
});

export const useUser = () => {
    const context = useContext(UserContext);
    if (context === undefined) {
        throw new Error("useUser must be used within a UserProvider");
    }
    return context;
}

export default function LoginRequired() {
    const [isAuthenticated, setIsAuthenticated] = useState(null);
    const [user, setUser] = useState({
        token: null,
        userId: null,
        username: null,
        name: null,
        email: null,
        role: null,
    });

    useEffect(() => {
        const checkLogin = async () => {
            const token = localStorage.getItem("token");
            if (!token) {
                setIsAuthenticated(false);
                return;
            }

            try {
                const response = await api.get("/api/v1/Authentication/currentuserinfo", {
                    headers: {
                        Authorization: `Bearer ${token}`
                    }
                });

                if (response.status === 200 && response.data?.isAuthenticated) {
                    setIsAuthenticated(true);
                    setUser({
                        token: token,
                        userId: response.data?.claims?.["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier"],
                        username: response.data?.userName,
                        name: response.data?.claims?.["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name"],
                        email: response.data?.claims?.["http://schemas.xmlsoap.org/ws/2005/05/identity/claims/emailaddress"],
                        role: response.data?.claims?.["http://schemas.microsoft.com/ws/2008/06/identity/claims/role"],
                    })
                } else {
                    setIsAuthenticated(false);
                }
            } catch (error) {
                setIsAuthenticated(false);
            }
        };

        checkLogin();
    }, []);

    if(isAuthenticated === null) {
        return <div>Loading</div>;
    }


    if(!isAuthenticated) {
        return <Navigate to="/auth/sign-in" replace/>;
    }

    return (
        <UserContext.Provider value={user}>
            <Outlet/>
        </UserContext.Provider>
    )

}
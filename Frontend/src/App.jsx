import { Routes, Route, Navigate } from "react-router-dom";
import { Dashboard, Auth } from "@/layouts";
import LoginRequired from "@/context/LoginRequired.jsx";


function App() {
  return (
    <Routes>
        <Route element={<LoginRequired/>}>
            <Route path="/dashboard/*" element={<Dashboard/>}/>
        </Route>
        <Route path="/auth/*" element={<Auth/>}/>
        <Route path="*" element={<Navigate to="/dashboard/home" replace/>}/>
    </Routes>
  );
}

export default App;

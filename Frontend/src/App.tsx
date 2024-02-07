import { Routes, Route, Navigate } from "react-router-dom";
import { Dashboard, Auth } from "./layouts";
import LoginRequired from "./context/LoginRequired";
import NotFound from "./pages/NotFound/NotFound";

function App() {
  return (
    <Routes>
        <Route element={<LoginRequired/>}>
            <Route path="/dashboard/*" element={<Dashboard/>}/>
        </Route>
        <Route path="/auth/*" element={<Auth/>}/>
        <Route path="/404" element={<NotFound />}/>
        <Route path="*" element={<Navigate to="/dashboard/home" replace/>}/>
    </Routes>
  );
}

export default App;

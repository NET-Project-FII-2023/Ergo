import { Routes, Route, Navigate } from "react-router-dom";
import { Dashboard, Auth } from "./layouts";
import LoginRequired from "./context/LoginRequired";
import { LocalizationProvider } from '@mui/x-date-pickers';
import { AdapterDayjs } from '@mui/x-date-pickers/AdapterDayjs'

function App() {
  return (
    <LocalizationProvider dateAdapter={AdapterDayjs}>
      <Routes>
          <Route element={<LoginRequired/>}>
              <Route path="/dashboard/*" element={<Dashboard/>}/>
          </Route>
          <Route path="/auth/*" element={<Auth/>}/>
          <Route path="*" element={<Navigate to="/dashboard/home" replace/>}/>
      </Routes>
    </LocalizationProvider>
  );
}

export default App;

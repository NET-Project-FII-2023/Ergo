import { Routes, Route, Navigate } from "react-router-dom";
import { Dashboard, Auth } from "@/layouts";
import LoginRequired from "@/context/LoginRequired.jsx";
import { createTheme } from '@mui/material/styles';

export  const myTheme = createTheme({
  palette: {
    primary: {
      light: '#757ce8',
      main: '#3f50b5',
      dark: '#002884',
      contrastText: '#fff',
    },
    secondary: {
      light: '#ff7961',
      main: '#f44336',
      dark: '#ba000d',
      contrastText: '#000',
    },
  },
});

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

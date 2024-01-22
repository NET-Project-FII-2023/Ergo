import React from 'react';
import './App.css';
import {BrowserRouter, Route, Routes} from "react-router-dom";
import Home from "./pages/Home/Home";
import NotFound from "./pages/NotFound/NotFound";
import LoginRequired from "./layouts/LoginRequired/LoginRequired";
import Login from "./pages/Login/Login";

function App() {

  return (
      <BrowserRouter>
          <Routes>
              {/* Public Routes */}
              <Route path="/login" element={<Login />} />

              {/* Protected Routes */}
              <Route element={<LoginRequired />}>
                  <Route path="/" element={<Home />} />
                  <Route path="/home" element={<Home />} />
              </Route>

              {/* Public Routes */}
              <Route path="*" element={<NotFound />} />
          </Routes>
      </BrowserRouter>
  );
}

export default App;

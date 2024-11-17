import React from "react";
import { BrowserRouter, Route, Routes, Navigate } from "react-router-dom";
import "./App.css";
import Dashboard from "./pages/Dashboard";
import Login from "./pages/Login";
import { useAuth } from "./context/AuthContext";
import Header from "./components/Header";
import { Typography } from "@mui/material";

interface IAppProps {
  appTitle: string;
}

const App: React.FC<IAppProps> = ({ appTitle }) => {
  const { auth } = useAuth();

  return (
    <div className="App">
      <Typography variant="h3" component="h1">
        {appTitle}
      </Typography>

      <BrowserRouter
        future={{
          v7_startTransition: true,
          v7_relativeSplatPath: true,
        }}
      >
        <Header />
        <Routes>
          <Route element={<Login />} path="/login" />
          <Route
            element={
              auth?.isAuthenticated ? (
                <Dashboard authorised={true} name={auth.name} />
              ) : (
                <Navigate to="/Login" />
              )
            }
            path="/Dashboard"
          />
          <Route path="/" element={<Navigate to="/Dashboard" />} />
        </Routes>
      </BrowserRouter>
    </div>
  );
};

export default App;

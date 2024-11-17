import React, { createContext, useState, useContext, useEffect } from "react";
import {RoleType} from '../Common/constants';

export interface AuthData {
  token: string;
  username: string;
  name: string;
  role: RoleType,
  isAuthenticated: boolean;
}

interface AuthContextType {
  auth: AuthData | null;
  setAuth: (auth: AuthData | null) => void;
}

const AuthContext = createContext<AuthContextType | null>(null);

export const AuthProvider: React.FC<{ children: React.ReactNode }> = ({
  children,
}) => {
  const [authData, setAuthData] = useState<AuthData | null>(() => {
    const storedAuth = localStorage.getItem("authData");
    return storedAuth ? JSON.parse(storedAuth) : null; //Lazy Initialization run only once during initialization
  });

  useEffect(() => {
    if (authData) {
      localStorage.setItem("authData", JSON.stringify(authData));
    } else {
      localStorage.removeItem("authData");
    }
  }, [authData]);

  return (
    <AuthContext.Provider value={{ auth: authData, setAuth: setAuthData }}>
      {children}
    </AuthContext.Provider>
  );
};

// Custom hook for easy access
export const useAuth = () => {
  const context = useContext(AuthContext);
  if (!context) {
    throw new Error("useAuth must be used within an AuthProvider");
  }
  return context;
};
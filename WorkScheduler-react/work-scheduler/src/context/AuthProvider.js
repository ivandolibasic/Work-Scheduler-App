import React, { createContext, useState, useContext, useEffect, useRef } from 'react';
import http from '../http-common';

const AuthContext = React.createContext();

export function useAuth() {
  return useContext(AuthContext);
}

export default function AuthProvider({children}) {
    const [userLogedIn, setUserLogedIn] = useState();
    const [username, setUsername] = useState('');
    const [accessLevel, setAccessLevel] = useState('');
    const dataFetchedRef = useRef(false);
    

    useEffect(()=>{
      if(dataFetchedRef.current) return;
      dataFetchedRef.current = true;
      setUser();
    },[]);

    const setUser = () => {
      
      var token = localStorage.getItem('access_token');
      if (token) {
        setUsername(localStorage.getItem('username'));
        setAccessLevel(localStorage.getItem('accessLevel'));
        setUserLogedIn(true);
      }else{
        setUserLogedIn(false);
      }
    }


    function LogIn(){
      setUser();
    }

    function LogOut(){
      setUserLogedIn(false);
      setUsername('');
      setAccessLevel('');
      localStorage.setItem("username", '');
      localStorage.setItem("accessLevel", '');
      localStorage.setItem("access_token", '');
      http.defaults.headers.common['Authorization']='';
    }
    const value={userLogedIn, LogIn, LogOut, username, accessLevel};
  return (
    <AuthContext.Provider value={value}>
      {children}
    </AuthContext.Provider>
  )
}

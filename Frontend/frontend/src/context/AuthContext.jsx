// src/context/AuthContext.jsx
import React, { createContext, useState, useEffect } from 'react';
import * as authApi from '../api/auth';
import { decodeToken } from '../utils/jwt';

export const AuthContext = createContext();

export function AuthProvider({ children }) {
  const [tokens, setTokens] = useState(() => {
    const stored = localStorage.getItem('tokens');
    return stored ? JSON.parse(stored) : null;
  });
  const [user, setUser] = useState(null);

  const ROLE_CLAIM   = 'http://schemas.microsoft.com/ws/2008/06/identity/claims/role';
  const ID_CLAIM     = 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier';
  const NAME_CLAIM   = 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name';
  const EMAIL_CLAIM  = 'email';

  function refreshUserFromToken(accessToken) {
    const decoded = decodeToken(accessToken);
    console.log('Decoded JWT payload:', decoded);
    if (!decoded) return null;

    
    let roles = [];
    if (decoded[ROLE_CLAIM]) {
      roles = Array.isArray(decoded[ROLE_CLAIM])
                ? decoded[ROLE_CLAIM]        
                : [decoded[ROLE_CLAIM]];     
    }

    return {
      id:    decoded[ID_CLAIM]   || null,
      name:  decoded[NAME_CLAIM] || null,
      email: decoded[EMAIL_CLAIM]|| null,
      roles
    };
  }

  useEffect(() => {
    if (tokens) {
      localStorage.setItem('tokens', JSON.stringify(tokens));
      localStorage.setItem('accessToken',  tokens.accessToken);
      localStorage.setItem('refreshToken', tokens.refreshToken);
      setUser(refreshUserFromToken(tokens.accessToken));
    } else {
      localStorage.clear();
      setUser(null);
    }
  }, [tokens]);

  const login    = async creds => { const d = await authApi.login(creds); setTokens(d); return d; };
  const register = creds => authApi.register(creds);
  const logout   = () => setTokens(null);

  return (
    <AuthContext.Provider value={{ tokens, user, login, register, logout }}>
      {children}
    </AuthContext.Provider>
  );
}

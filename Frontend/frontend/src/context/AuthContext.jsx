// src/hooks/AuthProvider.jsx
import React, { createContext, useState, useEffect, useCallback } from 'react';
import PropTypes from 'prop-types';
import client from '../api/client';
import * as authApi from '../api/auth';
import { decodeToken } from '../utils/jwt';

export const AuthContext = createContext();

export function AuthProvider({ children }) {
  const [tokens, setTokens] = useState(() => {
    const stored = localStorage.getItem('tokens');
    return stored ? JSON.parse(stored) : null;
  });
  const [user, setUser] = useState(null);
  const [loading, setLoading] = useState(true);

  const ROLE  = 'http://schemas.microsoft.com/ws/2008/06/identity/claims/role';
  const ID    = 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier';
  const NAME  = 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name';
  const EMAIL = 'email';

  /* tokens → localStorage + user ----------------------------------------- */
  useEffect(() => {
    if (tokens) {
      localStorage.setItem('tokens', JSON.stringify(tokens));
      localStorage.setItem('accessToken', tokens.accessToken);
      setUser(refreshUserFromToken(tokens.accessToken));
    } else {
      localStorage.removeItem('tokens');
      localStorage.removeItem('accessToken');
      setUser(null);
    }
  }, [tokens]);

  /* sayfa yenilendiğinde refresh-cookie varsa access token yenile --------- */
  useEffect(() => {
    (async () => {
      if (document.cookie.includes('refreshToken=')) {
        try {
          const { accessToken } = await authApi.refreshToken();
          setTokens({ accessToken, refreshToken: null });
        } catch {
          doLogout();
        }
      }
      setLoading(false);
    })();
  }, []);

  function refreshUserFromToken(at) {
    const d = decodeToken(at);
    if (!d) return null;
    const roles = d[ROLE] ? (Array.isArray(d[ROLE]) ? d[ROLE] : [d[ROLE]]) : [];
    return {
      id:    d[ID]    || null,
      name:  d[NAME]  || null,
      email: d[EMAIL] || null,
      roles
    };
  }

  /* ----------------------- PUBLIC API ----------------------------------- */
  const login = async creds => {
    const { accessToken } = await authApi.login(creds);
    if (!accessToken)
      throw new Error('Login işlemi başarılı, ama accessToken alınamadı.');
    setTokens({ accessToken, refreshToken: null });
  };

  /** Register - accessToken dönmüyor, yalnızca mesaj bekleniyor */
  const register = async info => {
    const data = await authApi.register(info);   // { message: "..." }
    return data;
  };

  const doLogout = useCallback(async () => {
    try { await client.post('/auth/logout'); } catch {}
    setTokens(null);
  }, []);

  if (loading) return <div>Yükleniyor…</div>;

  return (
    <AuthContext.Provider value={{
      tokens,
      user,
      login,
      register,
      logout: doLogout,
      isAuthenticated: !!tokens?.accessToken
    }}>
      {children}
    </AuthContext.Provider>
  );
}

AuthProvider.propTypes = { children: PropTypes.node.isRequired };

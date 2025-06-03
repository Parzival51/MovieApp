import React, { createContext, useState, useEffect, useCallback } from 'react';
import PropTypes from 'prop-types';
import client from '../api/client';
import * as authApi from '../api/auth';
import { decodeToken } from '../utils/jwt';

export const AuthContext = createContext();

export function AuthProvider({ children }) {
  const [tokens, setTokens] = useState(() => {
    // localStorage’da yalnızca accessToken saklıyoruz
    const stored = localStorage.getItem('tokens');
    return stored ? JSON.parse(stored) : null;
  });
  const [user, setUser]     = useState(null);
  const [loading, setLoading] = useState(true);

  const ROLE_CLAIM  = 'http://schemas.microsoft.com/ws/2008/06/identity/claims/role';
  const ID_CLAIM    = 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/nameidentifier';
  const NAME_CLAIM  = 'http://schemas.xmlsoap.org/ws/2005/05/identity/claims/name';
  const EMAIL_CLAIM = 'email';

  // Her tokens değiştiğinde localStorage ve user güncellensin:
  useEffect(() => {
    if (tokens) {
      localStorage.setItem('tokens', JSON.stringify(tokens));
      localStorage.setItem('accessToken', tokens.accessToken);
      // refreshToken artık HttpOnly cookie’de duruyor, localStorage’a yazmaya gerek yok.
      setUser(refreshUserFromToken(tokens.accessToken));
    } else {
      localStorage.removeItem('tokens');
      localStorage.removeItem('accessToken');
      setUser(null);
    }
  }, [tokens]);

  // Sayfa yüklendiğinde eğer cookie’de refreshToken varsa yeniletme denemesi yap:
  useEffect(() => {
    async function tryRefresh() {
      if (document.cookie.includes('refreshToken=')) {
        try {
          const { accessToken: newAccess } = await authApi.refreshToken();
          setTokens(prev => ({ ...prev, accessToken: newAccess }));
        } catch (err) {
          doLogout();
        }
      }
      setLoading(false);
    }
    tryRefresh();
  }, []);

  function refreshUserFromToken(accessToken) {
    const decoded = decodeToken(accessToken);
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

  const login = async ({ email, password }) => {
    const d = await authApi.login({ email, password });
    // API login endpoint’inden yalnızca accessToken bekliyoruz;
    // refreshToken, HttpOnly cookie’ye düşmüş olacak.
    if (d?.accessToken) {
      setTokens({ accessToken: d.accessToken, refreshToken: null });
      return d;
    } else {
      throw new Error('Login işlemi başarılı, ama accessToken alınamadı.');
    }
  };

  const register = async ({ userName, displayName, email, password }) => {
    const d = await authApi.register({ userName, displayName, email, password });
    if (d?.accessToken) {
      setTokens({ accessToken: d.accessToken, refreshToken: null });
      return d;
    } else {
      throw new Error('Kayıt başarılı, ama accessToken alınamadı.');
    }
  };

  const doLogout = useCallback(async () => {
    try {
      await client.post('/auth/logout');
    } catch (e) {
      console.warn('Logout isteği başarısız olabilir:', e);
    } finally {
      setTokens(null);
    }
  }, []);

  if (loading) {
    return <div>Yükleniyor…</div>;
  }

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

AuthProvider.propTypes = {
  children: PropTypes.node.isRequired
};

// src/components/PrivateRoute.jsx
import React from 'react';
import { Navigate, Outlet } from 'react-router-dom';
import useAuth from '../hooks/useAuth';

export default function PrivateRoute({ allowedRoles }) {
  const { tokens, user } = useAuth();

  // Giriş yoksa login’e
  if (!tokens?.accessToken) {
    return <Navigate to="/login" replace />;
  }

  // Rol kontrolü gerekiyorsa
  if (
    allowedRoles &&
    Array.isArray(user?.roles) &&
    !allowedRoles.some(r => user.roles.includes(r))
  ) {
    return <Navigate to="/" replace />;
  }

  // Her şey tamam, alt rotaları render et
  return <Outlet />;
}

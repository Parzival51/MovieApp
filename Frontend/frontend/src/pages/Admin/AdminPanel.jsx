// src/pages/AdminPanel.jsx
import React from 'react';
import { useNavigate } from 'react-router-dom';
import useAuth from '../../hooks/useAuth';

export default function AdminPanel() {
  const { logout } = useAuth();
  const navigate = useNavigate();

  const goHome = () => {
    navigate('/', { replace: true });
  };

  const handleLogout = () => {
    logout();
    navigate('/login', { replace: true });
  };

  return (
    <div className="container mx-auto px-4 py-6">
      {/* Üst navbar */}
      <div className="flex justify-between items-center mb-6">
        <h1 className="text-3xl font-bold">Admin Paneli</h1>
        <div className="space-x-4">
          <button
            onClick={goHome}
            className="px-3 py-1 bg-gray-200 rounded hover:bg-gray-300"
          >
            Anasayfa
          </button>
          <button
            onClick={handleLogout}
            className="px-3 py-1 bg-red-500 text-white rounded hover:bg-red-600"
          >
            Çıkış Yap
          </button>
        </div>
      </div>

      {/* Yönetim kartları */}
      <div className="grid grid-cols-1 md:grid-cols-2 lg:grid-cols-4 gap-6">
        <button
          onClick={() => navigate('/admin/movies')}
          className="p-6 rounded-lg shadow-sm hover:shadow-md transition
                     bg-surface text-surface-900 dark:text-surface-100"
        >
          Film Yönetimi
        </button>
        <button
          onClick={() => navigate('/admin/genres')}
          className="p-6 rounded-lg shadow-sm hover:shadow-md transition
                     bg-surface text-surface-900 dark:text-surface-100"
        >
          Tür Yönetimi
        </button>
        <button
          onClick={() => navigate('/admin/actors')}
          className="p-6 rounded-lg shadow-sm hover:shadow-md transition
                     bg-surface text-surface-900 dark:text-surface-100"
        >
          Oyuncu Yönetimi
        </button>
        <button
          onClick={() => navigate('/admin/users')}
          className="p-6 rounded-lg shadow-sm hover:shadow-md transition
                     bg-surface text-surface-900 dark:text-surface-100"
        >
          Kullanıcı Yönetimi
        </button>
        <button
          onClick={() => navigate('/admin/logs')}
          className="p-6 rounded-lg shadow-sm hover:shadow-md transition
                     bg-surface text-surface-900 dark:text-surface-100"
        >
          Aktivite Logları
        </button>
        {/* Yeni eklenen moderator sayfalarına linkler */}
        <button
          onClick={() => navigate('/moderator/reviews')}
          className="p-6 rounded-lg shadow-sm hover:shadow-md transition
                     bg-surface text-surface-900 dark:text-surface-100"
        >
          İnceleme Onayları
        </button>
        
      </div>
    </div>
  );
}

// src/pages/DirectorListPage.jsx
import React from 'react';
import { useNavigate } from 'react-router-dom';
import useFetch from '../../hooks/useFetch';
import { getAllDirectors } from '../../api/directors';
import DirectorCard from '../../components/DirectorCard';
import Header from '../../components/Header';

export default function DirectorListPage() {
  const navigate = useNavigate();
  const { data: raw = [], loading, error } = useFetch(() => getAllDirectors(), []);
  const directors = Array.isArray(raw) ? raw : [];

  if (loading) return <div className="py-20 text-center">Yükleniyor…</div>;
  if (error)  return <div className="py-20 text-center text-red-500">Hata: {error.message}</div>;

  return (
    <div className="container mx-auto px-4 py-0 space-y-6">
      <Header />
      <h1 className="text-3xl font-bold mb-4">Yönetmenler</h1>
      <div className="grid grid-cols-2 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-5 gap-6">
        {directors.map(director => (
          <div
            key={director.id}
            onClick={() => navigate(`/directors/${director.id}`)}
            className="cursor-pointer"
          >
            <DirectorCard director={director} />
          </div>
        ))}
      </div>
    </div>
  );
}

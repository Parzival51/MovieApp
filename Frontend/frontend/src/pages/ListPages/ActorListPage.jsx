import React from 'react';
import { useNavigate } from 'react-router-dom';
import useFetch from '../../hooks/useFetch';
import { getAllActors } from '../../api/actors';
import ActorCard from '../../components/ActorCard';
import Header from '../../components/Header';

export default function ActorListPage() {
  const navigate = useNavigate();
  const { data: raw = [], loading, error } = useFetch(() => getAllActors(), []);
  const actors = Array.isArray(raw) ? raw : [];

  if (loading) return <div className="py-20 text-center">Yükleniyor…</div>;
  if (error)  return <div className="py-20 text-center text-red-500">Hata: {error.message}</div>;

  return (
    <div className="container mx-auto px-4 py-0 space-y-6">
      <Header />
      <h1 className="text-3xl font-bold mb-4">Aktörler</h1>
      <div className="grid grid-cols-2 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-5 gap-6">
        {actors.map(actor => (
          <div
            key={actor.id}
            onClick={() => navigate(`/actors/${actor.id}`)}
            className="cursor-pointer"
          >
            <ActorCard actor={actor} />
          </div>
        ))}
      </div>
    </div>
  );
}

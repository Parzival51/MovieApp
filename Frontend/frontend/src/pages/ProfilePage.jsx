import React, { useState } from 'react';
import { useSearchParams, Navigate } from 'react-router-dom';
import useAuth from '../hooks/useAuth';
import FavoritesList from '../components/FavoritesList';
import WatchList from '../components/WatchList';

const TABS = ['info', 'favorites', 'watch'];

export default function ProfilePage() {
  const { user } = useAuth();
  const [params, setParams] = useSearchParams();
  const current = params.get('tab') ?? 'info';
  const [tab, setTab] = useState(TABS.includes(current) ? current : 'info');

  const change = t => { setTab(t); setParams({ tab: t }); };

  if (!user) return <div className="container mx-auto px-4 py-10">Yükleniyor…</div>;

  return (
    <div className="container mx-auto px-4 py-10 space-y-8">
      <h1 className="text-3xl font-bold">Profilim</h1>

      {/* Sekmeler */}
      <div className="flex gap-6 border-b border-muted/30 overflow-x-auto">
        {[
          ['info',        'Genel Bilgi'],
          ['favorites',   `Favoriler`],
          ['watch',       `İzlenecekler`],
        ].map(([key,label])=>(
          <button
            key={key}
            onClick={()=>change(key)}
            className={`pb-2 font-medium ${
              tab===key ? 'border-b-2 border-accent text-accent' : 'text-muted'
            }`}
          >
            {label}
          </button>
        ))}
      </div>

      {/* İçerik */}
      {tab==='info' && (
        <div className="bg-surface p-6 rounded shadow max-w-md space-y-2">
          <div><strong>ID:</strong> {user.id}</div>
          <div><strong>İsim:</strong> {user.name || '—'}</div>
          <div><strong>E-posta:</strong> {user.email || '—'}</div>
          <div><strong>Roller:</strong> {user.roles?.join(', ') || '—'}</div>
        </div>
      )}
      {tab==='favorites' && <FavoritesList />}
      {tab==='watch'      && <WatchList />}
    </div>
  );
}

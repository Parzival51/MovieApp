import React from 'react';
import { useNavigate } from 'react-router-dom';
import useFetch from '../hooks/useFetch';
import { getMyWatchlist } from '../api/watchlists';

export default function WatchList() {
  const { data, loading, error } = useFetch(getMyWatchlist, []);
  const navigate = useNavigate();
  const list = Array.isArray(data) ? data : [];

  if (loading) return <p>Yükleniyor…</p>;
  if (error)   return <p className="text-danger">{error.message}</p>;
  if (!list.length) return <p>İzleme listeniz boş.</p>;

  return (
    <div className="grid grid-cols-1 sm:grid-cols-2 lg:grid-cols-3 gap-4">
      {list.map(item => (
        <button
          key={item.id}
          onClick={() => navigate(`/movies/${item.movieId}`)}
          className="flex items-center gap-4 bg-surface p-3 rounded shadow hover:shadow-lg text-left"
        >
          <img
            src={item.posterUrl}
            alt={item.movieTitle}
            className="w-16 h-24 object-cover rounded"
          />
          <div>
            <h4 className="font-semibold">{item.movieTitle}</h4>
            <span className="text-xs text-muted">
              {new Date(item.addedAt).toLocaleDateString('tr-TR')}
            </span>
          </div>
        </button>
      ))}
    </div>
  );
}

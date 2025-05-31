// src/features/home/NewReleases.jsx
import React from 'react';
import useFetch from '../../hooks/useFetch';
import { getNewReleases } from '../../api/movies';
import MovieCard from '../../components/MovieCard';

export default function NewReleases({ genre }) {
  const { data: movies = [], loading, error } =
    useFetch(() => getNewReleases(), [genre]);

  const list = Array.isArray(movies) ? movies : [];

  const twoYearsAgo = new Date();
  twoYearsAgo.setFullYear(twoYearsAgo.getFullYear() - 2);

  const recent = list.filter(m => {
    const rd = new Date(m.releaseDate);
    return rd >= twoYearsAgo;
  });

  if (loading) {
    return (
      <div className="py-6 text-center text-gray-300 dark:text-gray-500">
        Yükleniyor…
      </div>
    );
  }

  if (error) {
    return (
      <div className="py-6 text-center text-danger">
        Yeni çıkanlar yüklenemedi.
      </div>
    );
  }

  if (recent.length === 0) {
    return (
      <div className="py-6 text-center text-gray-400 dark:text-gray-600">
        Son iki yıl içinde çıkan film yok.
      </div>
    );
  }

  return (
    <div className="grid grid-cols-1 sm:grid-cols-2 md:grid-cols-4 gap-6">
      {recent.map(m => (
        <MovieCard key={m.id} movie={m} />
      ))}
    </div>
  );
}

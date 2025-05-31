// src/features/home/TopRatedMovies.jsx
import React, { useState, useMemo } from 'react';
import useFetch from '../../hooks/useFetch';
import { getAllMovies } from '../../api/movies';
import Carousel from '../../components/Carousel';
import MovieCard from '../../components/MovieCard';

export default function TopRatedMovies() {
  const { data: movies = [], loading, error } =
    useFetch(() => getAllMovies(), []);

  const selected = useMemo(() => {
    if (!Array.isArray(movies) || movies.length === 0) return [];
    const allSame = movies.every(m => m.averageRating === movies[0].averageRating);
    if (allSame) {
      return [...movies].sort(() => Math.random() - 0.5).slice(0, 5);
    } else {
      return [...movies]
        .sort((a, b) => b.averageRating - a.averageRating)
        .slice(0, 5);
    }
  }, [movies]);

  const [current, setCurrent] = useState(0);
  const prev = () =>
    setCurrent(i => (i === 0 ? selected.length - 1 : i - 1));
  const next = () =>
    setCurrent(i => (i === selected.length - 1 ? 0 : i + 1));

  if (loading) {
    return (
      <div className="py-10 text-center text-gray-300 dark:text-gray-500">
        Yükleniyor…
      </div>
    );
  }

  if (error) {
    return (
      <div className="py-10 text-center text-accent">
        Hata: {error.message}
      </div>
    );
  }

  if (selected.length === 0) return null;

  return (
    <Carousel
      items={selected}
      current={current}
      prev={prev}
      next={next}
      visibleCount={4}
      renderItem={m => <MovieCard key={m.id} movie={m} />}
    />
  );
}

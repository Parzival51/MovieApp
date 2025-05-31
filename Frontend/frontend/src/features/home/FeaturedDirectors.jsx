// src/features/home/FeaturedDirectors.jsx
import React, { useState, useEffect } from 'react';
import useFetch from '../../hooks/useFetch';
import {
  getAllDirectors,
  getDirectorAverageRating
} from '../../api/directors';
import Carousel from '../../components/Carousel';
import DirectorCard from '../../components/DirectorCard';

export default function FeaturedDirectors() {
  const { data: raw = [], loading, error } =
    useFetch(() => getAllDirectors(), []);
  const directors = Array.isArray(raw) ? raw : [];

  const sample20 = directors
    .slice()
    .sort(() => Math.random() - 0.5)
    .slice(0, 20);

  const [withScore, setWithScore] = useState([]);

  useEffect(() => {
    if (sample20.length === 0) {
      setWithScore([]);
      return;
    }

    let cancelled = false;
    (async () => {
      const list = await Promise.all(
        sample20.map(async dir => {
          try {
            const avg = await getDirectorAverageRating(dir.id);
            return { ...dir, avgRating: avg ?? 0 };
          } catch {
            return { ...dir, avgRating: 0 };
          }
        })
      );
      if (!cancelled) setWithScore(list);
    })();

    return () => { cancelled = true };
  }, [sample20.length]);

  const top10 = withScore
    .slice()
    .sort((a, b) =>
      (b.popularity ?? 0) + (b.avgRating ?? 0)
      - ((a.popularity ?? 0) + (a.avgRating ?? 0))
    )
    .slice(0, 10);

  const [current, setCurrent] = useState(0);
  const prev = () => setCurrent(i => i === 0 ? top10.length - 1 : i - 1);
  const next = () => setCurrent(i => i === top10.length - 1 ? 0 : i + 1);

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
        Hata: {error.message}
      </div>
    );
  }

  if (top10.length === 0) return null;

  return (
    <Carousel
      items={top10}
      current={current}
      prev={prev}
      next={next}
      visibleCount={6}
      renderItem={director => (
        <DirectorCard
          key={director.id}
          director={director}
        />
      )}
    />
  );
}

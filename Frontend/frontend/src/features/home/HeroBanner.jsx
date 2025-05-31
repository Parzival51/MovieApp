// src/features/home/HeroBanner.jsx
import React, { useEffect, useState } from 'react';
import { HiChevronLeft, HiChevronRight } from 'react-icons/hi';
import { motion, useReducedMotion } from 'framer-motion';
import { getFeaturedMovies } from '../../api/movies';

export default function HeroBanner() {
  const [movies, setMovies]   = useState([]);
  const [current, setCurrent] = useState(0);
  const [error,   setError]   = useState(null);
  const reduce    = useReducedMotion();

  useEffect(() => {
    getFeaturedMovies(5)
      .then(setMovies)
      .catch(() => setError('Filmler yüklenirken hata oluştu.'));
  }, []);

  const prev = () => setCurrent(i => (i === 0 ? movies.length - 1 : i - 1));
  const next = () => setCurrent(i => (i === movies.length - 1 ? 0 : i + 1));

  if (error)        return <div className="p-4 bg-danger/10 text-danger">{error}</div>;
  if (!movies.length) return <div className="h-96 flex items-center justify-center text-muted">Yükleniyor…</div>;

  const { title, releaseDate, backdropUrl, posterUrl, description = '' } = movies[current];
  const src      = backdropUrl || posterUrl;
  const summary  = description.length > 160 ? description.slice(0, 157) + '…' : description;

  return (
    <div className="relative h-[70vh] min-h-[28rem] overflow-hidden bg-black">
      {/* arka görsel */}
      <motion.img
        key={src}
        src={src}
        alt={title}
        initial={reduce ? false : { opacity: 0.4, scale: 1.05 }}
        animate={{ opacity: 1, scale: 1 }}
        transition={{ duration: reduce ? 0 : 1 }}
        className="absolute inset-0 h-full w-full object-contain object-center"
      />

      {/* gradientler */}
      <div className="absolute inset-0 bg-gradient-to-t from-black/80 via-black/20 to-transparent" />
      <div className="absolute inset-0 bg-gradient-to-r from-black/75 via-transparent to-transparent" />

      {/* içerik bloğu */}
      <div className="relative z-10 flex h-full items-end pb-12">
        <div className="container mx-auto px-4 md:px-6 lg:px-8">
          <motion.h1
            key={title}
            initial={reduce ? false : { opacity: 0, x: -40 }}
            animate={{ opacity: 1, x: 0 }}
            transition={{ duration: 0.6 }}
            className="text-3xl sm:text-5xl md:text-6xl font-heading font-bold text-white drop-shadow-lg"
          >
            {title}
          </motion.h1>

          {summary && (
            <p className="mt-3 hidden md:block max-w-xl text-sm leading-relaxed text-gray-200/90 line-clamp-3">
              {summary}
            </p>
          )}
        </div>
      </div>

      {/* tarih rozeti */}
      <div className="absolute top-4 right-4 rounded bg-black/70 px-3 py-1 text-xs font-medium text-white backdrop-blur">
        {new Date(releaseDate).toLocaleDateString('tr-TR', {
          year: 'numeric',
          month: 'long',
          day: 'numeric',
        })}
      </div>

      {/* navigasyon okları */}
      <button
        onClick={prev}
        aria-label="Önceki"
        className="absolute left-6 top-1/2 -translate-y-1/2 z-20 rounded-full bg-black/60 p-3 text-white
                   hover:bg-black/80 focus:outline-none focus:ring-2 focus:ring-accent"
      >
        <HiChevronLeft size={24} />
      </button>
      <button
        onClick={next}
        aria-label="Sonraki"
        className="absolute right-6 top-1/2 -translate-y-1/2 z-20 rounded-full bg-black/60 p-3 text-white
                   hover:bg-black/80 focus:outline-none focus:ring-2 focus:ring-accent"
      >
        <HiChevronRight size={24} />
      </button>
    </div>
  );
}

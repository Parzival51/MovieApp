import React, { useRef } from 'react';
import useFetch from '../../hooks/useFetch';
import { getAllGenres } from '../../api/genres';
import PropTypes from 'prop-types';
import { HiChevronLeft, HiChevronRight } from 'react-icons/hi';

export default function GenreFilter({ selectedGenre, onSelect }) {
  const { data, loading, error } = useFetch(getAllGenres, []);
  const genres = Array.isArray(data) ? data : [];

  const scroller = useRef(null);
  const scrollBy = dir => {
    const el = scroller.current;
    if (!el) return;
    el.scrollBy({ left: dir * 200, behavior: 'smooth' });
  };

  if (loading) return null;
  if (error)
    return (
      <div className="py-4 text-center text-danger">
        Türler yüklenemedi.
      </div>
    );

  const base        = 'px-4 py-2 rounded-full font-medium transition whitespace-nowrap';
  const selectedCls = 'bg-secondary text-black';
  const defaultCls  = 'bg-muted/20 hover:bg-muted/40 text-surface-700 dark:text-surface-200';

  return (
    <div className="relative">
      {/* sola/sağa oklar (md ve üzeri görünür) */}
      <button
        onClick={() => scrollBy(-1)}
        className="hidden md:flex absolute left-0 top-1/2 -translate-y-1/2 bg-black/40 text-white p-1 rounded-full z-10"
      >
        <HiChevronLeft />
      </button>
      <button
        onClick={() => scrollBy(1)}
        className="hidden md:flex absolute right-0 top-1/2 -translate-y-1/2 bg-black/40 text-white p-1 rounded-full z-10"
      >
        <HiChevronRight />
      </button>

      <div
        ref={scroller}
        className="flex gap-3 overflow-x-auto scrollbar-hide py-2 snap-x"
      >
        <button
          onClick={() => onSelect(null)}
          className={`${base} ${selectedGenre === null ? selectedCls : defaultCls}`}
        >
          Tümü
        </button>
        {genres.map(g => (
          <button
            key={g.id}
            onClick={() => onSelect(String(g.id))}
            className={`${base} ${selectedGenre === String(g.id) ? selectedCls : defaultCls}`}
          >
            {g.name}
          </button>
        ))}
      </div>
    </div>
  );
}

GenreFilter.propTypes = {
  selectedGenre: PropTypes.string,
  onSelect:      PropTypes.func.isRequired
};

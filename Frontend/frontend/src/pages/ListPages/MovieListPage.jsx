import React, { useCallback } from 'react';
import { useSearchParams, useNavigate } from 'react-router-dom';
import useFetch from '../../hooks/useFetch';
import usePagination from '../../hooks/usePagination';
import {
  searchMovies,
  getByGenre,
  getAllMovies
} from '../../api/movies';
import GenreFilter from '../../features/home/GenreFilter';
import MovieCard from '../../components/MovieCard';
import Header from '../../components/Header';

export default function MovieListPage() {
  const [searchParams, setSearchParams] = useSearchParams();
  const navigate = useNavigate();

  const page     = parseInt(searchParams.get('page') || '1', 10);
  const q        = searchParams.get('q')     || '';
  const genreId  = searchParams.get('genre') || null;
  const pageSize = 20;

  const fetcher = useCallback(() => {
    if (q) {
      return searchMovies(q, page, pageSize);
    }
    if (genreId) {
      return getByGenre(genreId, page, pageSize);
    }
    return getAllMovies().then(arr => ({
      items: Array.isArray(arr) ? arr : [],
      total: Array.isArray(arr) ? arr.length : 0
    }));
  }, [q, genreId, page]);

  const { data: result, loading, error } = useFetch(fetcher, [fetcher]);
  const { items = [], total = 0 } = result ?? {};
  const list = Array.isArray(items) ? items : [];
  const totalItems = total || list.length;

  const { currentPage, totalPages, setPage } =
    usePagination(totalItems, pageSize, page);

  const changePage = p => {
    setSearchParams(params => {
      params.set('page', String(p));
      return params;
    });
    setPage(p);
    window.scrollTo(0, 0);
  };

  const handleGenreSelect = gId => {
    setSearchParams(params => {
      if (gId)  params.set('genre', gId);
      else      params.delete('genre');
      params.delete('q');
      params.set('page', '1');
      return params;
    });
  };

  if (loading) return <div className="py-20 text-center">Yükleniyor…</div>;
  if (error)
    return (
      <div className="py-20 text-center text-red-500">
        Hata: {error.message}
      </div>
    );

  return (
    <div className="container mx-auto px-4 py-0 space-y-6">
      <Header />
      <h1 className="text-3xl font-bold mb-4">Filmler</h1>

      {/* — filtre çubuğu  */}
       <div className="bg-background/30 dark:bg-primary/30 pt-2 pb-1 -mx-4 px-4 shadow-sm">
         <GenreFilter
           selectedGenre={genreId}
           onSelect={handleGenreSelect}
         />
       </div>

      <div className="grid grid-cols-2 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-5 gap-6">
        {list.map(m => (
          <div
            key={m.id}
            onClick={() => navigate(`/movies/${m.id}`)}
            className="cursor-pointer"
          >
            <MovieCard movie={m} />
          </div>
        ))}
      </div>

      {/* — sayfalama — */}
      {totalPages > 1 && (
        <div className="flex justify-center items-center space-x-4 mt-6">
          <button
            onClick={() => changePage(currentPage - 1)}
            disabled={currentPage <= 1}
            className="px-4 py-2 bg-gray-200 rounded disabled:opacity-50"
          >
            Önceki
          </button>
          <span>
            Sayfa {currentPage} / {totalPages}
          </span>
          <button
            onClick={() => changePage(currentPage + 1)}
            disabled={currentPage >= totalPages}
            className="px-4 py-2 bg-gray-200 rounded disabled:opacity-50"
          >
            Sonraki
          </button>
        </div>
      )}
    </div>
  );
}

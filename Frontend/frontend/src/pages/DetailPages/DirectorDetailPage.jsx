// src/pages/DirectorDetailPage.jsx
import React from 'react';
import { useParams } from 'react-router-dom';
import useFetch from '../../hooks/useFetch';
import {
  getDirectorById,
  getMoviesByDirector,
  rateDirector,
  getDirectorAverageRating
} from '../../api/directors';
import Card from '../../components/Card';
import ProgressiveImage from '../../components/ProgressiveImage';
import MovieCard from '../../components/MovieCard';
import Header from '../../components/Header';
import TenStarRating from '../../components/TenStarRating';
import useAuth from '../../hooks/useAuth';
import { useNotification } from '../../context/NotificationContext';

export default function DirectorDetailPage() {
  const { id } = useParams();
  const { tokens } = useAuth();
  const notify = useNotification();

  const {
    data: director,
    loading: dLoad,
    error: dErr
  } = useFetch(() => getDirectorById(id), [id]);

  const {
    data: films = [],
    loading: fLoad,
    error: fErr
  } = useFetch(() => getMoviesByDirector(id), [id]);

  const {
    data: avgRating = 0,
    refetch: refetchAvg,
  } = useFetch(() => getDirectorAverageRating(id), [id]);

  const handleRate = async value => {
    if (!tokens) {
      notify.notify('error', 'Lütfen giriş yapın');
      return;
    }
    try {
      await rateDirector(id, value);
      notify.notify('success', 'Oyunuz kaydedildi');
      refetchAvg();
    } catch {
      notify.notify('error', 'Oy verirken hata oluştu');
    }
  };

  if (dLoad) return <div className="py-20 text-center">Yükleniyor…</div>;
  if (dErr)  return <div className="py-20 text-center text-danger">{dErr.message}</div>;
  if (!director) return <div className="py-20 text-center">Yönetmen bulunamadı.</div>;

  const {
    name,
    profilePath: photoUrl,
    birthday,
    placeOfBirth,
    popularity,
    biography
  } = director;

  return (
    <div className="container mx-auto px-4 py-8 space-y-8">
      <Header />

      <div className="flex items-center justify-between">
        <h1 className="text-4xl font-bold">{name}</h1>
        <div className="flex flex-col items-end">
          <TenStarRating
            value={avgRating}
            onChange={handleRate}
            readOnly={!tokens}
          />
          <span className="text-sm text-gray-500 mt-1">
            {avgRating != null ? avgRating.toFixed(1) : '—'} / 10
          </span>
        </div>
      </div>

      <div className="flex flex-col md:flex-row gap-8">
        <div className="w-48 md:w-64">
          <Card hover={false} padding={false}>
            <ProgressiveImage
              src={photoUrl}
              alt={name}
              className="w-full h-full object-cover"
            />
          </Card>
        </div>
        <div className="flex-1 space-y-4 text-sm">
          {birthday && (
            <div>
              <strong>Doğum Tarihi:</strong>{' '}
              {new Date(birthday).toLocaleDateString('tr-TR')}
            </div>
          )}
          {placeOfBirth && (
            <div>
              <strong>Doğum Yeri:</strong> {placeOfBirth}
            </div>
          )}
          <div>
            <strong>TMDb Popülerlik:</strong>{' '}
            {popularity != null ? popularity.toFixed(1) : '—'}
          </div>
        </div>
      </div>

      {/* Biyografi */}
      {biography && (
        <div className="prose prose-lg dark:prose-invert">
          <h2 className="text-2xl font-semibold">Biyografi</h2>
          <p>{biography}</p>
        </div>
      )}

      {/* Filmografi */}
      <section className="space-y-4">
        <h2 className="text-2xl font-semibold">Filmografi</h2>
        {fLoad && <p>Yükleniyor…</p>}
        {fErr && <p className="text-danger">{fErr.message}</p>}
        {!fLoad && !fErr && (!films || films.length === 0) && (
          <p>Hiç film bilgisi yok.</p>
        )}
        <div className="grid grid-cols-2 sm:grid-cols-3 md:grid-cols-4 lg:grid-cols-5 gap-4">
          {(films || []).map(m => (
            <MovieCard
              key={m.id}
              movie={{
                id:            m.id,
                title:         m.title,
                posterUrl:     m.posterUrl,
                averageRating: m.averageRating
              }}
              small
            />
          ))}
        </div>
      </section>
    </div>
  );
}

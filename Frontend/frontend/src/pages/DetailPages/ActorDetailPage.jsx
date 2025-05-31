// src/pages/ActorDetailPage.jsx
import React from 'react';
import { useParams } from 'react-router-dom';
import useFetch from '../../hooks/useFetch';
import { getActorById, getMoviesByActor, rateActor, getActorAverageRating } from '../../api/actors';
import Card from '../../components/Card';
import ProgressiveImage from '../../components/ProgressiveImage';
import MovieCard from '../../components/MovieCard';
import Header from '../../components/Header';
import TenStarRating from '../../components/TenStarRating';
import useAuth from '../../hooks/useAuth';
import { useNotification } from '../../context/NotificationContext';

export default function ActorDetailPage() {
  const { id } = useParams();
  const { tokens } = useAuth();
  const notify = useNotification();

  const {
    data: actor,
    loading: aLoad,
    error: aErr
  } = useFetch(() => getActorById(id), [id]);

  const {
    data: films = [],
    loading: fLoad,
    error: fErr
  } = useFetch(() => getMoviesByActor(id), [id]);

  const {
    data: avgRating = 0,
    refetch: refetchAvg,
  } = useFetch(() => getActorAverageRating(id), [id]);

  const handleRate = async value => {
    if (!tokens) {
      notify.notify('error', 'Lütfen giriş yapın');
      return;
    }
    try {
      await rateActor(id, value);
      notify.notify('success', 'Oyunuz kaydedildi');
      refetchAvg();
    } catch {
      notify.notify('error', 'Oy verirken hata oluştu');
    }
  };

  if (aLoad) return <div className="py-20 text-center">Yükleniyor…</div>;
  if (aErr)   return <div className="py-20 text-center text-danger">{aErr.message}</div>;
  if (!actor) return <div className="py-20 text-center">Oyuncu bulunamadı.</div>;

  const { name, photoUrl, birthday, placeOfBirth, popularity, biography } = actor;

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

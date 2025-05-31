// src/pages/MovieDetailPage.jsx
import React, { useState, useEffect, useCallback, useRef } from 'react';
import { useParams, Link } from 'react-router-dom';
import useFetch            from '../../hooks/useFetch';
import useAuth             from '../../hooks/useAuth';
import { useNotification } from '../../context/NotificationContext';
import Header              from '../../components/Header';
import ProgressiveImage    from '../../components/ProgressiveImage';
import TenStarRating       from '../../components/TenStarRating';
import ReviewCard          from '../../components/ReviewCard';
import ReviewForm          from '../../components/ReviewForm';
import SmallMovieCard      from '../../components/SmallMovieCard';

import { getMovieById, getSimilarMovies } from '../../api/movies';
import { getRatingsByMovie, getMyRating, upsertRating } from '../../api/ratings';
import { getReviewsByMovie, deleteReview }  from '../../api/reviews';
import {
  getCommentsByReview,  // ← EKLENDİ
  addComment,
  deleteComment
} from '../../api/comments';
import {
  getMyFavorites,
  addFavorite,
  deleteFavorite
} from '../../api/favorites';
import {
  getMyWatchlist,
  addWatch,
  deleteWatch
} from '../../api/watchlists';

export default function MovieDetailPage() {
  const { id }     = useParams();
  const { tokens } = useAuth();
  const notify     = useNotification();

  const [showReviews, setShowReviews]   = useState(false);
  const reviewsRef                      = useRef(null);
  const [commentsMap, setCommentsMap]   = useState({});
  const [favEntryId, setFavEntryId]     = useState(null);
  const [watchEntryId, setWatchEntryId] = useState(null);

  const { data: movie, loading: mLoad, error: mErr } = useFetch(
    () => getMovieById(id), [id]
  );
  const {
    data: reviews = [],
    loading: revLoad,
    error: revErr,
    refetch: refetchReviews
  } = useFetch(() => getReviewsByMovie(id), [id]);
  const { data: rawSimilar = [], loading: sLoad, error: sErr } = useFetch(
    () => getSimilarMovies(id), [id]
  );
  const similar = Array.isArray(rawSimilar) ? rawSimilar : [];
  const {
    data: ratesRaw = [],
    loading: rLoad,
    refetch: refetchRates
  } = useFetch(() => getRatingsByMovie(id), [id]);
  const allRates = Array.isArray(ratesRaw) ? ratesRaw : [];
  const avg = allRates.length
    ? allRates.reduce((sum, r) => sum + r.score10, 0) / allRates.length
    : 0;
  const { data: myRate, refetch: refetchMyRate } = useFetch(
    () => (tokens ? getMyRating(id) : Promise.resolve(null)),
    [tokens, id]
  );

  const loadAllComments = useCallback(() => {
    if (!Array.isArray(reviews) || reviews.length === 0) return;
    Promise.all(
      reviews.map(r => getCommentsByReview(r.id).then(cs => [r.id, cs]))
    ).then(pairs => setCommentsMap(Object.fromEntries(pairs)));
  }, [reviews]);
  useEffect(loadAllComments, [loadAllComments]);

  const handleRate = useCallback(async score10 => {
    try {
      await upsertRating(id, score10);
      notify.notify('success', 'Puanınız kaydedildi');
      refetchRates();
      refetchMyRate();
    } catch {
      notify.notify('error', 'Puan gönderilemedi');
    }
  }, [id, notify, refetchRates, refetchMyRate]);

  useEffect(() => {
    if (!tokens) return;
    getMyFavorites()
      .then(favs => {
        const e = favs.find(f => f.movieId === id);
        setFavEntryId(e?.id || null);
      })
      .catch(() => {});
    getMyWatchlist()
      .then(wl => {
        const e = wl.find(w => w.movieId === id);
        setWatchEntryId(e?.id || null);
      })
      .catch(() => {});
  }, [tokens, id]);

  if (mLoad) return <div className="py-20 text-center">Yükleniyor…</div>;
  if (mErr)  return <div className="py-20 text-center text-danger">{mErr.message}</div>;
  if (!movie) return <div className="py-20 text-center">Film bulunamadı.</div>;

  return (
    <div className="container mx-auto px-4 py-8 space-y-8">
      <Header />

      {/* Header with controls */}
      <header className="flex flex-col md:flex-row md:items-center gap-4">
        <h1 className="flex-1 text-4xl font-bold">{movie.title}</h1>
        {tokens && (
          <>
            <button
              onClick={() => {
                setShowReviews(true);
                setTimeout(() => reviewsRef.current?.scrollIntoView({ behavior: 'smooth' }), 100);
              }}
              className="px-3 py-1 bg-accent text-white rounded"
            >
              İncelemelere Git
            </button>
            {/* Favori / İzleme butonları */}
            <button
              onClick={async () => {
                try {
                  if (!favEntryId) {
                    const added = await addFavorite(id);
                    setFavEntryId(added.id);
                    notify.notify('success', 'Favorilere eklendi');
                  } else {
                    await deleteFavorite(favEntryId);
                    setFavEntryId(null);
                    notify.notify('success', 'Favorilerden çıkarıldı');
                  }
                } catch {
                  notify.notify('error',
                    favEntryId
                      ? 'Favoriden çıkarılamadı'
                      : 'Favorilere eklenemedi'
                  );
                }
              }}
              className={`px-3 py-1 rounded ${
                favEntryId ? 'bg-gray-500' : 'bg-red-500'
              } text-white`}
            >
              {favEntryId ? 'Favoriden Çıkar' : 'Favorilere Ekle'}
            </button>
            <button
              onClick={async () => {
                try {
                  if (!watchEntryId) {
                    const added = await addWatch(id);
                    setWatchEntryId(added.id);
                    notify.notify('success', 'İzleme listesine eklendi');
                  } else {
                    await deleteWatch(watchEntryId);
                    setWatchEntryId(null);
                    notify.notify('success', 'İzleme listesinden çıkarıldı');
                  }
                } catch {
                  notify.notify('error',
                    watchEntryId
                      ? 'Listeden çıkarılamadı'
                      : 'Listeye eklenemedi'
                  );
                }
              }}
              className={`px-3 py-1 rounded ${
                watchEntryId ? 'bg-gray-500' : 'bg-blue-500'
              } text-white`}
            >
              {watchEntryId ? 'Listeden Çıkar' : 'Listeye Ekle'}
            </button>
            <TenStarRating value={myRate?.score10 ?? 0} onChange={handleRate} />
          </>
        )}
        <div className="flex flex-col items-center ml-auto">
          <span className="text-3xl font-bold text-accent">{avg.toFixed(1)}</span>
          <span className="text-xs text-muted">10 üzerinden</span>
        </div>
      </header>

      {/* Poster & details */}
      <section className="flex flex-col md:flex-row gap-8">
        <div className="w-48 md:w-64 shrink-0">
          <ProgressiveImage
            src={movie.posterUrl}
            alt={movie.title}
            className="w-full rounded-lg shadow object-cover"
          />
        </div>
        <div className="flex-1 space-y-6 text-sm">
          <div><strong>Yönetmen:</strong> {movie.directors?.join(', ') || '—'}</div>
          <div><strong>Tür:</strong> {movie.genres?.join(', ') || '—'}</div>
          <div><strong>Çıkış Tarihi:</strong> {new Date(movie.releaseDate).toLocaleDateString('tr-TR')}</div>
          <div><strong>Süre:</strong> {movie.duration} dk</div>
          <div><strong>Oyuncular:</strong> {(movie.actors||[]).slice(0,5).join(', ') || '—'}</div>
          <div><strong>Tagline:</strong> {movie.tagline || '—'}</div>
          <div><strong>Status:</strong> {movie.status || '—'}</div>
          <div><strong>Bütçe:</strong> {movie.budget?.toLocaleString() ?? '—'} USD</div>
          <div><strong>Gelir:</strong> {movie.revenue?.toLocaleString() ?? '—'} USD</div>
          <div>
            <strong>IMDb:</strong>{' '}
            {movie.imdbId
              ? <a href={`https://www.imdb.com/title/${movie.imdbId}`} target="_blank" rel="noopener noreferrer">{movie.imdbId}</a>
              : '—'}
          </div>
          <div>
            <strong>Website:</strong>{' '}
            {movie.homepageUrl
              ? <a href={movie.homepageUrl} target="_blank" rel="noopener noreferrer">Link</a>
              : '—'}
          </div>
          <div><strong>Yetişkin İçerik:</strong> {movie.isAdult ? 'Evet' : 'Hayır'}</div>
        </div>
      </section>

      {/* Cast */}
      <section className="space-y-4">
        <h2 className="text-2xl font-semibold">Oyuncular</h2>
        <div className="flex gap-4 overflow-x-auto py-2">
          {(movie.cast||[]).slice(0,10).map(a => (
            <Link key={a.actorId} to={`/actors/${a.actorId}`}>
              <div className="w-24 flex flex-col items-center">
                {a.photoUrl && (
                  <img
                    src={a.photoUrl}
                    alt={a.name}
                    className="w-16 h-16 rounded-full shadow object-cover"
                  />
                )}
                <span className="mt-1 text-xs text-center truncate">{a.name}</span>
              </div>
            </Link>
          ))}
        </div>
      </section>

      {/* Similar Movies */}
      <section className="space-y-6">
        <h2 className="text-2xl font-semibold">Benzer Filmler</h2>
        {sLoad ? (
          <div>Yükleniyor…</div>
        ) : sErr ? (
          <div className="text-danger">{sErr.message}</div>
        ) : (
          <div className="flex gap-4 overflow-x-auto py-2">
            {similar.map(m => (
              <SmallMovieCard key={m.id} movie={m} />
            ))}
          </div>
        )}
      </section>

      {/* New Review Form */}
      {tokens && (
        <section className="space-y-4">
          <h2 className="text-2xl font-semibold">Yeni İnceleme Yaz</h2>
          <ReviewForm
            movieId={id}
            onSuccess={() => {
              refetchReviews();
              loadAllComments();
            }}
          />
        </section>
      )}

      {/* Reviews + Comments */}
      {showReviews && (
        <section ref={reviewsRef} className="space-y-6">
          <h2 className="text-2xl font-semibold">İncelemeler</h2>
          {revLoad ? (
            <div>Yükleniyor…</div>
          ) : revErr ? (
            <div className="text-danger">{revErr.message}</div>
          ) : (
            Array.isArray(reviews) && reviews.map(r => (
              <ReviewCard
                key={r.id}
                review={r}
                comments={commentsMap[r.id] || []}
                onAddComment={dto =>
                  addComment(dto).then(newComment => {
                    setCommentsMap(m => ({
                      ...m,
                      [dto.reviewId]: [...(m[dto.reviewId]||[]), newComment]
                    }));
                  })
                }
                onDeleteComment={cid =>
                  deleteComment(cid).then(() => {
                    setCommentsMap(m => ({
                      ...m,
                      [r.id]: (m[r.id]||[]).filter(c => c.id !== cid)
                    }));
                  })
                }
                onDeleteReview={rid =>
                  deleteReview(rid).then(() => {
                    refetchReviews();
                    loadAllComments();
                  })
                }
              />
            ))
          )}
        </section>
      )}
    </div>
  );
}

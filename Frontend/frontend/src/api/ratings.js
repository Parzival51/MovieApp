// src/api/ratings.js
import client from './client';

export const getRatingsByMovie = movieId =>
  client
    .get('/ratings', { params: { movieId } })
    .then(r => r.data);

export const getMyRating = movieId =>
  client
    .get('/ratings', { params: { movieId, mine: true } })
    .then(r => (Array.isArray(r.data) && r.data.length ? r.data[0] : null));

export const upsertRating = (movieId, score10) =>
  client
    .post('/ratings/upsert', { movieId, score10 })
    .then(r => r.data);
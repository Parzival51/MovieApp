import client from './client';

export const getMyFavorites = () =>
  client.get('/favorites').then(r => r.data);

export const addFavorite = movieId =>
  client.post('/favorites', { movieId }).then(r => r.data);

export const deleteFavorite = favId =>
  client.delete(`/favorites/${favId}`);

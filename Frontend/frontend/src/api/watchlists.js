import client from './client';

export const getMyWatchlist = () =>
  client.get('/watchlists').then(r => r.data);

export const addWatch = movieId =>
  client.post('/watchlists', { movieId }).then(r => r.data);

export const deleteWatch = id =>
  client.delete(`/watchlists/${id}`);

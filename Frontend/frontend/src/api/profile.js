import client from './client';

export const getProfile = () =>
  client.get('/profile').then(r => r.data);

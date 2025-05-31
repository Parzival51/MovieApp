// src/api/actors.js
import client from './client';

export const getFeaturedActors = (limit = 10) =>
  client.get(`/actors?limit=${limit}`).then(r => r.data);

export const getAllActors = () =>
  client.get('/actors').then(r => r.data);

export const getActorById = id =>
  client.get(`/actors/${id}`).then(r => r.data);

export const createActor = dto =>
  client.post('/actors', dto).then(r => r.data);

export const updateActor = dto =>
  client.put(`/actors/${dto.id}`, dto).then(r => r.data);

export const deleteActor = id =>
  client.delete(`/actors/${id}`);

// Filmografi
export function getMoviesByActor(actorId) {
  return client
    .get(`/actors/${actorId}/movies`)
    .then(r => r.data);
}

// Kullanıcı ortalama puanı getir
export function getActorAverageRating(actorId) {
  return client
    .get(`/actors/${actorId}/rating`)
    .then(r => r.data.average);
}

// Aktöre oy ver/güncelle
export function rateActor(actorId, score) {
  return client
    .put(`/actors/${actorId}/rating`, { score })
    .then(r => r.data);
}

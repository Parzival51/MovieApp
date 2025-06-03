import client from './client';

export function getAllDirectors(limit = 100) {
  return client
    .get(`/directors?limit=${limit}`)
    .then(res => res.data);
}

export function getDirectorById(id) {
  return client
    .get(`/directors/${id}`)
    .then(res => res.data);
}

export function getMoviesByDirector(id) {
  return client
    .get(`/directors/${id}/movies`)
    .then(res => res.data);
}

export function getDirectorAverageRating(id) {
  return client
    .get(`/directors/${id}/rating`)
    .then(res => res.data.average);
}

export function rateDirector(id, score) {
  return client
    .put(`/directors/${id}/rating`, { score })
    .then(res => res.data.average);
}

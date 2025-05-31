import client from './client';

// Tüm yönetmenleri getirir (mevcut)
export function getAllDirectors(limit = 100) {
  return client
    .get(`/directors?limit=${limit}`)
    .then(res => res.data);
}

// Tek bir yönetmeni getirir
export function getDirectorById(id) {
  return client
    .get(`/directors/${id}`)
    .then(res => res.data);
}

// **Yönetmene ait filmleri getirir**
export function getMoviesByDirector(id) {
  return client
    .get(`/directors/${id}/movies`)
    .then(res => res.data);
}

// Yönetmenin ortalama puanını getirir
export function getDirectorAverageRating(id) {
  return client
    .get(`/directors/${id}/rating`)
    .then(res => res.data.average);
}

// Yönetmene puan (rating) gönderir ve güncel ortalamayı döner
export function rateDirector(id, score) {
  return client
    .put(`/directors/${id}/rating`, { score })
    .then(res => res.data.average);
}

import client from './client';

export function getFeaturedMovies(limit = 5) {
  return client.get('/movies').then(res =>
    Array.isArray(res.data) ? res.data.slice(0, limit) : []
  );
}

export function getTopRated(count = 5) {
  return client.get(`/movies/top-rated?count=${count}`).then(r => r.data);
}

export function getNewReleases() {
  return client.get('/movies?sort=releaseDate').then(r => r.data);
}

export function getMovieById(id) {
  return client.get(`/movies/${id}`).then(r => r.data);
}

// filtreli arama ve tür bazlı sayfalı liste:
export function searchMovies(q, page = 1, pageSize = 20) {
  const query = encodeURIComponent(q);
  return client
    .get(`/movies/search?q=${query}&page=${page}&pageSize=${pageSize}`)
    .then(r => r.data);
}

export function getByGenre(genreId, page = 1, pageSize = 20) {
  return client
    .get(`/movies/search?genre=${genreId}&page=${page}&pageSize=${pageSize}`)
    .then(r => r.data);
}

/* ───────── ADMIN CRUD (değişmedi) ───────── */
export function createMovie(dto)      { return client.post('/movies', dto).then(r => r.data); }
export function updateMovie(dto)      { return client.put(`/movies/${dto.id}`, dto).then(r => r.data); }
export function deleteMovie(id)       { return client.delete(`/movies/${id}`); }
export function getAllMovies()        { return client.get('/movies').then(r => r.data); }

export function suggestMovies(q, max = 5) {
  const query = encodeURIComponent(q);
  return client
    .get(`/movies/suggest?q=${query}&max=${max}`)
    .then(r => Array.isArray(r.data) ? r.data : []);
}

export function getSimilarMovies(movieId, max = 5) {
  return client
    .get(`/movies/${movieId}/similar?max=${max}`)
    .then(r => Array.isArray(r.data) ? r.data : []);
}

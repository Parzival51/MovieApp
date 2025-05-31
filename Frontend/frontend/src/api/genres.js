import client from "./client";



export function getAllGenres() {
  return client.get("/genres").then(r => r.data);
}

export function createGenre(dto) {
  return client.post("/genres", dto).then(r => r.data);
}

export function updateGenre(dto) {
  return client.put(`/genres/${dto.id}`, dto).then(r => r.data);
}

export function deleteGenre(id) {
  return client.delete(`/genres/${id}`);
}

export function getGenreById(id) {
  return client.get(`/genres/${id}`).then(r => r.data);
}



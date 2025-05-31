import client from './client';

/* ───────── CREATE ───────── */
export const addComment = dto =>
  client.post('/comments', dto).then(r => r.data);

/* ───────── READ ────────── */
export const getCommentsByReview = reviewId =>
  client
    .get(`/comments?reviewId=${encodeURIComponent(reviewId)}`)
    .then(r => r.data);

/* ───────── UPDATE ───────── */
export const updateComment = (id, dto) =>
  client.put(`/comments/${id}`, dto);

/* ───────── DELETE ───────── */
export const deleteComment = id =>
  client.delete(`/comments/${id}`);

/* ──────── MODERATION ─────── */
// ❶ Onay bekleyen yorumları getir
export const getPendingComments = () =>
  client.get('/comments/pending').then(r => r.data);

// ❷ Yorum onayla
export const approveComment = id =>
  client.put(`/comments/${id}/approve`);

import client from './client';

export const addComment = dto =>
  client.post('/comments', dto).then(r => r.data);

export const getCommentsByReview = reviewId =>
  client
    .get(`/comments?reviewId=${encodeURIComponent(reviewId)}`)
    .then(r => r.data);

export const updateComment = (id, dto) =>
  client.put(`/comments/${id}`, dto);

export const deleteComment = id =>
  client.delete(`/comments/${id}`);

export const getPendingComments = () =>
  client.get('/comments/pending').then(r => r.data);

export const approveComment = id =>
  client.put(`/comments/${id}/approve`);

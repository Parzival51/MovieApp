// src/api/reviews.js
import client from './client';

export const addReview = body =>
  client.post('/reviews', body).then(r => r.data);

export const getReviewsByMovie = movieId =>
  client.get(`/reviews/movie/${movieId}`).then(r => r.data);

export const getLatestReviews = (limit = 10) =>
  client.get('/reviews')
        .then(r => Array.isArray(r.data) ? r.data : [])
        .then(arr =>
          arr
            .filter(x => x.isApproved)
            .sort((a, b) => new Date(b.createdAt) - new Date(a.createdAt))
            .slice(0, limit)
            .map(x => ({
              ...x,
              excerpt: x.content.length > 140
                        ? x.content.slice(0, 137) + '…'
                        : x.content
            }))
        );

export const getPendingReviews = () =>
  client.get('/reviews/pending').then(r => r.data);

export const approveReview = reviewId =>
  client.put(`/reviews/${reviewId}/approve`).then(r => r.data);

export const deleteReview = id =>
  client.delete(`/reviews/${id}`);

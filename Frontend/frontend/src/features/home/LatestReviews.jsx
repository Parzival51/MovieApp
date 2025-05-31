// src/features/home/LatestReviews.jsx
import React from 'react';
import useFetch from '../../hooks/useFetch';
import { getLatestReviews } from '../../api/reviews';
import ReviewCard from '../../components/ReviewCard';

export default function LatestReviews() {
  const { data, loading, error } = useFetch(() => getLatestReviews(5), []);
  const reviews = Array.isArray(data) ? data : [];

  if (loading) {
    return (
      <div className="py-6 text-center text-gray-300 dark:text-gray-500">
        Yükleniyor…
      </div>
    );
  }

  if (error) {
    return (
      <div className="py-6 text-center text-danger">
        Hata: {error.message}
      </div>
    );
  }

  if (reviews.length === 0) {
    return (
      <div className="py-6 text-center italic text-gray-400 dark:text-gray-600">
        Henüz inceleme yok.
      </div>
    );
  }

  return (
    <div className="space-y-4">
      {reviews.map(review => (
        <ReviewCard key={review.id} review={review} />
      ))}
    </div>
  );
}

import React, { useState } from 'react';
import PropTypes from 'prop-types';
import { addReview } from '../api/reviews';

export default function ReviewForm({ movieId, onSuccess }) {
  const [content, setContent] = useState('');
  const [stars, setStars] = useState(5);
  const [submitting, setSubmitting] = useState(false);

  const handleSubmit = async e => {
    e.preventDefault();
    if (submitting) return;

    setSubmitting(true);
    try {
      // CreateReviewDto requires movieId, content, and stars
      await addReview({ movieId, content, stars });
      setContent('');
      setStars(5);
      onSuccess?.();
    } catch {
      alert('Yorum gönderilemedi. Lütfen tekrar deneyin.');
    } finally {
      setSubmitting(false);
    }
  };

  return (
    <form onSubmit={handleSubmit} className="space-y-2 bg-surface p-4 rounded">
      <textarea
        className="w-full border px-2 py-1 rounded bg-surface"
        placeholder="Yorumunuz"
        rows={4}
        value={content}
        onChange={e => setContent(e.target.value)}
        required
        disabled={submitting}
      />

      {/* ⭐ Puan seçici */}
      <div className="flex items-center space-x-2">
        <label className="text-sm">Puanınız:</label>
        <select
          value={stars}
          onChange={e => setStars(Number(e.target.value))}
          disabled={submitting}
          className="border rounded px-2 py-1"
        >
          {[1, 2, 3, 4, 5].map(n => (
            <option key={n} value={n}>{n} yıldız</option>
          ))}
        </select>
      </div>

      <button
        type="submit"
        className="px-4 py-1 bg-blue-600 text-white rounded hover:bg-blue-700 disabled:opacity-50"
        disabled={submitting}
      >
        {submitting ? 'Gönderiliyor…' : 'Gönder'}
      </button>
    </form>
  );
}

ReviewForm.propTypes = {
  movieId: PropTypes.string.isRequired,
  onSuccess: PropTypes.func
};

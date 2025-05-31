// src/components/CommentForm.jsx
import React, { useState } from 'react';
import PropTypes from 'prop-types';

export default function CommentForm({ reviewId, onSubmit }) {
  const [content, setContent] = useState('');

  const handle = e => {
    e.preventDefault();
    if (!content.trim()) return;
    onSubmit({ reviewId, content });
    setContent('');
  };

  return (
    <form onSubmit={handle} className="mt-4 space-y-2 bg-transparent">
      <textarea
        className="w-full border border-gray-600 rounded px-2 py-1 bg-transparent text-white placeholder-gray-500"
        placeholder="Yorumunuzu yazın…"
        rows={3}
        value={content}
        onChange={e => setContent(e.target.value)}
        required
      />
      <button
        type="submit"
        className="px-4 py-1 bg-blue-600 text-white rounded hover:bg-blue-700"
      >
        Yorum Ekle
      </button>
    </form>
  );
}

CommentForm.propTypes = {
  reviewId: PropTypes.string.isRequired,
  onSubmit: PropTypes.func.isRequired,
};

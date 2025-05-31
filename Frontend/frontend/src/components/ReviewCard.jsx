// src/components/ReviewCard.jsx
import React, { useState } from 'react';
import PropTypes from 'prop-types';
import { motion, useReducedMotion } from 'framer-motion';
import Card from './Card';
import CommentForm from './CommentForm';

function formatRelativeDate(dateString) {
  const date = new Date(dateString);
  const now = new Date();
  const diffMs     = now - date;
  const diffHours  = Math.floor(diffMs / 1000 / 60 / 60);
  const diffDays   = Math.floor(diffHours / 24);

  if (diffHours < 1)   return 'şimdi';
  if (diffHours < 24)  return `${diffHours} saat önce`;
  if (diffDays < 7)    return `${diffDays} gün önce`;
  const diffWeeks  = Math.floor(diffDays / 7);
  if (diffWeeks < 4)   return `${diffWeeks} hafta önce`;
  const diffMonths = Math.floor(diffDays / 30);
  if (diffMonths < 12) return `${diffMonths} ay önce`;
  return `${Math.floor(diffMonths / 12)} yıl önce`;
}

export default function ReviewCard({
  review,
  comments = [],
  onAddComment,
  onDeleteComment,
  onDeleteReview
}) {
  const [open, setOpen] = useState(false);
  const shouldReduce   = useReducedMotion();

  const toggle = () => setOpen(o => !o);
  const handleDelete = e => {
    e.stopPropagation();
    if (window.confirm('Emin misin?')) {
      onDeleteReview(review.id);
    }
  };
  const starsTxt = n => '★'.repeat(n) + '☆'.repeat(5 - n);

  return (
    <Card padding={false} className="overflow-hidden border rounded-lg bg-transparent">
      {/* Dış kapsayıcı artık <div> */}
      <div
        role="button"
        onClick={toggle}
        className="w-full px-6 py-4 hover:bg-white/10 focus:outline-none cursor-pointer"
      >
        {/* Header: userName – relativeDate – stars – silme */}
        <div className="flex items-center justify-between">
          <div className="flex items-center space-x-2">
            <span className="text-xs text-gray-400">{review.userName}</span>
            <span className="text-xs text-gray-500">{formatRelativeDate(review.createdAt)}</span>
          </div>
          <div className="flex items-center space-x-4">
            <div className="text-accent shrink-0">{starsTxt(review.stars)}</div>
            {onDeleteReview && (
              <button
                onClick={handleDelete}
                className="text-red-500 text-xs hover:underline"
              >
                Sil
              </button>
            )}
          </div>
        </div>

        {/* İçerik */}
        <p className="text-sm text-gray-200 mt-2 w-full text-left">
          {review.content}
        </p>
      </div>

      {/* Collapsible Comments & Form */}
      <motion.div
        initial={false}
        animate={open ? 'open' : 'collapsed'}
        variants={{
          open:      { height: 'auto', opacity: 1 },
          collapsed: { height: 0,     opacity: 0 }
        }}
        transition={shouldReduce ? { duration: 0 } : { duration: 0.3 }}
        className="overflow-hidden border-t border-gray-700 bg-transparent"
      >
        <div className="px-6 py-4 space-y-4">
          {comments.length > 0 ? (
            <ul className="space-y-2">
              {comments.map(c => (
                <li key={c.id} className="border rounded p-3 flex justify-between bg-transparent">
                  <div className="flex flex-col">
                    <div className="flex items-center space-x-2">
                      <span className="text-xs text-gray-400">{c.userName}</span>
                      <span className="text-xs text-gray-500">{formatRelativeDate(c.createdAt)}</span>
                    </div>
                    <p className="text-sm text-gray-200 mt-1">{c.content}</p>
                  </div>
                  {onDeleteComment && (
                    <button
                      onClick={() => onDeleteComment(c.id)}
                      className="text-red-500 text-sm hover:underline"
                    >
                      Sil
                    </button>
                  )}
                </li>
              ))}
            </ul>
          ) : (
            <div className="italic text-gray-500">Henüz yorum yok.</div>
          )}
          {open && onAddComment && (
            <CommentForm reviewId={review.id} onSubmit={onAddComment} />
          )}
        </div>
      </motion.div>
    </Card>
  );
}

ReviewCard.propTypes = {
  review: PropTypes.shape({
    id:        PropTypes.string.isRequired,
    userName:  PropTypes.string.isRequired,
    content:   PropTypes.string.isRequired,
    createdAt: PropTypes.string.isRequired,
    stars:     PropTypes.number.isRequired
  }).isRequired,
  comments: PropTypes.arrayOf(
    PropTypes.shape({
      id:        PropTypes.string.isRequired,
      userName:  PropTypes.string.isRequired,
      content:   PropTypes.string.isRequired,
      createdAt: PropTypes.string.isRequired
    })
  ).isRequired,
  onAddComment:    PropTypes.func,
  onDeleteComment: PropTypes.func,
  onDeleteReview:  PropTypes.func.isRequired
};

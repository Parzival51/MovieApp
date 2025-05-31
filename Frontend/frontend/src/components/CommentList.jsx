import React from 'react';
import PropTypes from 'prop-types';

function formatRelativeDate(dateString) {
  const date = new Date(dateString);
  const now = new Date();
  const diffMs = now - date;
  const diffSeconds = Math.floor(diffMs / 1000);
  const diffMinutes = Math.floor(diffSeconds / 60);
  const diffHours   = Math.floor(diffMinutes / 60);
  const diffDays    = Math.floor(diffHours / 24);

  if (diffHours < 24) {
    return `${diffHours} saat önce`;
  } else if (diffDays < 7) {
    return `${diffDays} gün önce`;
  }

  const diffWeeks = Math.floor(diffDays / 7);
  if (diffWeeks < 4) {
    return `${diffWeeks} hafta önce`;
  }

  const diffMonths = Math.floor(diffDays / 30);
  if (diffMonths < 12) {
    return `${diffMonths} ay önce`;
  }

  const diffYears = Math.floor(diffMonths / 12);
  return `${diffYears} yıl önce`;
}

export default function CommentList({ comments = [], onDelete }) {
  if (comments.length === 0) {
    return <div className="italic text-gray-500">Henüz yorum yok.</div>;
  }

  return (
    <ul className="space-y-4">
      {comments.map(c => (
        <li key={c.id} className="border-b pb-2 flex justify-between">
          <div className="flex flex-col">
            <div className="flex items-center space-x-2">
              <span className="text-xs text-gray-400">{c.userName}</span>
              <span className="text-xs text-gray-500">{formatRelativeDate(c.createdAt)}</span>
            </div>
            <div className="text-sm mt-1">{c.content}</div>
          </div>
          {onDelete && (
            <button
              onClick={() => onDelete(c.id)}
              className="text-red-500 text-sm hover:underline ml-4"
            >
              Sil
            </button>
          )}
        </li>
      ))}
    </ul>
  );
}

CommentList.propTypes = {
  comments: PropTypes.arrayOf(PropTypes.shape({
    id:        PropTypes.string.isRequired,
    userName:  PropTypes.string.isRequired,
    content:   PropTypes.string.isRequired,
    createdAt: PropTypes.string.isRequired,
  })),
  onDelete: PropTypes.func,
};

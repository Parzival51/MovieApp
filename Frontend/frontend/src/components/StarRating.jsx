import React from 'react';

export default function StarRating({ value = 0, onChange, readOnly = false }) {
  const stars = [1, 2, 3, 4, 5];

  return (
    <div className="flex space-x-1">
      {stars.map(n => (
        <button
          key={n}
          type="button"
          onClick={() => !readOnly && onChange(n)}
          className={
            n <= value ? 'text-yellow-400 text-xl' : 'text-gray-300 text-xl'
          }
        >
          ★
        </button>
      ))}
    </div>
  );
}

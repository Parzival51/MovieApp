import React from 'react';
import PropTypes from 'prop-types';

/**
 * 1-10 arası puanlayıcı.
 * readOnly=true → sadece görüntüleme.
 */
export default function TenStarRating({ value = 0, onChange, readOnly = false }) {
  const circles = Array.from({ length: 10 }, (_, i) => i + 1);

  return (
    <div className="flex gap-1">
      {circles.map(n => (
        <button
          key={n}
          type="button"
          disabled={readOnly}
          onClick={() => onChange?.(n)}
          className={`w-5 h-5 rounded-full border
                      ${n <= value ? 'bg-accent border-accent'
                                    : 'bg-transparent border-muted'}
                      ${readOnly ? 'cursor-default'
                                 : 'hover:bg-accent/40 transition-colors'}`}
          aria-label={`${n} puan`}
        />
      ))}
    </div>
  );
}

TenStarRating.propTypes = {
  value:    PropTypes.number,
  onChange: PropTypes.func,
  readOnly: PropTypes.bool
};

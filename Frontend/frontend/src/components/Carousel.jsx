// src/components/Carousel.jsx
import React from 'react';
import PropTypes from 'prop-types';

/**
 * Basit yatay carousel
 *
 * Props
 *  - items        : any[]               – liste verisi
 *  - renderItem   : (item) => ReactNode – kartı nasıl çizeceğiz
 *  - prev / next  : () => void          – buton callback’leri
 *  - visibleCount : number              – aynı anda kaç kart görünsün
 *  - current      : number              – aktif indeks (parent state)
 */
export default function Carousel({
  items = [],
  renderItem,
  prev,
  next,
  visibleCount = 3,
  current = 0
}) {
  const list = Array.isArray(items) ? items : [];
  if (list.length === 0) return null;

  // Her kart için yüzde genişlik
  const widthPct = 100 / visibleCount;
  // İndeks sınırları
  const maxIndex = Math.max(0, list.length - visibleCount);
  const idx = Math.min(Math.max(current, 0), maxIndex);

  return (
    <div className="relative overflow-hidden select-none">
      {/* ◀︎ Önceki */}
      <button
        onClick={prev}
        className="absolute left-2 top-1/2 -translate-y-1/2 z-10
                   rounded-full bg-black/50 p-2 text-white
                   hover:bg-black/70"
      >
        ‹
      </button>

      
      <div className="overflow-hidden -mx-2">
        {/* Kaydırma rayı */}
        <div
          className="flex transition-transform duration-300 ease-in-out"
          style={{ transform: `translateX(-${idx * widthPct}%)` }}
        >
          {list.map((item, i) => (
            <div
              key={i}
              className="flex-none px-2"
              style={{ width: `${widthPct}%` }}
            >
              {renderItem(item)}
            </div>
          ))}
        </div>
      </div>

      {/* ▶︎ Sonraki */}
      <button
        onClick={next}
        className="absolute right-2 top-1/2 -translate-y-1/2 z-10
                   rounded-full bg-black/50 p-2 text-white
                   hover:bg-black/70"
      >
        ›
      </button>
    </div>
  );
}

Carousel.propTypes = {
  items:        PropTypes.array,
  renderItem:   PropTypes.func.isRequired,
  prev:         PropTypes.func.isRequired,
  next:         PropTypes.func.isRequired,
  visibleCount: PropTypes.number,
  current:      PropTypes.number
};

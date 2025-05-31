import React from 'react';
import PropTypes from 'prop-types';
import { useNavigate } from 'react-router-dom';
import Card from './Card';
import ProgressiveImage from './ProgressiveImage';

export default function MovieCard({ movie }) {
  const navigate = useNavigate();

  return (
    <Card
      hover
      padding={false}
      onClick={() => navigate(`/movies/${movie.id}`)}
      className="cursor-pointer group overflow-hidden"
    >
      {/* Poster */}
      <div className="relative aspect-[2/3]">
        <ProgressiveImage
          src={movie.posterUrl}
          alt={movie.title}
          className="w-full h-full object-cover transition-transform duration-300 group-hover:scale-105"
        />

        {/* Rating badge */}
        <span className="absolute top-2 right-2 bg-black/70 text-accent text-xs font-semibold px-2 py-0.5 rounded-full">
          {movie.averageRating.toFixed(1)}
        </span>
      </div>

      {/* Başlık */}
      <div className="p-4">
        <h3 className="text-lg font-semibold truncate text-surface-900 dark:text-surface-100">
          {movie.title}
        </h3>
      </div>
    </Card>
  );
}

MovieCard.propTypes = {
  movie: PropTypes.shape({
    id:             PropTypes.string.isRequired,
    title:          PropTypes.string.isRequired,
    posterUrl:      PropTypes.string,
    averageRating:  PropTypes.number
  }).isRequired
};

import React from 'react';
import PropTypes from 'prop-types';
import { Link } from 'react-router-dom';

export default function SmallMovieCard({ movie }) {
  return (
    <Link
      to={`/movies/${movie.id}`}
      className="block w-24 flex-shrink-0 text-center"
    >
      <img
        src={movie.posterUrl}
        alt={movie.title}
        className="w-full h-auto rounded-lg shadow-sm object-cover"
      />
      <p className="mt-1 text-xs text-white truncate">{movie.title}</p>
    </Link>
  );
}

SmallMovieCard.propTypes = {
  movie: PropTypes.shape({
    id:         PropTypes.string.isRequired,
    posterUrl:  PropTypes.string,
    title:      PropTypes.string.isRequired,
  }).isRequired,
};

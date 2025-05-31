import React from 'react';
import PropTypes from 'prop-types';
import { Link } from 'react-router-dom';
import Card from './Card';
import ProgressiveImage from './ProgressiveImage';

export default function DirectorCard({ director }) {
  return (
    <Link to={`/directors/${director.id}`}>
      <Card
        hover
        padding={false}
        className="cursor-pointer group overflow-hidden"
      >
        <div className="relative aspect-[2/3]">
          <ProgressiveImage
            src={director.profilePath}
            alt={director.name}
            className="w-full h-full object-cover transition-transform duration-300 group-hover:scale-105"
          />
        </div>
        <div className="p-4">
          <h4 className="text-sm font-medium truncate text-center text-surface-900 dark:text-surface-100">
            {director.name}
          </h4>
        </div>
      </Card>
    </Link>
  );
}

DirectorCard.propTypes = {
  director: PropTypes.shape({
    id:          PropTypes.string.isRequired,
    name:        PropTypes.string.isRequired,
    profilePath: PropTypes.string,
  }).isRequired,
};

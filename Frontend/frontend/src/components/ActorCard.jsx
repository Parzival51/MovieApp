// src/components/ActorCard.jsx
import React from 'react';
import PropTypes from 'prop-types';
import { Link } from 'react-router-dom';
import Card from './Card';
import ProgressiveImage from './ProgressiveImage';

export default function ActorCard({ actor }) {
  return (
    <Link to={`/actors/${actor.id}`}>  {/* Link to detay sayfası */}
      <Card
        hover
        padding={false}
        className="cursor-pointer group overflow-hidden"
      >
        {/* Fotoğraf bölümü: 2:3 görüntü oranı */}
        <div className="relative aspect-[2/3]">
          <ProgressiveImage
            src={actor.photoUrl}
            alt={actor.name}
            className="w-full h-full object-cover transition-transform duration-300 group-hover:scale-105"
          />
        </div>

        {/* İsim bölümü */}
        <div className="p-4">
          <h4 className="text-sm font-medium truncate text-center text-surface-900 dark:text-surface-100">
            {actor.name}
          </h4>
        </div>
      </Card>
    </Link>
  );
}

ActorCard.propTypes = {
  actor: PropTypes.shape({
    id:       PropTypes.string.isRequired,
    name:     PropTypes.string.isRequired,
    photoUrl: PropTypes.string
  }).isRequired
};


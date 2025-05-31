// src/components/GenreBadge.jsx
import React from 'react';
import PropTypes from 'prop-types';

export default function GenreBadge({ name, selected, onClick }) {
  return (
    <button
      onClick={onClick}
      className={`px-3 py-1 rounded-full text-sm mr-2 mb-2 focus:outline-none transition ${
        selected
          ? 'bg-blue-600 text-white'
          : 'bg-gray-200 text-gray-800 hover:bg-gray-300'
      }`}
    >
      {name}
    </button>
  );
}

GenreBadge.propTypes = {
  name: PropTypes.string.isRequired,
  selected: PropTypes.bool,
  onClick: PropTypes.func,
};

import React, { useState, useCallback } from 'react';
import PropTypes from 'prop-types';
import { useNavigate } from 'react-router-dom';
import useDebounce from '../hooks/useDebounce';
import { suggestMovies } from '../api/movies';

export default function SearchBar({ className = '' }) {
  const [query,   setQuery]   = useState('');
  const [results, setResults] = useState([]);
  const [error,   setError]   = useState(null);
  const navigate  = useNavigate();

  const debounced = useDebounce(query, 400);

  /* autosuggest */
  const fetchSuggest = useCallback(async () => {
    if (!debounced.trim()) { setResults([]); return; }
    try {
      const list = await suggestMovies(debounced, 8);
      setResults(list);
    } catch (err) {
      console.error(err);
      setError('Arama yapılamadı');
    }
  }, [debounced]);

  React.useEffect(() => { fetchSuggest(); }, [fetchSuggest]);

  const gotoMovie = id => {
    setQuery('');
    setResults([]);
    navigate(`/movies/${id}`);
  };

  return (
    <div className={`relative ${className}`}>
      <input
        type="text"
        placeholder="Film ara…"
        value={query}
        onChange={e => setQuery(e.target.value)}
        className="px-3 py-1 border rounded w-48 md:w-64"
      />

      {/* Drop-down */}
      {results.length > 0 && (
        <ul className="absolute z-50 mt-1 w-full bg-white border rounded shadow max-h-60 overflow-y-auto">
          {results.map(m => (
            <li
              key={m.id}
              onClick={() => gotoMovie(m.id)}
              className="px-3 py-2 cursor-pointer hover:bg-gray-100"
            >
              {m.title}
            </li>
          ))}
        </ul>
      )}

      {error && (
        <div className="absolute z-50 mt-1 w-full bg-red-100 text-red-800 p-2 rounded">
          {error}
        </div>
      )}
    </div>
  );
}

SearchBar.propTypes = { className: PropTypes.string };

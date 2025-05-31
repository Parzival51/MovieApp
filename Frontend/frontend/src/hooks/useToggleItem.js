import { useState, useEffect } from 'react';

export default function useToggleItem(list, movieId) {
  const [entry, setEntry] = useState(null);

  useEffect(() => {
    const found = (Array.isArray(list) ? list : []).find(i => i.movieId === movieId);
    setEntry(found || null);
  }, [list, movieId]);

  return entry;        
}

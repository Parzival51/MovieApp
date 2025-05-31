import { useState, useEffect, useCallback } from 'react';

/**
 * useFetch: herhangi bir async fetch fonksiyonunu
 * loading / data / error üçlüsüyle yönetir.
 *
 * @param {Function} fetcher - async fonksiyon (örn. () => getTopRated(5))
 * @param {Array} deps - effect bağımlılıkları
 * @returns {{ data: any, loading: boolean, error: any, refetch: Function }}
 */
export default function useFetch(fetcher, deps = []) {
  const [data, setData] = useState(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState(null);

  const load = useCallback(async () => {
    setLoading(true);
    setError(null);
    try {
      const result = await fetcher();
      setData(result);
    } catch (err) {
      setError(err);
    } finally {
      setLoading(false);
    }
  },  deps);

  useEffect(() => {
    load();
  }, [load]);

  return { data, loading, error, refetch: load };
}

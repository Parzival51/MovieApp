import React, { useCallback, useState, useMemo } from 'react';
import { Link } from 'react-router-dom';
import useFetch from '../../hooks/useFetch';
import { getAllMovies, deleteMovie } from '../../api/movies';
import DataTable from '../../components/DataTable';
import Button from '../../components/Button';

export default function AdminMoviesPage() {
  const { data, loading, error, refetch } = useFetch(getAllMovies, []);
  const movies = Array.isArray(data) ? data : [];

  const [sortKey, setSortKey] = useState('title');
  const [sortDir, setSortDir] = useState('asc');
  const sorted = useMemo(() => {
    if (!sortKey) return movies;
    const sorted = [...movies].sort((a, b) => {
      const av = String(a[sortKey] ?? '').toLowerCase();
      const bv = String(b[sortKey] ?? '').toLowerCase();
      return av.localeCompare(bv, 'tr');
    });
    return sortDir === 'asc' ? sorted : sorted.reverse();
  }, [movies, sortKey, sortDir]);

  const handleSort = (key, dir) => {
    setSortKey(key);
    setSortDir(dir);
  };

  const handleDelete = useCallback(
    async id => {
      if (!window.confirm('Bu filmi silmek istediğine emin misin?')) return;
      await deleteMovie(id);
      refetch();
    },
    [refetch]
  );

  const columns = [
    { key: 'title',       label: 'Başlık',       sortable: true,  className:'w-1/4' },
    { key: 'releaseDate', label: 'Çıkış',        sortable: true  },
    { key: 'duration',    label: 'Süre',         sortable: true  },
    { key: 'language',    label: 'Dil',          sortable: true  },
    { key: 'country',     label: 'Ülke',         sortable: true  },
    { key: 'actions',     label: 'İşlemler',     sortable: false }
  ];

  const renderRow = m => (
    <tr key={m.id} className="border-t">
      <td className="px-4 py-2">{m.title}</td>
      <td className="px-4 py-2">{m.releaseDate?.split('T')[0]}</td>
      <td className="px-4 py-2">{m.duration} dk</td>
      <td className="px-4 py-2">{m.language}</td>
      <td className="px-4 py-2">{m.country}</td>
      <td className="px-4 py-2 space-x-2">
        <Link
          to={`/admin/movies/edit/${m.id}`}
          className="text-secondary hover:underline"
        >
          Düzenle
        </Link>
        <button
          onClick={() => handleDelete(m.id)}
          className="text-danger hover:underline"
        >
          Sil
        </button>
      </td>
    </tr>
  );

  if (error) return <div className="p-4 text-danger">Hata: {error.message}</div>;

  return (
    <div className="container mx-auto px-4 py-6 space-y-4">
      <div className="flex items-center justify-between">
        <h2 className="text-2xl font-bold">Film Yönetimi</h2>
        <Button
          as={Link}
          to="/admin/movies/new"
          variant="primary"
          size="sm"
        >
          Yeni Film
        </Button>
      </div>

      <DataTable
        columns={columns}
        data={sorted}
        loading={loading}
        sortKey={sortKey}
        sortDir={sortDir}
        onSort={handleSort}
        renderRow={renderRow}
      />
    </div>
  );
}

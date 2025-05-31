import React, { useCallback, useState, useMemo } from 'react';
import { Link } from 'react-router-dom';
import useFetch from '../../hooks/useFetch';
import { getAllGenres, deleteGenre } from '../../api/genres';
import DataTable from '../../components/DataTable';
import Button from '../../components/Button';

export default function AdminGenresPage() {
  const { data, loading, error, refetch } = useFetch(getAllGenres, []);
  const genres = Array.isArray(data) ? data : [];

  const [sortKey, setSortKey] = useState('name');
  const [sortDir, setSortDir] = useState('asc');

  const sorted = useMemo(() => {
    const arr = [...genres].sort((a, b) =>
      (a.name || '').localeCompare(b.name || '', 'tr')
    );
    return sortDir === 'asc' ? arr : arr.reverse();
  }, [genres, sortDir]);

  const handleSort = (k,d) => { setSortDir(d); };

  const handleDelete = useCallback(async id => {
    if (!window.confirm('Bu türü silmek istediğinize emin misiniz?')) return;
    await deleteGenre(id);
    refetch();
  }, [refetch]);

  const columns = [
    { key:'name',    label:'Ad', sortable:true },
    { key:'actions', label:'İşlemler' }
  ];
  const renderRow = g => (
    <tr key={g.id} className="border-t">
      <td className="px-4 py-2">{g.name}</td>
      <td className="px-4 py-2 space-x-2">
        <Link to={`/admin/genres/edit/${g.id}`} className="text-secondary hover:underline">Düzenle</Link>
        <button onClick={() => handleDelete(g.id)} className="text-danger hover:underline">Sil</button>
      </td>
    </tr>
  );

  if (error) return <div className="p-4 text-danger">Hata: {error.message}</div>;

  return (
    <div className="container mx-auto px-4 py-6 space-y-4">
      <div className="flex items-center justify-between">
        <h2 className="text-2xl font-bold">Tür Yönetimi</h2>
        <Button as={Link} to="/admin/genres/new" variant="primary" size="sm">Yeni Tür</Button>
      </div>

      <DataTable
        columns={columns}
        data={sorted}
        loading={loading}
        sortKey="name"
        sortDir={sortDir}
        onSort={handleSort}
        renderRow={renderRow}
      />
    </div>
  );
}

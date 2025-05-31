// src/pages/AdminActorsPage.jsx
import React, { useCallback, useState, useMemo, useEffect } from 'react';
import { Link, useLocation } from 'react-router-dom';
import useFetch from '../../hooks/useFetch';
import { getAllActors, deleteActor } from '../../api/actors';
import DataTable from '../../components/DataTable';
import Button from '../../components/Button';

export default function AdminActorsPage() {
  const location = useLocation();
  const { data, loading, error, refetch } = useFetch(getAllActors, []);
  const actors = Array.isArray(data) ? data : [];

  useEffect(() => {
    if (location.state?.shouldRefresh) {
      refetch();
      window.history.replaceState({}, '');
    }
  }, [location.state, refetch]);

  const [sortKey, setSortKey] = useState('name');
  const [sortDir, setSortDir] = useState('asc');

  const sorted = useMemo(() => {
    const arr = [...actors];
    arr.sort((a, b) =>
      (a[sortKey] || '').localeCompare(b[sortKey] || '', 'tr')
    );
    return sortDir === 'asc' ? arr : arr.reverse();
  }, [actors, sortKey, sortDir]);

  const handleSort = (k, d) => {
    setSortKey(k);
    setSortDir(d);
  };

  const handleDelete = useCallback(async id => {
    if (!window.confirm('Bu aktörü silmek istediğinize emin misiniz?')) return;
    await deleteActor(id);
    refetch();
  }, [refetch]);

  const columns = [
    { key:'name',       label:'İsim',          sortable:true },
    { key:'birthDate',  label:'Doğum Tarihi',  sortable:true },
    { key:'actions',    label:'İşlemler' }
  ];

  const renderRow = a => (
    <tr key={a.id} className="border-t">
      <td className="px-4 py-2">{a.name}</td>
      <td className="px-4 py-2">{a.birthDate?.split('T')[0] || '—'}</td>
      <td className="px-4 py-2 space-x-2">
        <Link to={`/admin/actors/edit/${a.id}`} className="text-secondary hover:underline">
          Düzenle
        </Link>
        <button onClick={() => handleDelete(a.id)} className="text-danger hover:underline">
          Sil
        </button>
      </td>
    </tr>
  );

  if (error) return <div className="p-4 text-danger">Hata: {error.message}</div>;

  return (
    <div className="container mx-auto px-4 py-6 space-y-4">
      <div className="flex items-center justify-between">
        <h2 className="text-2xl font-bold">Aktör Yönetimi</h2>
        <Button as={Link} to="/admin/actors/new" variant="primary" size="sm">
          Yeni Aktör
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

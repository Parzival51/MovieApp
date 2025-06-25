import React, { useCallback, useState, useMemo } from 'react';
import useFetch from '../../hooks/useFetch';
import { getAllUsers, deleteUser, updateUserRoles } from '../../api/users';
import DataTable from '../../components/DataTable';
import RoleModal from '../../components/RoleModal';

export default function AdminUsersPage() {
  const { data, loading, error, refetch } = useFetch(getAllUsers, []);
  const users = Array.isArray(data) ? data : [];

  const [editing, setEditing] = useState(null);
  const [sortKey, setSortKey] = useState('displayName');
  const [sortDir, setSortDir] = useState('asc');

  const sorted = useMemo(() => {
    const arr = [...users].sort((a,b) =>
      (a[sortKey] || '').toString().localeCompare((b[sortKey]||'').toString(),'tr')
    );
    return sortDir === 'asc' ? arr : arr.reverse();
  }, [users, sortKey, sortDir]);

  const handleSort = (k,d) => { setSortKey(k); setSortDir(d); };

  const handleDelete = useCallback(async id => {
    if (!window.confirm('Bu kullanıcıyı silmek istediğinizden emin misiniz?')) return;
    await deleteUser(id);
    refetch();
  }, [refetch]);

  const handleSaveRoles = async roles => {
    await updateUserRoles(editing.id, roles);
    setEditing(null);
    refetch();
  };

  const columns = [
    { key:'displayName', label:'Ad',     sortable:true },
    { key:'email',       label:'E-posta',sortable:true },
    { key:'roleList',    label:'Roller' },
    { key:'actions',     label:'İşlemler' }
  ];

  const renderRow = u => (
    <tr key={u.id} className="border-t">
      <td className="px-4 py-2">{u.displayName || u.userName}</td>
      <td className="px-4 py-2">{u.email}</td>
      <td className="px-4 py-2 space-x-1">
        {(u.roles || []).map(r => (
          <span key={r} className="inline-block px-2 py-0.5 bg-secondary/10 text-secondary rounded text-xs">
            {r}
          </span>
        ))}
      </td>
      <td className="px-4 py-2 space-x-2">
        <button onClick={() => setEditing(u)} className="text-secondary hover:underline">Rolleri Düzenle</button>
        <button onClick={() => handleDelete(u.id)} className="text-danger hover:underline">Sil</button>
      </td>
    </tr>
  );

  if (error) return <div className="p-4 text-danger">Hata: {error.message}</div>;

  return (
    <div className="container mx-auto px-4 py-6 space-y-4">
      <h2 className="text-2xl font-bold">Kullanıcı Yönetimi</h2>

      <DataTable
        columns={columns}
        data={sorted}
        loading={loading}
        sortKey={sortKey}
        sortDir={sortDir}
        onSort={handleSort}
        renderRow={renderRow}
      />

      {editing && (
        <RoleModal
          open
          user={editing}
          onClose={() => setEditing(null)}
          onSave={handleSaveRoles}
        />
      )}
    </div>
  );
}

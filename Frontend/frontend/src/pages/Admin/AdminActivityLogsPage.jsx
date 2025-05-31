import React, { useState, useMemo } from 'react';
import useFetch from '../../hooks/useFetch';
import { getActivityLogs } from '../../api/activityLogs';
import DataTable from '../../components/DataTable';
import Button from '../../components/Button';

export default function AdminActivityLogsPage() {
  const [filters, setFilters] = useState({ from:'', to:'', action:'' });

  const { data: raw = [], loading, error, refetch } = useFetch(
    () => getActivityLogs({ ...filters, page:1, pageSize:100 }),
    [filters]
  );
  const logs = Array.isArray(raw) ? raw : [];

  const [sortKey, setSortKey] = useState('timestamp');
  const [sortDir, setSortDir] = useState('desc');
  const sorted = useMemo(() => {
    const arr = [...logs];
    if (sortKey === 'timestamp') {
      arr.sort((a,b) => new Date(a.timestamp) - new Date(b.timestamp));
    } else {
      arr.sort((a,b) =>
        (a[sortKey]||'').localeCompare(b[sortKey]||'','tr')
      );
    }
    return sortDir === 'asc' ? arr : arr.reverse();
  }, [logs, sortKey, sortDir]);
  const handleSort = (k,d)=>{ setSortKey(k); setSortDir(d); };

  const onChange = e => setFilters(f => ({ ...f, [e.target.name]: e.target.value }));

  /* kolon */
  const columns = [
    { key:'timestamp',  label:'Zaman',   sortable:true },
    { key:'userName',   label:'Kullanıcı',sortable:true },
    { key:'action',     label:'Aksiyon', sortable:true },
    { key:'entityName', label:'Varlık',  sortable:true },
    { key:'entityId',   label:'ID' }
  ];

  const renderRow = l => (
    <tr key={l.id} className="border-t">
      <td className="px-4 py-2">{new Date(l.timestamp).toLocaleString('tr-TR')}</td>
      <td className="px-4 py-2">{l.userName || 'Sistem'}</td>
      <td className="px-4 py-2">{l.action}</td>
      <td className="px-4 py-2">{l.entityName}</td>
      <td className="px-4 py-2">{l.entityId ?? '—'}</td>
    </tr>
  );

  if (error) return <div className="p-4 text-danger">{String(error)}</div>;

  return (
    <div className="container mx-auto px-4 py-6 space-y-4">
      <h2 className="text-2xl font-bold">Aktivite Logları</h2>

      {/* filtreler */}
      <div className="flex flex-wrap gap-3">
        <input type="date" name="from"  value={filters.from}  onChange={onChange} className="border rounded px-2 py-1"/>
        <input type="date" name="to"    value={filters.to}    onChange={onChange} className="border rounded px-2 py-1"/>
        <input type="text" name="action" value={filters.action} onChange={onChange} placeholder="Aksiyon" className="border rounded px-2 py-1"/>
        <Button variant="secondary" size="sm" onClick={refetch}>Uygula</Button>
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

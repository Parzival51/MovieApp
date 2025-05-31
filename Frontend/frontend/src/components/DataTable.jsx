import React from 'react';
import clsx from 'clsx';
import PropTypes from 'prop-types';

/**
 * columns = [
 *   { key:'title',  label:'Başlık',  sortable:true,  className:'w-1/3' },
 *   { key:'year',   label:'Yıl',     sortable:false },
 *   …
 * ]
 *
 * <DataTable
 *   columns={columns}
 *   data={rows}
 *   sortKey={key}
 *   sortDir={'asc'|'desc'}
 *   onSort={(key,dir)=>…}
 *   loading={false}
 *   renderRow={row => <tr>…</tr>}
 * />
 */
export default function DataTable({
  columns,
  data,
  sortKey,
  sortDir,
  onSort,
  loading = false,
  renderRow
}) {
  /* Skeleton satır sayısı */
  const skeletonRows = 5;

  const toggleSort = key => {
    if (!onSort) return;
    if (sortKey === key) {
      onSort(key, sortDir === 'asc' ? 'desc' : 'asc');
    } else {
      onSort(key, 'asc');
    }
  };

  return (
    <table className="min-w-full bg-surface shadow rounded overflow-hidden text-sm">
      <thead className="bg-muted/10 text-surface-900 dark:text-surface-100">
        <tr>
          {columns.map(col => (
            <th
              key={col.key}
              onClick={() => col.sortable && toggleSort(col.key)}
              className={clsx(
                'px-4 py-2 text-left select-none',
                col.className,
                col.sortable && 'cursor-pointer hover:bg-muted/20'
              )}
            >
              {col.label}
              {col.sortable && sortKey === col.key && (
                <span className="ml-1">
                  {sortDir === 'asc' ? '▲' : '▼'}
                </span>
              )}
            </th>
          ))}
        </tr>
      </thead>

      <tbody>
        {loading
          ? Array.from({ length: skeletonRows }).map((_, i) => (
              <tr key={i} className="animate-pulse">
                {columns.map(col => (
                  <td key={col.key} className="px-4 py-2">
                    <div className="h-4 bg-muted/20 rounded" />
                  </td>
                ))}
              </tr>
            ))
          : data.map(renderRow)}
      </tbody>
    </table>
  );
}

DataTable.propTypes = {
  columns:  PropTypes.array.isRequired,
  data:     PropTypes.array.isRequired,
  sortKey:  PropTypes.string,
  sortDir:  PropTypes.oneOf(['asc', 'desc']),
  onSort:   PropTypes.func,
  loading:  PropTypes.bool,
  renderRow:PropTypes.func.isRequired
};

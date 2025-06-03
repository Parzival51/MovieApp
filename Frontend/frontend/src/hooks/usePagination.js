import { useState, useMemo } from 'react';

/**
 * usePagination: sayfalama mantığını yönetir.
 *
 * @param {number} totalItems - toplam öğe sayısı
 * @param {number} pageSize - sayfa başına öğe sayısı
 * @param {number} initialPage - başlangıç sayfa (default 1)
 * @returns {{ currentPage: number, setPage: Function,
 *            totalPages: number, offset: number, limit: number }}
 */
export default function usePagination(totalItems, pageSize, initialPage = 1) {
  const [currentPage, setCurrentPage] = useState(initialPage);

  const totalPages = useMemo(() => {
    return Math.max(1, Math.ceil(totalItems / pageSize));
  }, [totalItems, pageSize]);

  const offset = useMemo(() => (currentPage - 1) * pageSize, [currentPage, pageSize]);
  const limit = pageSize;

  function setPage(page) {
    const p = Math.min(totalPages, Math.max(1, page));
    setCurrentPage(p);
  }

  return { currentPage, setPage, totalPages, offset, limit };
}

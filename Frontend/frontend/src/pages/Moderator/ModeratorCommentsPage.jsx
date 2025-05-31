import React from 'react';
import useFetch from '../../hooks/useFetch';
import {
  getPendingComments,
  approveComment,
  deleteComment
} from '../../api/comments';

export default function ModeratorCommentsPage() {
  const {
    data: raw = [],
    loading,
    error,
    refetch
  } = useFetch(getPendingComments, []);

  const comments = Array.isArray(raw) ? raw : [];

  const onApprove = async (c) => {
    await approveComment(c.id);
    refetch();
  };

  const onDelete = async (id) => {
    if (!window.confirm('Bu yorumu silmek istediğinize emin misiniz?')) return;
    await deleteComment(id);
    refetch();
  };

  if (loading) return <div className="py-10 text-center">Yükleniyor…</div>;
  if (error)   return <div className="py-10 text-center text-red-500">Hata: {error.message}</div>;

  return (
    <div className="container mx-auto px-4 py-6">
      <h1 className="text-3xl font-bold mb-6">Onay Bekleyen Yorumlar</h1>
      {comments.length === 0 ? (
        <div>Bekleyen yorum yok.</div>
      ) : (
        <ul className="space-y-4">
          {comments.map(c => (
            <li key={c.id}
                className="bg-surface p-4 shadow-sm rounded text-surface-900 dark:text-surface-100">
              <div className="flex items-start justify-between">
                <div>
                  <p className="text-sm mb-2">{c.content}</p>
                  <span className="text-xs text-muted">
                    {c.userName} • {new Date(c.createdAt).toLocaleDateString('tr-TR')}
                  </span>
                </div>
                <div className="space-x-2">
                                    <button
                    onClick={() => onApprove(c)}
                    className="px-3 py-1 rounded bg-success text-white hover:bg-success/90"
                  >
                    Onayla
                  </button>
                  <button
                    onClick={() => onDelete(c.id)}
                    className="px-3 py-1 rounded bg-danger text-white hover:bg-danger/90"
                  >
                    Sil
                  </button>
                </div>
              </div>
            </li>
          ))}
        </ul>
      )}
    </div>
  );
}

import React from 'react';
import useFetch from '../../hooks/useFetch';
import {
  getPendingReviews,
  approveReview,
  deleteReview
} from '../../api/reviews';

export default function ModeratorReviewPage() {
  const {
    data: raw = [],   
    loading,
    error,
    refetch
  } = useFetch(getPendingReviews, []);

  const reviews = Array.isArray(raw) ? raw : [];

  const onApprove = async (r) => {
    try {
      await approveReview(r.id, {
        id:         r.id,
        title:      r.title,
        content:    r.content,
        stars:      r.stars,
        isApproved: true
      });
      refetch();
    } catch (err) {
      console.error(err);
      alert('Onaylama sırasında hata oluştu.');
    }
  };

  const onDelete = async (id) => {
    if (!window.confirm('İncelemeyi silmek istediğinize emin misiniz?')) return;
    await deleteReview(id);
    refetch();
  };

  if (loading) return <div className="py-10 text-center">Yükleniyor…</div>;
  if (error)   return <div className="py-10 text-center text-red-500">Hata: {error.message}</div>;

  return (
    <div className="container mx-auto px-4 py-6">
      <h1 className="text-3xl font-bold mb-6">Onay Bekleyen İncelemeler</h1>

      {reviews.length === 0 ? (
        <div>Bekleyen inceleme yok.</div>
      ) : (
        <ul className="space-y-4">
          {reviews.map(r => (
            <li key={r.id} className="bg-white p-4 shadow rounded">
              <div className="flex items-start justify-between">
                <div>
                  <h4 className="font-semibold">{r.title}</h4>
                  <p className="text-sm text-gray-600">{r.content}</p>
                  <span className="text-xs text-gray-400">
                    {r.userName} • {new Date(r.createdAt).toLocaleDateString('tr-TR')}
                  </span>
                </div>
                <div className="space-x-2">
                  <button
                    onClick={() => onApprove(r)}
                    className="px-3 py-1 bg-green-600 text-white rounded"
                  >
                    Onayla
                  </button>
                  <button
                    onClick={() => onDelete(r.id)}
                    className="px-3 py-1 bg-red-600 text-white rounded"
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

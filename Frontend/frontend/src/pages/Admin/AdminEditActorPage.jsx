// src/pages/AdminEditActorPage.jsx
import React, { useState } from 'react';
import { useNavigate, useParams } from 'react-router-dom';
import useFetch from '../../hooks/useFetch';
import { getActorById, updateActor } from '../../api/actors';
import ActorFormWizard from '../../components/ActorFormWizard';

export default function AdminEditActorPage() {
  const { id } = useParams();
  const nav    = useNavigate();

  const { data, loading, error: fetchErr } = useFetch(
    () => getActorById(id),
    [id]
  );

  const [saving, setSaving] = useState(false);
  const [error,  setError ] = useState(null);

  const handleSave = async form => {
    try {
      setSaving(true);
      await updateActor({ id, ...form });
      nav('/admin/actors', {
        replace: true,
        state: { shouldRefresh: true }
      });
    } catch (e) {
      setError(e.response?.data || e.message);
    } finally {
      setSaving(false);
    }
  };

  if (loading)   return <div className="p-4">Yükleniyor…</div>;
  if (fetchErr)  return <div className="p-4 text-danger">Hata: {fetchErr.message}</div>;

  return (
    <div className="container mx-auto px-4 py-6">
      <h2 className="text-2xl font-bold mb-4">Oyuncu Düzenle</h2>

      <ActorFormWizard
        initial={data}
        loading={saving}
        error={error}
        onSave={handleSave}
      />
    </div>
  );
}

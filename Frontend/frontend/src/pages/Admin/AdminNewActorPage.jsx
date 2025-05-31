import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import { createActor } from '../../api/actors';
import ActorFormWizard from '../../components/ActorFormWizard';

export default function AdminNewActorPage() {
  const navigate = useNavigate();
  const [error,  setError ] = useState(null);
  const [saving, setSaving] = useState(false);

  const handleSave = async data => {
    try {
      setSaving(true);
      await createActor(data);
      navigate('/admin/actors', { replace: true });
    } catch (e) {
      setError(e.response?.data || e.message);
    } finally {
      setSaving(false);
    }
  };

  return (
    <div className="container mx-auto px-4 py-6">
      <h2 className="text-2xl font-bold mb-4">Yeni Oyuncu</h2>

      <ActorFormWizard
        loading={saving}
        error={error}
        onSave={handleSave}
      />
    </div>
  );
}

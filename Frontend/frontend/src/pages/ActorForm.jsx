// src/components/ActorForm.jsx
import React, { useState, useEffect } from 'react';

export default function ActorForm({ initial, onSubmit, saving }) {
  const [form, setForm] = useState({
    name:       '',
    birthDate:  '',
    photoUrl:   '',
    bio:        ''
  });

  useEffect(() => {
    if (initial) setForm({
      name:      initial.name      ?? '',
      birthDate: initial.birthDate ? initial.birthDate.split('T')[0] : '',
      photoUrl:  initial.photoUrl  ?? '',
      bio:       initial.bio       ?? ''
    });
  }, [initial]);

  const handleChange = e => {
    const { name, value } = e.target;
    setForm(f => ({ ...f, [name]: value }));
  };

  return (
    <form
      onSubmit={e => { e.preventDefault(); onSubmit(form); }}
      className="space-y-4 max-w-md"
    >
      <div>
        <label className="block mb-1">Ad</label>
        <input
          name="name"
          value={form.name}
          onChange={handleChange}
          required
          className="w-full border px-3 py-2 rounded"
        />
      </div>

      <div>
        <label className="block mb-1">Doğum Tarihi</label>
        <input
          type="date"
          name="birthDate"
          value={form.birthDate}
          onChange={handleChange}
          className="w-full border px-3 py-2 rounded"
        />
      </div>

      <div>
        <label className="block mb-1">Fotoğraf URL</label>
        <input
          name="photoUrl"
          value={form.photoUrl}
          onChange={handleChange}
          className="w-full border px-3 py-2 rounded"
        />
      </div>

      <div>
        <label className="block mb-1">Biyografi</label>
        <textarea
          name="bio"
          value={form.bio}
          onChange={handleChange}
          rows={4}
          className="w-full border px-3 py-2 rounded"
        />
      </div>

      <button
        type="submit"
        disabled={saving}
        className="px-6 py-2 bg-green-500 text-white rounded hover:bg-green-600"
      >
        {saving ? 'Kaydediliyor…' : 'Kaydet'}
      </button>
    </form>
  );
}

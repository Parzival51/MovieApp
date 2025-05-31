import React, { useState } from "react";
import { useNavigate } from "react-router-dom";
import { createGenre } from "../../api/genres";

export default function AdminNewGenrePage() {
  const navigate = useNavigate();
  const [name,   setName]   = useState("");
  const [error,  setError]  = useState(null);
  const [saving, setSaving] = useState(false);

  const submit = async e => {
    e.preventDefault();
    setSaving(true);
    setError(null);
    try {
      await createGenre({ name });
      navigate("/admin/genres", { replace: true });
    } catch (err) {
      setError(err.response?.data || err.message);
    } finally {
      setSaving(false);
    }
  };

  return (
    <div className="container mx-auto px-4 py-6">
      <h2 className="text-2xl font-bold mb-4">Yeni Tür</h2>

      {error && <div className="mb-4 p-2 bg-red-100 text-red-800 rounded">{String(error)}</div>}

      <form onSubmit={submit} className="space-y-4 max-w-md">
        <div>
          <label className="block mb-1">Ad</label>
          <input
            value={name}
            onChange={e => setName(e.target.value)}
            required
            className="w-full border px-3 py-2 rounded"
          />
        </div>
        <button
          type="submit"
          disabled={saving}
          className="px-6 py-2 bg-green-500 text-white rounded hover:bg-green-600"
        >
          {saving ? "Kaydediliyor…" : "Kaydet"}
        </button>
      </form>
    </div>
  );
}

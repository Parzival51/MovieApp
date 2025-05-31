import React, { useState, useEffect } from "react";
import { useNavigate, useParams } from "react-router-dom";
import { getGenreById, updateGenre } from "../../api/genres";

export default function AdminEditGenrePage() {
  const { id }   = useParams();
  const navigate = useNavigate();

  const [name,   setName]   = useState("");
  const [error,  setError]  = useState(null);
  const [saving, setSaving] = useState(false);

  useEffect(() => {
    getGenreById(id)
      .then(g => setName(g.name))
      .catch(err => setError(err.response?.data || err.message));
  }, [id]);

  const submit = async e => {
    e.preventDefault();
    setSaving(true);
    setError(null);
    try {
      await updateGenre({ id, name });
      navigate("/admin/genres", { replace: true });
    } catch (err) {
      setError(err.response?.data || err.message);
    } finally {
      setSaving(false);
    }
  };

  return (
    <div className="container mx-auto px-4 py-6">
      <h2 className="text-2xl font-bold mb-4">Türü Düzenle</h2>

      {error && (
        <div className="mb-4 p-2 bg-red-100 text-red-800 rounded">
          {String(error)}
        </div>
      )}

      <form onSubmit={submit} className="space-y-4 max-w-md">
        <div>
          <label className="block mb-1">Ad</label>
          <input
            value={name}
            onChange={e => setName(e.target.value)}
            required
            className="field"
          />
        </div>
        <button
          type="submit"
          disabled={saving}
          className="px-6 py-2 bg-blue-600 text-white rounded hover:bg-blue-700"
        >
          {saving ? "Güncelleniyor…" : "Güncelle"}
        </button>
      </form>
    </div>
  );
}

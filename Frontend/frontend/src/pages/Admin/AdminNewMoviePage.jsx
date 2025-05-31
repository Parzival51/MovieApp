// src/pages/AdminNewMoviePage.jsx
import React, { useState, useCallback } from 'react';
import { useNavigate } from 'react-router-dom';
import useFetch from '../../hooks/useFetch';
import { createMovie } from '../../api/movies';
import { getAllGenres } from '../../api/genres';
import { getFeaturedActors as getAllActors } from '../../api/actors';
import { getAllDirectors } from '../../api/directors';
import Button from '../../components/Button';

const STEPS = ['Detaylar', 'Medya', 'İlişkiler'];

export default function AdminNewMoviePage() {
  const navigate = useNavigate();

  const [form, setForm] = useState({
    title: '',
    description: '',
    tagline: '',
    status: '',
    releaseDate: '',
    duration: '',
    language: '',
    country: '',
    posterUrl: '',
    trailerUrl: '',
    homepageUrl: '',
    backdropPath: '',
    budget: '',
    revenue: '',
    imdbId: '',
    isAdult: false,
    genreIds: [],
    actorIds: [],
    directorIds: []
  });

  const [step, setStep]     = useState(0);
  const [error, setError]   = useState(null);
  const [saving, setSaving] = useState(false);

  const { data: genres    = [] } = useFetch(getAllGenres, []);
  const { data: actors    = [] } = useFetch(() => getAllActors(150), []);
  const { data: directors = [] } = useFetch(getAllDirectors, []);

  const handleChange = e => {
    const { name, value, type, checked } = e.target;
    setForm(f => ({
      ...f,
      [name]: type === 'checkbox' ? checked : value
    }));
  };
  const handleMulti = field => e => {
    const vals = Array.from(e.target.selectedOptions).map(o => o.value);
    setForm(f => ({ ...f, [field]: vals }));
  };

  const next = () => setStep(s => Math.min(s + 1, STEPS.length - 1));
  const prev = () => setStep(s => Math.max(s - 1, 0));

  const validateStep = () => {
    if (step === 0) {
      const required = ['title','description','releaseDate','duration','language','country'];
      return required.every(k => form[k]?.toString().trim());
    }
    if (step === 1) return Boolean(form.posterUrl);
    return true; 
  };

  const handleSubmit = useCallback(async () => {
    setSaving(true);
    setError(null);
    try {
      await createMovie({
        ...form,
        releaseDate: new Date(form.releaseDate),
        duration: Number(form.duration),
        budget: Number(form.budget),
        revenue: Number(form.revenue),
        isAdult: form.isAdult
      });
      navigate('/admin/movies', { replace: true });
    } catch (err) {
      setError(err.response?.data || err.message);
    } finally {
      setSaving(false);
    }
  }, [form, navigate]);

  return (
    <div className="container mx-auto px-4 py-6 max-w-2xl space-y-6">
      <h2 className="text-2xl font-bold">Yeni Film · {STEPS[step]}</h2>

      {/* Step Indicator */}
      <div className="flex items-center gap-4">
        {STEPS.map((label, i) => (
          <div key={label} className="flex items-center gap-2">
            <span
              className={`w-6 h-6 rounded-full text-xs flex items-center justify-center
                ${i === step ? 'bg-secondary text-black'
                  : i < step ? 'bg-success text-white'
                  : 'bg-muted/30'}`}
            >
              {i + 1}
            </span>
            <span className={i === step ? 'font-medium' : 'text-muted'}>
              {label}
            </span>
            {i < STEPS.length - 1 && <div className="flex-1 h-px bg-muted/30" />}
          </div>
        ))}
      </div>

      {error && (
        <div className="p-2 bg-danger/10 text-danger rounded">{error}</div>
      )}

      {/* Step Content */}
      {step === 0 && (
        <div className="space-y-4">
          <input
            name="title"
            placeholder="Başlık"
            value={form.title}
            onChange={handleChange}
            className="field"
            required
          />
          <textarea
            name="description"
            placeholder="Açıklama"
            value={form.description}
            onChange={handleChange}
            className="field"
            rows={3}
            required
          />
          <input
            name="tagline"
            placeholder="Tagline"
            value={form.tagline}
            onChange={handleChange}
            className="field"
          />
          <input
            name="status"
            placeholder="Status"
            value={form.status}
            onChange={handleChange}
            className="field"
          />
          <div className="grid grid-cols-2 gap-4">
            <input
              type="date"
              name="releaseDate"
              value={form.releaseDate}
              onChange={handleChange}
              className="field"
              required
            />
            <input
              type="number"
              name="duration"
              placeholder="Süre (dk)"
              value={form.duration}
              onChange={handleChange}
              className="field"
              required
            />
          </div>
          <div className="grid grid-cols-3 gap-4">
            <input
              name="language"
              placeholder="Dil"
              value={form.language}
              onChange={handleChange}
              className="field"
              required
            />
            <input
              name="country"
              placeholder="Ülke"
              value={form.country}
              onChange={handleChange}
              className="field"
              required
            />
            <label className="flex items-center space-x-2">
              <input
                type="checkbox"
                name="isAdult"
                checked={form.isAdult}
                onChange={handleChange}
              />
              <span>Yetişkin</span>
            </label>
          </div>
          <div className="grid grid-cols-2 gap-4">
            <input
              type="number"
              name="budget"
              placeholder="Budget"
              value={form.budget}
              onChange={handleChange}
              className="field"
            />
            <input
              type="number"
              name="revenue"
              placeholder="Revenue"
              value={form.revenue}
              onChange={handleChange}
              className="field"
            />
          </div>
          <input
            name="imdbId"
            placeholder="IMDb ID"
            value={form.imdbId}
            onChange={handleChange}
            className="field"
          />
        </div>
      )}

      {step === 1 && (
        <div className="space-y-4">
          <input
            name="posterUrl"
            placeholder="Poster URL"
            value={form.posterUrl}
            onChange={handleChange}
            className="field"
            required
          />
          <input
            name="trailerUrl"
            placeholder="Fragman URL"
            value={form.trailerUrl}
            onChange={handleChange}
            className="field"
          />
          <input
            name="homepageUrl"
            placeholder="Homepage URL"
            value={form.homepageUrl}
            onChange={handleChange}
            className="field"
          />
          <input
            name="backdropPath"
            placeholder="Backdrop Path"
            value={form.backdropPath}
            onChange={handleChange}
            className="field"
          />
        </div>
      )}

      {step === 2 && (
        <div className="space-y-4">
          <select
            multiple
            className="field h-32"
            value={form.genreIds}
            onChange={handleMulti('genreIds')}
          >
            {genres.map(g => (
              <option key={g.id} value={g.id}>
                {g.name}
              </option>
            ))}
          </select>
          <select
            multiple
            className="field h-32"
            value={form.actorIds}
            onChange={handleMulti('actorIds')}
          >
            {actors.map(a => (
              <option key={a.id} value={a.id}>
                {a.name}
              </option>
            ))}
          </select>
          <select
            multiple
            className="field h-32"
            value={form.directorIds}
            onChange={handleMulti('directorIds')}
          >
            {directors.map(d => (
              <option key={d.id} value={d.id}>
                {d.name}
              </option>
            ))}
          </select>
        </div>
      )}

      {/* Navigation Buttons */}
      <div className="flex justify-between pt-4">
        <Button variant="secondary" size="sm" onClick={prev} disabled={step === 0}>
          Geri
        </Button>
        {step < STEPS.length - 1 ? (
          <Button variant="primary" size="sm" onClick={next} disabled={!validateStep()}>
            İleri
          </Button>
        ) : (
          <Button
            variant="success"
            size="sm"
            onClick={handleSubmit}
            disabled={saving || !validateStep()}
          >
            {saving ? 'Kaydediliyor…' : 'Kaydet'}
          </Button>
        )}
      </div>
    </div>
  );
}

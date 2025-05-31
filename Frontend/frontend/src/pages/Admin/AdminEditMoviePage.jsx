// src/pages/AdminEditMoviePage.jsx
import React, { useState, useEffect } from "react";
import { useNavigate, useParams } from "react-router-dom";
import useFetch from "../../hooks/useFetch";
import { getMovieById, updateMovie } from "../../api/movies";
import { getAllGenres } from "../../api/genres";
import { getFeaturedActors as getAllActors } from "../../api/actors";
import { getAllDirectors } from "../../api/directors";
import Button from "../../components/Button";

const STEPS = ["Detaylar", "Medya", "İlişkiler"];

export default function AdminEditMoviePage() {
  const { id } = useParams();
  const nav = useNavigate();

  const {
    data: movie,
    loading: loadMovie,
    error: errMovie,
  } = useFetch(() => getMovieById(id), [id]);
  const { data: genres = [] } = useFetch(getAllGenres, []);
  const { data: actors = [] } = useFetch(() => getAllActors(150), []);
  const { data: directors = [] } = useFetch(getAllDirectors, []);

  const [form, setForm] = useState(null);
  const [step, setStep] = useState(0);
  const [saving, setSaving] = useState(false);
  const [error, setError] = useState(null);

  useEffect(() => {
    if (!movie) return;
    setForm({
      title: movie.title ?? "",
      description: movie.description ?? "",
      releaseDate: movie.releaseDate?.split("T")[0] ?? "",
      duration: String(movie.duration ?? ""),
      language: movie.language ?? "",
      country: movie.country ?? "",
      tagline: movie.tagline ?? "",
      status: movie.status ?? "",
      budget: movie.budget ?? 0,
      revenue: movie.revenue ?? 0,
      imdbId: movie.imdbId ?? "",
      homepageUrl: movie.homepageUrl ?? "",
      isAdult: movie.isAdult ?? false,
      posterUrl: movie.posterUrl ?? "",
      trailerUrl: movie.trailerUrl ?? "",
      backdropPath: movie.backdropPath ?? "",
      genreIds: (movie.genres ?? []).map((g) => g.id),
      actorIds: (movie.actors ?? []).map((a) => a.id),
      directorIds: (movie.directors ?? []).map((d) => d.id),
    });
  }, [movie]);

  const handle = (e) =>
    setForm((f) => ({ ...f, [e.target.name]: e.target.value }));
  const handleCheckbox = (e) =>
    setForm((f) => ({ ...f, [e.target.name]: e.target.checked }));
  const multi = (field) => (e) =>
    setForm((f) => ({
      ...f,
      [field]: Array.from(e.target.selectedOptions).map((o) => o.value),
    }));

  const next = () => setStep((s) => Math.min(s + 1, STEPS.length - 1));
  const prev = () => setStep((s) => Math.max(s - 1, 0));

  const valid = () => {
    if (!form) return false;
    if (step === 0)
      return [
        "title",
        "description",
        "releaseDate",
        "duration",
        "language",
        "country",
      ].every((k) => form[k]?.toString().trim());
    if (step === 1) return !!form.posterUrl;
    return true;
  };

  const submit = async () => {
    setSaving(true);
    setError(null);
    try {
      await updateMovie({
        id,
        ...form,
        duration: Number(form.duration),
        releaseDate: new Date(form.releaseDate),
        budget: Number(form.budget),
        revenue: Number(form.revenue),
      });
      nav("/admin/movies", { replace: true });
    } catch (e) {
      const pd = e.response?.data;
      let msg;
      if (pd?.errors) {
        msg = Object.values(pd.errors)
          .flat()
          .join(" ");
      } else if (pd?.title) {
        msg = pd.title;
      } else {
        msg = e.message;
      }
      setError(msg);
    } finally {
      setSaving(false);
    }
  };

  /* UI durumları */
  if (loadMovie)
    return <div className="p-4">Yükleniyor…</div>;
  if (errMovie)
    return <div className="p-4 text-danger">Hata: {errMovie.message}</div>;
  if (!form) return null;

  return (
    <div className="container mx-auto px-4 py-6 max-w-2xl space-y-6">
      <h2 className="text-2xl font-bold">
        Filmi Düzenle · {STEPS[step]}
      </h2>

      {/* stepper */}
      <div className="flex items-center gap-4">
        {STEPS.map((label, i) => (
          <div key={label} className="flex items-center gap-2">
            <span
              className={`w-6 h-6 flex items-center justify-center rounded-full text-xs ${
                i === step
                  ? "bg-secondary text-black"
                  : i < step
                  ? "bg-success text-white"
                  : "bg-muted/30"
              }`}
            >
              {i + 1}
            </span>
            <span className={i === step ? "font-medium" : "text-muted"}>
              {label}
            </span>
            {i < STEPS.length - 1 && (
              <div className="flex-1 h-px bg-muted/30" />
            )}
          </div>
        ))}
      </div>

      {error && (
        <div className="p-2 bg-danger/10 text-danger rounded">
          {error}
        </div>
      )}

      {/* içerik */}
      {step === 0 && (
        <div className="space-y-4">
          <input
            className="field"
            name="title"
            value={form.title}
            onChange={handle}
            placeholder="Başlık"
            required
          />
          <textarea
            className="field"
            name="description"
            value={form.description}
            onChange={handle}
            rows={3}
            placeholder="Açıklama"
            required
          />
          <div className="grid grid-cols-2 gap-4">
            <input
              type="date"
              className="field"
              name="releaseDate"
              value={form.releaseDate}
              onChange={handle}
              required
            />
            <input
              type="number"
              className="field"
              name="duration"
              value={form.duration}
              onChange={handle}
              placeholder="Süre (dk)"
              required
            />
          </div>
          <div className="grid grid-cols-2 gap-4">
            <input
              className="field"
              name="language"
              value={form.language}
              onChange={handle}
              placeholder="Dil"
              required
            />
            <input
              className="field"
              name="country"
              value={form.country}
              onChange={handle}
              placeholder="Ülke"
              required
            />
          </div>
          <input
            className="field"
            name="tagline"
            value={form.tagline}
            onChange={handle}
            placeholder="Tagline"
          />
          <input
            className="field"
            name="status"
            value={form.status}
            onChange={handle}
            placeholder="Status"
          />
          <div className="grid grid-cols-2 gap-4">
            <input
              type="number"
              className="field"
              name="budget"
              value={form.budget}
              onChange={handle}
              placeholder="Budget"
            />
            <input
              type="number"
              className="field"
              name="revenue"
              value={form.revenue}
              onChange={handle}
              placeholder="Revenue"
            />
          </div>
          <div className="grid grid-cols-2 gap-4">
            <input
              className="field"
              name="imdbId"
              value={form.imdbId}
              onChange={handle}
              placeholder="IMDb ID"
            />
            <input
              className="field"
              name="homepageUrl"
              value={form.homepageUrl}
              onChange={handle}
              placeholder="Homepage URL"
            />
          </div>
          <label className="inline-flex items-center space-x-2">
            <input
              type="checkbox"
              name="isAdult"
              checked={form.isAdult}
              onChange={handleCheckbox}
            />
            <span>Yetişkin İçerik</span>
          </label>
        </div>
      )}

      {step === 1 && (
        <div className="space-y-4">
          <input
            className="field"
            name="posterUrl"
            value={form.posterUrl}
            onChange={handle}
            placeholder="Poster URL"
            required
          />
          <input
            className="field"
            name="trailerUrl"
            value={form.trailerUrl}
            onChange={handle}
            placeholder="Fragman URL"
          />
          <input
            className="field"
            name="backdropPath"
            value={form.backdropPath}
            onChange={handle}
            placeholder="Backdrop Path URL"
          />
        </div>
      )}

      {step === 2 && (
        <div className="space-y-4">
          <select
            multiple
            className="field h-32"
            value={form.genreIds}
            onChange={multi("genreIds")}
          >
            {genres.map((g) => (
              <option key={g.id} value={g.id}>
                {g.name}
              </option>
            ))}
          </select>
          <select
            multiple
            className="field h-32"
            value={form.actorIds}
            onChange={multi("actorIds")}
          >
            {actors.map((a) => (
              <option key={a.id} value={a.id}>
                {a.name}
              </option>
            ))}
          </select>
          <select
            multiple
            className="field h-32"
            value={form.directorIds}
            onChange={multi("directorIds")}
          >
            {directors.map((d) => (
              <option key={d.id} value={d.id}>
                {d.name}
              </option>
            ))}
          </select>
        </div>
      )}

      {/* navigation */}
      <div className="flex justify-between pt-4">
        <Button
          variant="secondary"
          size="sm"
          onClick={prev}
          disabled={step === 0}
        >
          Geri
        </Button>
        {step < STEPS.length - 1 ? (
          <Button
            variant="primary"
            size="sm"
            onClick={next}
            disabled={!valid()}
          >
            İleri
          </Button>
        ) : (
          <Button
            variant="success"
            size="sm"
            onClick={submit}
            disabled={saving || !valid()}
          >
            {saving ? "Kaydediliyor…" : "Güncelle"}
          </Button>
        )}
      </div>
    </div>
  );
}

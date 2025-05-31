import React, { useState, useEffect } from 'react';
import Button from './Button';

export default function ActorFormWizard({ initial = null, loading = false, onSave, error }) {
  const [step, setStep] = useState(0);
  const [saving, setSaving] = useState(false);
  const [form, setForm] = useState({
    name: '',
    birthDate: '',
    photoUrl: '',
    bio: ''
  });

  // Populate form when initial data arrives
  useEffect(() => {
    if (initial) {
      setForm({
        name: initial.name ?? '',
        birthDate: initial.birthday ? initial.birthday.split('T')[0] : '',
        photoUrl: initial.photoUrl ?? '',
        bio: initial.biography ?? ''
      });
    }
  }, [initial]);

  const handleChange = e => {
    const { name, value } = e.target;
    setForm(f => ({ ...f, [name]: value }));
  };
  
  const valid = step === 0
    ? form.name.trim().length > 0
    : Boolean(form.photoUrl);

  const next = () => setStep(1);
  const prev = () => setStep(0);

  const submit = async () => {
    setSaving(true);
    await onSave(form);
    setSaving(false);
  };

  return (
    <div className="space-y-6 max-w-md">
      {error && <div className="p-2 bg-danger/10 text-danger rounded">{error}</div>}

      {/* Step indicator */}
      <div className="flex gap-4">
        <span className={`step-dot ${step === 0 ? 'active' : ''}`}>1</span> Bilgi
        <div className="flex-1 h-px bg-muted/30" />
        <span className={`step-dot ${step === 1 ? 'active' : ''}`}>2</span> Medya
      </div>

      {/* Content */}
      {step === 0 ? (
        <div className="space-y-4">
          <input
            className="field"
            name="name"
            value={form.name}
            onChange={handleChange}
            placeholder="İsim"
            required
          />
          <input
            type="date"
            className="field"
            name="birthDate"
            value={form.birthDate}
            onChange={handleChange}
          />
          <textarea
            className="field"
            name="bio"
            rows={4}
            value={form.bio}
            onChange={handleChange}
            placeholder="Biyografi"
          />
        </div>
      ) : (
        <div className="space-y-4">
          <input
            className="field"
            name="photoUrl"
            value={form.photoUrl}
            onChange={handleChange}
            placeholder="Fotoğraf URL"
            required
          />
        </div>
      )}

      {/* Navigation buttons */}
      <div className="flex justify-between">
        <Button
          variant="secondary"
          size="sm"
          onClick={prev}
          disabled={step === 0}
        >
          Geri
        </Button>
        {step === 0 ? (
          <Button
            variant="primary"
            size="sm"
            onClick={next}
            disabled={!valid}
          >
            İleri
          </Button>
        ) : (
          <Button
            variant="success"
            size="sm"
            onClick={submit}
            disabled={saving || !valid}
          >
            {saving ? 'Kaydediliyor…' : 'Kaydet'}
          </Button>
        )}
      </div>
    </div>
  );
}

/* Helper step-dot class in index.css:
.step-dot { @apply w-6 h-6 flex items-center justify-center text-xs rounded-full bg-muted/30; }
.step-dot.active { @apply bg-secondary text-black; }
*/

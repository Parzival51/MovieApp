// src/pages/AuthPages/ResendConfirmationPage.jsx
import React, { useState } from 'react';
import Layout from '../../components/Layout';
import Field  from '../../components/Field';
import Button from '../../components/Button';

export default function ResendConfirmationPage() {
  const [email,    setEmail]    = useState('');
  const [error,    setError]    = useState(null);
  const [info,     setInfo]     = useState(null);
  const [loading,  setLoading]  = useState(false);

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError(null);
    setInfo(null);
    setLoading(true);
    try {
      const res = await fetch('https://localhost:7176/api/auth/resend-confirmation', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ email })
      });
      const text = await res.text();
      if (res.ok) {
        setInfo(text || 'Onay maili gönderildi. Lütfen e-postanızı kontrol edin.');
      } else {
        setError(text || 'Bir hata oluştu.');
      }
    } catch {
      setError('Sunucuya bağlanırken bir hata oluştu.');
    } finally {
      setLoading(false);
    }
  };

  return (
    <Layout>
      <div className="flex items-center justify-center py-20">
        <form
          onSubmit={handleSubmit}
          className="w-full max-w-md space-y-6 bg-surface shadow rounded p-8"
        >
          <div className="text-center">
            <h1 className="text-3xl font-heading mb-2">Onay Maili Tekrar Gönder</h1>
            <p className="text-sm text-muted">
              E-posta adresinizi girin, size onay bağlantısı yeniden gönderilsin.
            </p>
          </div>

          <Field label="E-posta">
            <input
              type="email"
              value={email}
              onChange={(e) => setEmail(e.target.value)}
              required
              className="w-full rounded border border-muted px-3 py-2 bg-transparent focus:border-secondary focus:ring-secondary focus:outline-none"
            />
          </Field>

          {error && <p className="text-danger text-sm">{error}</p>}
          {info  && <p className="text-primary text-sm">{info}</p>}

          <Button
            type="submit"
            variant="primary"
            size="md"
            className="w-full"
            disabled={loading}
          >
            {loading ? 'Gönderiliyor…' : 'Tekrar Gönder'}
          </Button>
        </form>
      </div>
    </Layout>
  );
}
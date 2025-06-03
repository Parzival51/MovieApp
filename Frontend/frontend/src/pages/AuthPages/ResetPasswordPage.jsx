// src/pages/AuthPages/ResetPasswordPage.jsx
import React, { useEffect, useState } from 'react';
import { useSearchParams, useNavigate } from 'react-router-dom';
import Layout from '../../components/Layout';
import Field  from '../../components/Field';
import Button from '../../components/Button';

export default function ResetPasswordPage() {
  const [searchParams] = useSearchParams();
  const navigate = useNavigate();
  const userId = searchParams.get('userId');
  const token  = searchParams.get('token');

  const [newPass,     setNewPass]     = useState('');
  const [confirmPass, setConfirmPass] = useState('');
  const [error,       setError]       = useState(null);
  const [info,        setInfo]        = useState(null);
  const [loading,     setLoading]     = useState(false);

  useEffect(() => {
    if (!userId || !token) {
      setError('Geçersiz veya eksik parametre.');
    }
  }, [userId, token]);

  const handleSubmit = async e => {
    e.preventDefault();
    if (newPass !== confirmPass) {
      setError('Şifreler uyuşmuyor');
      return;
    }
    setLoading(true);
    setError(null);
    setInfo(null);
    try {
      const res = await fetch('https://localhost:7176/api/auth/reset-password', {
        method: 'POST',
        headers: { 'Content-Type': 'application/json' },
        body: JSON.stringify({ userId, token, newPassword: newPass })
      });
      const text = await res.text();
      if (res.ok) {
        setInfo(text || 'Şifre başarıyla sıfırlandı.');
        setTimeout(() => navigate('/login', { replace: true }), 3000);
      } else {
        setError(text || 'Şifre sıfırlama sırasında bir hata oluştu.');
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
            <h1 className="text-3xl font-heading mb-2">Şifre Sıfırla</h1>
            <p className="text-sm text-muted">
              Yeni şifrenizi girin.
            </p>
          </div>

          <Field label="Yeni Şifre">
            <input
              type="password"
              value={newPass}
              onChange={e => setNewPass(e.target.value)}
              required
              className="w-full rounded border border-muted px-3 py-2 bg-transparent focus:border-secondary focus:ring-secondary focus:outline-none"
            />
          </Field>

          <Field label="Şifre (Tekrar)">
            <input
              type="password"
              value={confirmPass}
              onChange={e => setConfirmPass(e.target.value)}
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
            {loading ? 'Kaydediliyor…' : 'Şifreyi Güncelle'}
          </Button>
        </form>
      </div>
    </Layout>
  );
}

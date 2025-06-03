// src/pages/AuthPages/ChangePasswordPage.jsx
import React, { useState } from 'react';
import { useNavigate } from 'react-router-dom';
import Layout from '../../components/Layout';
import Field from '../../components/Field';
import Button from '../../components/Button';
import useAuth from '../../hooks/useAuth';
import client from '../../api/client';

export default function ChangePasswordPage() {
  const { tokens } = useAuth();
  const navigate = useNavigate();

  const [currentPassword, setCurrentPassword]   = useState('');
  const [newPassword, setNewPassword]           = useState('');
  const [confirmNewPassword, setConfirmNewPassword] = useState('');

  const [error, setError]       = useState(null);
  const [info, setInfo]         = useState(null);
  const [loading, setLoading]   = useState(false);

  const handleSubmit = async (e) => {
    e.preventDefault();
    setError(null);
    setInfo(null);

    if (newPassword !== confirmNewPassword) {
      setError('Yeni şifre ile onay şifresi uyuşmuyor.');
      return;
    }

    setLoading(true);
    try {
      const response = await client.post(
        '/auth/change-password',
        { currentPassword, newPassword, confirmNewPassword }
      );
      setInfo(response.data || 'Şifre başarıyla değiştirildi.');
      setTimeout(() => {
        navigate('/', { replace: true });
      }, 2000);
    } catch (err) {
      const text = err.response?.data;
      if (typeof text === 'object') {
        const errors = Object.values(text).flat().join(' ');
        setError(errors);
      } else {
        setError(text || 'Şifre değiştirme sırasında bir hata oluştu.');
      }
    } finally {
      setLoading(false);
    }
  };

  if (!tokens?.accessToken) {
    return (
      <Layout>
        <div className="py-20 text-center">Lütfen önce giriş yapın.</div>
      </Layout>
    );
  }

  return (
    <Layout>
      <div className="flex items-center justify-center py-20">
        <form
          onSubmit={handleSubmit}
          className="w-full max-w-md space-y-6 bg-surface shadow rounded p-8"
        >
          <div className="text-center">
            <h1 className="text-3xl font-heading mb-2">Şifre Değiştir</h1>
            <p className="text-sm text-muted">
              Mevcut şifrenizi girip, yeni şifrenizi belirleyin.
            </p>
          </div>

          {/* Mevcut Şifre */}
          <Field label="Mevcut Şifre">
            <input
              type="password"
              value={currentPassword}
              onChange={(e) => setCurrentPassword(e.target.value)}
              required
              className="w-full rounded border border-muted px-3 py-2 bg-transparent focus:border-secondary focus:ring-secondary focus:outline-none"
            />
          </Field>

          {/* Yeni Şifre */}
          <Field label="Yeni Şifre">
            <input
              type="password"
              value={newPassword}
              onChange={(e) => setNewPassword(e.target.value)}
              required
              minLength={6}
              className="w-full rounded border border-muted px-3 py-2 bg-transparent focus:border-secondary focus:ring-secondary focus:outline-none"
            />
          </Field>

          {/* Yeni Şifre (Onay) */}
          <Field label="Yeni Şifre (Tekrar)">
            <input
              type="password"
              value={confirmNewPassword}
              onChange={(e) => setConfirmNewPassword(e.target.value)}
              required
              minLength={6}
              className="w-full rounded border border-muted px-3 py-2 bg-transparent focus:border-secondary focus:ring-secondary focus:outline-none"
            />
          </Field>

          {/* Hata / Bilgi Mesajları */}
          {error && <p className="text-danger text-sm">{error}</p>}
          {info &&  <p className="text-primary text-sm">{info}</p>}

          {/* Gönder Butonu */}
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

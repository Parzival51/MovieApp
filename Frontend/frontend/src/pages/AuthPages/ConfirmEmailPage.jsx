// src/pages/AuthPages/ConfirmEmailPage.jsx
import React, { useEffect, useState } from 'react';
import { useSearchParams, Link } from 'react-router-dom';
import Layout from '../../components/Layout';
import Spinner from '../../components/Spinner';

export default function ConfirmEmailPage() {
  const [searchParams] = useSearchParams();
  const userId = searchParams.get('userId');
  const token  = searchParams.get('token');

  const [status, setStatus] = useState('loading');
  const [message, setMessage] = useState('');

  useEffect(() => {
    if (!userId || !token) {
      setStatus('error');
      setMessage('Geçersiz onay linki.');
      return;
    }

    // “token” zaten backend tarafından bir kez UrlEncode edilmiş olarak geliyor.
    // Burada tekrar encodeURIComponent(token) kullanmayacağız.
    fetch(
      `https://localhost:7176/api/auth/confirm-email?userId=${userId}&token=${token}`,
      { method: 'GET' }
    )
      .then(async (res) => {
        if (res.ok) {
          const text = await res.text();
          setStatus('success');
          setMessage(text || 'E-posta adresiniz başarıyla doğrulandı. Giriş yapabilirsiniz.');
        } else {
          const err = await res.text();
          setStatus('error');
          setMessage(err || 'E-posta onayı sırasında bir hata oluştu.');
        }
      })
      .catch(() => {
        setStatus('error');
        setMessage('Sunucuya bağlanırken bir hata oluştu.');
      });
  }, [userId, token]);

  return (
    <Layout>
      <div className="flex items-center justify-center py-20">
        <div className="w-full max-w-md space-y-6 bg-surface shadow rounded p-8 text-center">
          {status === 'loading' && (
            <>
              <Spinner />
              <p className="text-sm text-muted">E-posta onayınız kontrol ediliyor…</p>
            </>
          )}
          {status === 'success' && (
            <>
              <h1 className="text-2xl font-bold text-green-600">Onay Başarılı!</h1>
              <p className="text-sm text-muted">{message}</p>
              <Link to="/login" className="text-secondary hover:underline">
                Giriş sayfasına git
              </Link>
            </>
          )}
          {status === 'error' && (
            <>
              <h1 className="text-2xl font-bold text-danger">Onay Başarısız</h1>
              <p className="text-sm text-muted">{message}</p>
              <Link to="/register" className="text-secondary hover:underline">
                Tekrar kayıt olmayı deneyin
              </Link>
            </>
          )}
        </div>
      </div>
    </Layout>
  );
}

import React, { useState } from 'react';
import { useNavigate, Link } from 'react-router-dom';
import Layout from '../../components/Layout';
import Field  from '../../components/Field';
import Button from '../../components/Button';
import useAuth from '../../hooks/useAuth';
import client from '../../api/client';

export default function LoginPage() {
  const { login } = useAuth();
  const navigate  = useNavigate();

  const [email,    setEmail]    = useState('');
  const [password, setPassword] = useState('');
  const [error,    setError]    = useState(null);
  const [info,     setInfo]     = useState(null);
  const [loading,  setLoading]  = useState(false);
  const [resending, setResending] = useState(false);

  const handleSubmit = async e => {
    e.preventDefault();
    setLoading(true);
    setError(null);
    setInfo(null);

    try {
      await login({ email, password });
      // Başarılı login → ana sayfaya yönlendir:
      navigate('/', { replace: true });
    } catch (err) {
      console.error('Login error:', err);
      // Axios hatasında err.response.data döner, yoksa err.message
      const serverMessage = err.response?.data || err.message;
      setError(serverMessage || 'Login failed');
    } finally {
      setLoading(false);
    }
  };

  const handleResend = async () => {
    setResending(true);
    setInfo(null);
    try {
      await client.post('/auth/resend-confirmation', { email });
      setInfo('Onay maili tekrar gönderildi. Lütfen e-posta adresinizi kontrol edin.');
      setError(null);
    } catch (err) {
      console.error('Resend confirmation error:', err);
      setError('Onay maili gönderilirken bir hata oluştu.');
    } finally {
      setResending(false);
    }
  };

  // Eğer hata mesajı “Lütfen önce e-posta adresinizi doğrulayın” içeriyorsa,
  // “Resend confirmation” butonunu göster:
  const showResendButton = error?.includes('Lütfen önce e-posta adresinizi doğrulayın');

  return (
    <Layout>
      <div className="flex items-center justify-center py-20">
        <form
          onSubmit={handleSubmit}
          className="w-full max-w-md space-y-6 bg-surface shadow rounded p-8"
        >
          <div className="text-center">
            <h1 className="text-3xl font-heading mb-2">Giriş Yap</h1>
            <p className="text-sm text-muted">
              MovieApp’e erişmek için bilgilerinizi girin
            </p>
          </div>

          {/* E-posta alanı */}
          <Field label="E-posta">
            <input
              type="email"
              value={email}
              onChange={e => setEmail(e.target.value)}
              required
              className="w-full rounded border border-muted px-3 py-2 bg-transparent focus:border-secondary focus:ring-secondary focus:outline-none"
            />
          </Field>

          {/* Şifre alanı */}
          <Field label="Şifre">
            <input
              type="password"
              value={password}
              onChange={e => setPassword(e.target.value)}
              required
              className="w-full rounded border border-muted px-3 py-2 bg-transparent focus:border-secondary focus:ring-secondary focus:outline-none"
            />
          </Field>

          {/* Hata veya bilgi mesajı */}
          {error && (
            <p className="text-danger text-sm">{error}</p>
          )}
          {info && (
            <p className="text-primary text-sm">{info}</p>
          )}

          {/* “Şifremi Unuttum” linki */}
          <div className="flex justify-end">
            <Link
              to="/forgot-password"
              className="text-sm text-secondary hover:underline"
            >
              Şifremi Unuttum?
            </Link>
          </div>

          {/* “Onay maili tekrar gönder” butonu, hata mesajında ilgili uyarı varsa göster */}
          {showResendButton && (
            <Button
              type="button"
              variant="ghost"
              size="sm"
              className="mb-2"
              onClick={handleResend}
              disabled={resending}
            >
              {resending ? 'Gönderiliyor…' : 'Onay Maili Tekrar Gönder'}
            </Button>
          )}

          {/* “Giriş Yap” butonu */}
          <Button
            type="submit"
            variant="primary"
            size="md"
            className="w-full"
            disabled={loading}
          >
            {loading ? 'Giriş Yapılıyor…' : 'Giriş Yap'}
          </Button>
        </form>
      </div>
    </Layout>
  );
}

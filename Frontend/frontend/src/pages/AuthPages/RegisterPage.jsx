import React, { useState } from "react";
import Layout   from "../../components/Layout";
import Field    from "../../components/Field";
import Button   from "../../components/Button";
import useAuth  from "../../hooks/useAuth";

export default function RegisterPage() {
  const { register } = useAuth();

  const [displayName, setDisplayName] = useState("");
  const [email,       setEmail]       = useState("");
  const [userName,    setUserName]    = useState("");
  const [password,    setPassword]    = useState("");
  const [confirmPass, setConfirmPass] = useState("");

  const [error, setError] = useState(null);
  const [info,  setInfo]  = useState(null);
  const [loading, setLoading] = useState(false);

  const handleSubmit = async e => {
    e.preventDefault();

    if (password !== confirmPass) {
      setError("Şifreler uyuşmuyor");
      return;
    }

    setLoading(true);
    setError(null);
    setInfo(null);

    try {
      const { message } = await register({
        userName,
        displayName,
        email,
        password
      });

      setInfo(message);
    } catch (err) {
      const msg =
        err.response?.data?.message ||  
        err.response?.data?.Message ||   
        err.response?.data ||            
        err.message ||                  
        "Kayıt başarısız";

      setError(msg);
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
            <h1 className="text-3xl font-heading mb-2">Kayıt Ol</h1>
            <p className="text-sm text-muted">
              MovieApp’e katılmak için bilgilerinizi doldurun
            </p>
          </div>

          <Field label="İsim">
            <input
              type="text"
              value={displayName}
              onChange={e => setDisplayName(e.target.value)}
              required
              className="w-full rounded border border-muted px-3 py-2 bg-transparent focus:border-secondary focus:ring-secondary focus:outline-none"
            />
          </Field>

          <Field label="E-posta">
            <input
              type="email"
              value={email}
              onChange={e => setEmail(e.target.value)}
              required
              className="w-full rounded border border-muted px-3 py-2 bg-transparent focus:border-secondary focus:ring-secondary focus:outline-none"
            />
          </Field>

          <Field label="Kullanıcı Adı">
            <input
              type="text"
              value={userName}
              onChange={e => setUserName(e.target.value)}
              required
              className="w-full rounded border border-muted px-3 py-2 bg-transparent focus:border-secondary focus:ring-secondary focus:outline-none"
            />
          </Field>

          <Field label="Şifre">
            <input
              type="password"
              value={password}
              onChange={e => setPassword(e.target.value)}
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
            {loading ? "Kayıt Yapılıyor…" : "Kayıt Ol"}
          </Button>
        </form>
      </div>
    </Layout>
  );
}

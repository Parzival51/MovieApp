import React, { useState } from 'react';
import { Link, useNavigate } from 'react-router-dom';
import { HiMenu, HiX } from 'react-icons/hi';
import SearchBar from './SearchBar';
import useAuth from '../hooks/useAuth';

export default function Header() {
  const { tokens, user, logout } = useAuth();
  const navigate  = useNavigate();
  const [open, setOpen] = useState(false);      // mobil menü

  /* ------ yetki / durum ------ */
  const roles       = Array.isArray(user?.roles) ? user.roles : [];
  const isAdmin     = roles.includes('Admin');
  const isLoggedIn  = Boolean(tokens);

  const handleLogout = () => {
    logout();
    navigate('/login', { replace: true });
  };

  /* ------ yardımcı class ------ */
  const linkCls = 'text-muted hover:text-accent transition-colors';

  return (
    <header className="sticky top-0 z-40 bg-surface/80 backdrop-blur shadow-sm">
      <div className="container mx-auto flex items-center justify-between px-4 py-3">

        {/* ---------- Logo ---------- */}
        <Link
          to="/"
          className="font-heading text-2xl font-bold text-accent"
          onClick={() => setOpen(false)}
        >
          MovieApp
        </Link>

        {/* ---------- Masaüstü Menü ---------- */}
        <nav className="hidden md:flex items-center space-x-6">
          <Link to="/movies" className={linkCls}>Filmler</Link>
          <Link to="/actors" className={linkCls}>Oyuncular</Link>
          <Link to="/directors" className={linkCls}>Yönetmenler</Link>
          {isAdmin && <Link to="/admin" className={linkCls}>Admin Paneli</Link>}
        </nav>

        {/* ---------- Arama ---------- */}
        <SearchBar className="hidden lg:block flex-1 mx-6 max-w-lg" />

        {/* ---------- Auth Links ---------- */}
        <div className="hidden md:flex items-center space-x-4">
          {!isLoggedIn ? (
            <>
              <Link to="/login" className={linkCls}>Giriş Yap</Link>
              <Link to="/register" className={linkCls}>Kayıt Ol</Link>
            </>
          ) : (
            <>
              <Link to="/profile" className={linkCls}>Profil</Link>
              <button onClick={handleLogout} className={linkCls}>Çıkış Yap</button>
            </>
          )}
        </div>

        {/* ---------- Mobil Hamburger ---------- */}
        <button
          onClick={() => setOpen(o => !o)}
          className="md:hidden text-accent focus:outline-none"
          aria-label="Menüyü Aç/Kapat"
        >
          {open ? <HiX size={28} /> : <HiMenu size={28} />}
        </button>
      </div>

      {/* ---------- Mobil Çekme Menüsü ---------- */}
      {open && (
        <nav className="md:hidden bg-surface border-t border-muted/20">
          <ul className="flex flex-col space-y-2 px-4 py-4">
            <li>
              <Link to="/movies" onClick={() => setOpen(false)} className={linkCls}>
                Filmler
              </Link>
            </li>
            <li>
              <Link to="/actors" onClick={() => setOpen(false)} className={linkCls}>
                Oyuncular
              </Link>
            </li>
            <li>
              <Link to="/directors" onClick={() => setOpen(false)} className={linkCls}>
                Yönetmenler
              </Link>
            </li>
            {isAdmin && (
              <li>
                <Link to="/admin" onClick={() => setOpen(false)} className={linkCls}>
                  Admin Paneli
                </Link>
              </li>
            )}
            {!isLoggedIn ? (
              <>
                <li>
                  <Link to="/login" onClick={() => setOpen(false)} className={linkCls}>
                    Giriş Yap
                  </Link>
                </li>
                <li>
                  <Link to="/register" onClick={() => setOpen(false)} className={linkCls}>
                    Kayıt Ol
                  </Link>
                </li>
              </>
            ) : (
              <>
                <li>
                  <Link to="/profile" onClick={() => setOpen(false)} className={linkCls}>
                    Profil
                  </Link>
                </li>
                <li>
                  <button
                    onClick={handleLogout}
                    className={`${linkCls} w-full text-left`}
                  >
                    Çıkış Yap
                  </button>
                </li>
              </>
            )}
          </ul>
        </nav>
      )}
    </header>
  );
}

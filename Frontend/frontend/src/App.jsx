// src/App.jsx
import React, { lazy, Suspense } from 'react';
import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import { AuthProvider } from './context/AuthContext';
import { NotificationProvider } from './context/NotificationContext';
import { ThemeProvider } from './context/ThemeContext';
import PrivateRoute from './components/PrivateRoute';
import Spinner from './components/Spinner';

/* Lazy-loaded pages */
const HomePage               = lazy(() => import('./pages/HomePage.jsx'));
const LoginPage              = lazy(() => import('./pages/AuthPages/LoginPage.jsx'));
const RegisterPage           = lazy(() => import('./pages/AuthPages/RegisterPage.jsx'));
const ProfilePage            = lazy(() => import('./pages/ProfilePage.jsx'));
const MovieListPage          = lazy(() => import('./pages/ListPages/MovieListPage.jsx'));
const MovieDetailPage        = lazy(() => import('./pages/DetailPages/MovieDetailPage.jsx'));
const ActorListPage          = lazy(() => import('./pages/ListPages/ActorListPage.jsx'));
const ActorDetailPage        = lazy(() => import('./pages/DetailPages/ActorDetailPage.jsx'));
const DirectorListPage       = lazy(() => import('./pages/ListPages/DirectorListPage.jsx'));
const DirectorDetailPage     = lazy(() => import('./pages/DetailPages/DirectorDetailPage.jsx'));
/* Moderator */
const ModeratorReviewPage    = lazy(() => import('./pages/Moderator/ModeratorReviewPage.jsx'));
const ModeratorCommentsPage  = lazy(() => import('./pages/Moderator/ModeratorCommentsPage.jsx'));
/* Admin */
const AdminPanel             = lazy(() => import('./pages/Admin/AdminPanel.jsx'));
const AdminMoviesPage        = lazy(() => import('./pages/Admin/AdminMoviesPage.jsx'));
const AdminNewMoviePage      = lazy(() => import('./pages/Admin/AdminNewMoviePage.jsx'));
const AdminEditMoviePage     = lazy(() => import('./pages/Admin/AdminEditMoviePage.jsx'));
const AdminGenresPage        = lazy(() => import('./pages/Admin/AdminGenresPage.jsx'));
const AdminNewGenrePage      = lazy(() => import('./pages/Admin/AdminNewGenrePage.jsx'));
const AdminEditGenrePage     = lazy(() => import('./pages/Admin/AdminEditGenrePage.jsx'));
const AdminActorsPage        = lazy(() => import('./pages/Admin/AdminActorsPage.jsx'));
const AdminNewActorPage      = lazy(() => import('./pages/Admin/AdminNewActorPage.jsx'));
const AdminEditActorPage     = lazy(() => import('./pages/Admin/AdminEditActorPage.jsx'));
const AdminUsersPage         = lazy(() => import('./pages/Admin/AdminUsersPage.jsx'));
const AdminActivityLogsPage  = lazy(() => import('./pages/Admin/AdminActivityLogsPage.jsx'));

/* Auth-related pages */
const ConfirmEmailPage       = lazy(() => import('./pages/AuthPages/ConfirmEmailPage.jsx'));
const ForgotPasswordPage     = lazy(() => import('./pages/AuthPages/ForgotPasswordPage.jsx'));
const ResetPasswordPage      = lazy(() => import('./pages/AuthPages/ResetPasswordPage.jsx'));
const ResendConfirmationPage = lazy(() => import('./pages/AuthPages/ResendConfirmationPage.jsx'));
const ChangePasswordPage     = lazy(() => import('./pages/AuthPages/ChangePasswordPage.jsx'));

export default function App() {
  return (
    <ThemeProvider>
      <AuthProvider>
        <NotificationProvider>
          <BrowserRouter>
            <Suspense fallback={<Spinner />}>
              <Routes>
                {/* Public */}
                <Route path="/login" element={<LoginPage />} />
                <Route path="/register" element={<RegisterPage />} />
                <Route path="/confirm-email" element={<ConfirmEmailPage />} />
                <Route path="/forgot-password" element={<ForgotPasswordPage />} />
                <Route path="/reset-password" element={<ResetPasswordPage />} />
                <Route path="/resend-confirmation" element={<ResendConfirmationPage />} />

                {/* Authenticated */}
                <Route element={<PrivateRoute />}>
                  <Route index element={<HomePage />} />
                  <Route path="profile" element={<ProfilePage />} />

                  {/* Movies */}
                  <Route path="movies">
                    <Route index element={<MovieListPage />} />
                    <Route path=":id" element={<MovieDetailPage />} />
                  </Route>

                  {/* Actors */}
                  <Route path="actors">
                    <Route index element={<ActorListPage />} />
                    <Route path=":id" element={<ActorDetailPage />} />
                  </Route>

                  {/* Directors */}
                  <Route path="directors">
                    <Route index element={<DirectorListPage />} />
                    <Route path=":id" element={<DirectorDetailPage />} />
                  </Route>

                  {/* Redirected personal lists */}
                  <Route path="favorites" element={<Navigate to="/profile?tab=favorites" replace />} />
                  <Route path="watchlist" element={<Navigate to="/profile?tab=watch" replace />} />

                  {/* Moderator */}
                  <Route element={<PrivateRoute allowedRoles={['Moderator','Admin']} />}>
                    <Route path="moderator/reviews" element={<ModeratorReviewPage />} />
                    <Route path="moderator/comments" element={<ModeratorCommentsPage />} />
                  </Route>

                  {/* Admin */}
                  <Route element={<PrivateRoute allowedRoles={['Admin']} />}>
                    <Route path="admin" element={<AdminPanel />} />
                    {/* Movies */}
                    <Route path="admin/movies" element={<AdminMoviesPage />} />
                    <Route path="admin/movies/new" element={<AdminNewMoviePage />} />
                    <Route path="admin/movies/edit/:id" element={<AdminEditMoviePage />} />
                    {/* Genres */}
                    <Route path="admin/genres" element={<AdminGenresPage />} />
                    <Route path="admin/genres/new" element={<AdminNewGenrePage />} />
                    <Route path="admin/genres/edit/:id" element={<AdminEditGenrePage />} />
                    {/* Actors */}
                    <Route path="admin/actors" element={<AdminActorsPage />} />
                    <Route path="admin/actors/new" element={<AdminNewActorPage />} />
                    <Route path="admin/actors/edit/:id" element={<AdminEditActorPage />} />
                    {/* Users & Logs */}
                    <Route path="admin/users" element={<AdminUsersPage />} />
                    <Route path="admin/logs" element={<AdminActivityLogsPage />} />
                  </Route>

                  {/* Change Password (authenticated) */}
                  <Route path="/change-password" element={<ChangePasswordPage />} />
                </Route>

                {/* Fallback */}
                <Route path="*" element={<Navigate to="/" replace />} />
              </Routes>
            </Suspense>
          </BrowserRouter>
        </NotificationProvider>
      </AuthProvider>
    </ThemeProvider>
  );
}

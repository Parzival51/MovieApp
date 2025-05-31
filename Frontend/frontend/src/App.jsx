import React, { lazy, Suspense } from 'react';
import { BrowserRouter, Routes, Route, Navigate } from 'react-router-dom';
import { AuthProvider } from './context/AuthContext';
import { NotificationProvider } from './context/NotificationContext';
import { ThemeProvider } from './context/ThemeContext';
import PrivateRoute from './components/PrivateRoute';
import Spinner from './components/Spinner';

/* Lazy-loaded pages */
const HomePage              = lazy(() => import('./pages/HomePage'));
const LoginPage             = lazy(() => import('./pages/LoginPage'));
const RegisterPage          = lazy(() => import('./pages/AuthPages/RegisterPage'));
const ProfilePage           = lazy(() => import('./pages/ProfilePage'));
const MovieListPage         = lazy(() => import('./pages/ListPages/MovieListPage'));
const MovieDetailPage       = lazy(() => import('./pages/DetailPages/MovieDetailPage'));
const ActorListPage         = lazy(() => import('./pages/ListPages/ActorListPage'));
const ActorDetailPage       = lazy(() => import('./pages/DetailPages/ActorDetailPage'));
const DirectorListPage      = lazy(() => import('./pages/ListPages/DirectorListPage'));
const DirectorDetailPage    = lazy(() => import('./pages/DetailPages/DirectorDetailPage'));
/* Moderator */
const ModeratorReviewPage   = lazy(() => import('./pages/Moderator/ModeratorReviewPage'));
const ModeratorCommentsPage = lazy(() => import('./pages/Moderator/ModeratorCommentsPage'));
/* Admin */
const AdminPanel            = lazy(() => import('./pages/Admin/AdminPanel'));
const AdminMoviesPage       = lazy(() => import('./pages/Admin/AdminMoviesPage'));
const AdminNewMoviePage     = lazy(() => import('./pages/Admin/AdminNewMoviePage'));
const AdminEditMoviePage    = lazy(() => import('./pages/Admin/AdminEditMoviePage'));
const AdminGenresPage       = lazy(() => import('./pages/Admin/AdminGenresPage'));
const AdminNewGenrePage     = lazy(() => import('./pages/Admin/AdminNewGenrePage'));
const AdminEditGenrePage    = lazy(() => import('./pages/Admin/AdminEditGenrePage'));
const AdminActorsPage       = lazy(() => import('./pages/Admin/AdminActorsPage'));
const AdminNewActorPage     = lazy(() => import('./pages/Admin/AdminNewActorPage'));
const AdminEditActorPage    = lazy(() => import('./pages/Admin/AdminEditActorPage'));
const AdminUsersPage        = lazy(() => import('./pages/AdminUsersPage'));
const AdminActivityLogsPage = lazy(() => import('./pages/Admin/AdminActivityLogsPage'));

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
                  <Route element={<PrivateRoute allowedRoles={[ 'Moderator', 'Admin' ]} />}>
                    <Route path="moderator/reviews" element={<ModeratorReviewPage />} />
                    <Route path="moderator/comments" element={<ModeratorCommentsPage />} />
                  </Route>

                  {/* Admin */}
                  <Route element={<PrivateRoute allowedRoles={[ 'Admin' ]} />}>
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


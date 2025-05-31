import React from 'react';
import { Link } from 'react-router-dom';
import useAuth   from '../../hooks/useAuth';

export default function LatestCommentsLink() {
  const { user, tokens } = useAuth();
  const roles   = Array.isArray(user?.roles) ? user.roles : [];
  const canModerate = tokens && roles.includes('Moderator');

  if (!canModerate) return null;
  return (
    <Link
      to="/moderator/comments"
      className="text-gray-600 hover:text-gray-900"
    >
      Yorumları Onayla
    </Link>
  );
}

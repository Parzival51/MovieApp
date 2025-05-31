import client from "./client";

export function getActivityLogs({ from, to, action, page = 1, pageSize = 100 }) {
  const params = new URLSearchParams({ page, pageSize });
  if (from)   params.append("from",   from);
  if (to)     params.append("to",     to);
  if (action) params.append("action", action);

  return client
    .get(`/activitylogs?${params.toString()}`)
    .then(r => r.data);                 // API list ([]) döndürür
}

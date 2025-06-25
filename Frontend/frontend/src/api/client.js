// ─── src/api/client.js ──────────────────────────────────────────────────────
import axios from 'axios';


const client = axios.create({
  baseURL: 'https://localhost:7176/api',
  withCredentials: true,                 
  headers: { 'Content-Type': 'application/json' }
});


client.interceptors.request.use(cfg => {
  const token = localStorage.getItem('accessToken');
  if (token) cfg.headers.Authorization = `Bearer ${token}`;
  return cfg;
});


client.interceptors.response.use(
  res => res,
  async err => {
    const orig = err.config;

    if (err.response?.status === 401 && !orig._retry) {
      orig._retry = true;
      try {
        /*  !!!  .data okumayı unutma  !!!  */
        const { accessToken } = (await client.post('/auth/refresh-token')).data;

        if (accessToken) {
          localStorage.setItem('accessToken', accessToken);
          orig.headers.Authorization = `Bearer ${accessToken}`;
          return client(orig);                 
        }
      } catch {
      }

      localStorage.removeItem('accessToken');
      window.location.href = '/login';
    }

    return Promise.reject(err);
  }
);

export default client;

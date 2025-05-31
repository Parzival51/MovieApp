// src/api/client.js
import axios from 'axios';

const client = axios.create({
  baseURL: 'https://localhost:7176/api',
  withCredentials: true,          // ← eklendi
  headers: { 'Content-Type':'application/json' }
});

// Her isteğe Authorization header'ı ekle
client.interceptors.request.use(config => {
  const token = localStorage.getItem('accessToken');
  if (token) {
    config.headers.Authorization = `Bearer ${token}`;
  }
  return config;
});

export default client;

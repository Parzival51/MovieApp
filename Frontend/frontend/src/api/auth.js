import client from "./client";

export function login({ email, password }) {
  return client.post("/auth/login", { email, password }).then(r => r.data);
}

export function register({ userName, displayName, email, password }) {
  // backend → { message : "…" }
  return client
    .post("/auth/register", { userName, displayName, email, password })
    .then(r => r.data);
}

export const refreshToken = () =>
  client.post("/auth/refresh-token").then(r => r.data);

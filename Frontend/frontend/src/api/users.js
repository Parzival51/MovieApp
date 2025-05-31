import client from './client';


export const getAllUsers = () =>
  client.get('/users').then(r => r.data);


export const deleteUser = id =>
  client.delete(`/users/${id}`);


export const updateUserRoles = (id, roles) =>

  client.put(`/users/${id}/roles`, { roles }).then(r => r.data);

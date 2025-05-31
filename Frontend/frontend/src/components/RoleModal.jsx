import React, { useState, useEffect } from 'react';
import PropTypes from 'prop-types';
import Modal from './Modal';
import Button from './Button';

const ALL_ROLES = ['Admin', 'Moderator', 'User'];

export default function RoleModal({ open, onClose, user, onSave }) {
  const [selected, setSelected] = useState(user.roles || []);

  /* Modal her açıldığında state yenilensin */
  useEffect(() => {
    if (open) setSelected(user.roles || []);
  }, [open, user.roles]);

  const toggle = role =>
    setSelected(s =>
      s.includes(role) ? s.filter(r => r !== role) : [...s, role]
    );

  return (
    <Modal open={open} onClose={onClose} ariaLabel="Rol Düzenle">
      <h3 id="role-modal-title" className="text-xl font-heading mb-4">
        {user.displayName || user.userName} – Roller
      </h3>

      <ul className="space-y-2 mb-6">
        {ALL_ROLES.map(r => (
          <li key={r}>
            <label className="inline-flex items-center space-x-2">
              <input
                type="checkbox"
                checked={selected.includes(r)}
                onChange={() => toggle(r)}
              />
              <span>{r}</span>
            </label>
          </li>
        ))}
      </ul>

      <div className="flex justify-end space-x-3">
        <Button variant="secondary" size="sm" onClick={onClose}>
          İptal
        </Button>
        <Button
          variant="primary"
          size="sm"
          onClick={() => onSave(selected)}
        >
          Kaydet
        </Button>
      </div>
    </Modal>
  );
}

RoleModal.propTypes = {
  open:    PropTypes.bool.isRequired,
  onClose: PropTypes.func.isRequired,
  onSave:  PropTypes.func.isRequired,
  user:    PropTypes.object.isRequired
};

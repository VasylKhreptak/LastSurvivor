using System;
using Gameplay.Aim;
using Gameplay.Weapons.Core;
using UnityEngine;
using Zenject;

namespace Gameplay.Weapons
{
    public class WeaponAimer : ITickable
    {
        private readonly Trackpad _trackpad;
        private readonly Camera _camera;
        private readonly IWeapon _weapon;
        private readonly Preferences _preferences;

        public WeaponAimer(Trackpad trackpad, Camera camera, IWeapon weapon, Preferences preferences)
        {
            _trackpad = trackpad;
            _camera = camera;
            _weapon = weapon;
            _preferences = preferences;
        }

        public bool Enabled = true;

        private Vector3 _lookPoint;
        private Transform _weaponTransform;
        private Vector3 _weaponPosition;

        public void Tick()
        {
            if (Enabled == false)
                return;

            Aim();
        }

        private void Aim()
        {
            _lookPoint = GetLookPoint();

            _weaponTransform = _weapon.Transform;
            _weaponPosition = _weaponTransform.position;

            Debug.DrawLine(_weaponPosition, _lookPoint, Color.red);

            _weaponTransform.rotation = Quaternion.LookRotation(_lookPoint - _weaponPosition);
        }

        private Vector3 GetLookPoint()
        {
            Ray ray = _camera.ScreenPointToRay(_trackpad.GetScreenPoint());

            if (Physics.Raycast(ray, out RaycastHit hitInfo, _preferences.WorkingDistance, _preferences.AimLayerMask))
            {
                Debug.DrawRay(ray.origin, ray.direction * hitInfo.distance, Color.green);
                return hitInfo.point;
            }

            Debug.DrawRay(ray.origin, ray.direction * _preferences.WorkingDistance, Color.green);
            return _camera.transform.position + ray.direction * _preferences.WorkingDistance;
        }

        [Serializable]
        public class Preferences
        {
            [SerializeField] private LayerMask _aimLayerMask;
            [SerializeField] private float _workingDistance;

            public LayerMask AimLayerMask => _aimLayerMask;
            public float WorkingDistance => _workingDistance;
        }
    }
}
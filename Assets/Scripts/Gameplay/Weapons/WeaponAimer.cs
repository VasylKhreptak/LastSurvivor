using System;
using Gameplay.Aim;
using UnityEngine;
using Zenject;

namespace Gameplay.Weapons
{
    public class WeaponAimer : ITickable
    {
        private readonly Trackpad _trackpad;
        private readonly Camera _camera;
        private readonly WeaponHolder _weaponHolder;
        private readonly Preferences _preferences;

        public WeaponAimer(Trackpad trackpad, Camera camera, WeaponHolder weaponHolder, Preferences preferences)
        {
            _trackpad = trackpad;
            _camera = camera;
            _weaponHolder = weaponHolder;
            _preferences = preferences;
        }

        public bool Enabled = true;

        private Vector3 _lookPoint;
        private Transform _transform;
        private Vector3 _weaponPosition;

        public void Tick()
        {
            if (Enabled == false || _weaponHolder.Instance == null)
                return;

            Aim();
        }

        private void Aim()
        {
            _lookPoint = GetLookPoint();

            _transform = _weaponHolder.Instance.Transform;
            _weaponPosition = _transform.position;

            Debug.DrawLine(_weaponPosition, _lookPoint, Color.red);

            _transform.rotation = Quaternion.LookRotation(_lookPoint - _weaponPosition);
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
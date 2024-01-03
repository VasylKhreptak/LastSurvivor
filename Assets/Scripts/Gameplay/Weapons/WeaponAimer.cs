using System;
using Gameplay.Aim;
using UnityEngine;
using Zenject;

namespace Gameplay.Weapons
{
    public class WeaponAimer : ITickable
    {
        private readonly Transform _transform;
        private readonly Trackpad _trackpad;
        private readonly Camera _camera;
        private readonly Preferences _preferences;

        public WeaponAimer(Transform transform, Trackpad trackpad, Camera camera, Preferences preferences)
        {
            _transform = transform;
            _trackpad = trackpad;
            _camera = camera;
            _preferences = preferences;
        }

        private Vector3 _lookPoint;

        public void Tick() => Aim();

        private void Aim()
        {
            _lookPoint = GetLookPoint();

            Debug.DrawLine(_transform.position, _lookPoint, Color.red);

            _transform.rotation = Quaternion.LookRotation(_lookPoint - _transform.position);
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
            public LayerMask AimLayerMask;
            public float WorkingDistance;
        }
    }
}
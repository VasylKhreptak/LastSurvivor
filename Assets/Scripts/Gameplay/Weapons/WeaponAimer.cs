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
        private readonly Preferences _preferences;

        public WeaponAimer(Trackpad trackpad, Camera camera, Preferences preferences)
        {
            _trackpad = trackpad;
            _camera = camera;
            _preferences = preferences;
        }

        private Vector3 _lookPoint;

        public void Tick() => Aim();

        private void Aim()
        {
            _lookPoint = GetLookPoint();

            Debug.DrawLine(_preferences.Transform.position, _lookPoint, Color.red);

            _preferences.Transform.rotation = Quaternion.LookRotation(_lookPoint - _preferences.Transform.position);
        }

        private Vector3 GetLookPoint()
        {
            Ray ray = _camera.ScreenPointToRay(_trackpad.GetScreenPoint());

            if (Physics.Raycast(ray, out RaycastHit hitInfo, _preferences.WorkingDistance, _preferences.AimLayerMask))
            {
                Debug.DrawRay(ray.origin, ray.direction * hitInfo.distance, Color.green);
                return hitInfo.point + _preferences.LookPointOffset;
            }

            Debug.DrawRay(ray.origin, ray.direction * _preferences.WorkingDistance, Color.green);
            return _camera.transform.position + ray.direction * _preferences.WorkingDistance + _preferences.LookPointOffset;
        }

        [Serializable]
        public class Preferences
        {
            [SerializeField] private Transform _transform;
            [SerializeField] private LayerMask _aimLayerMask;
            [SerializeField] private float _workingDistance;
            [SerializeField] private Vector3 _lookPointOffset;

            public Transform Transform => _transform;
            public LayerMask AimLayerMask => _aimLayerMask;
            public float WorkingDistance => _workingDistance;
            public Vector3 LookPointOffset => _lookPointOffset;
        }
    }
}
using UnityEngine;
using Zenject;

namespace Utilities.TransformUtilities.Looker
{
    public class TransformLooker : ITickable
    {
        private readonly TransformLookerPreferences _preferences;

        public TransformLooker(TransformLookerPreferences preferences)
        {
            _preferences = preferences;
        }

        private Vector3 _direction;
        private Quaternion _currentRotation;
        private Quaternion _targetRotation;
        private Quaternion _intermediateRotation;

        public void Tick() => LookStep();

        public void LookImmediately()
        {
            if (_preferences.Source == null || _preferences.Target == null)
                return;

            _direction = _preferences.Target.position - _preferences.Source.position;
            _targetRotation = Quaternion.LookRotation(_direction, _preferences.Upwards) * Quaternion.Euler(_preferences.Offset);
            _preferences.Source.rotation = _targetRotation;
        }

        private void LookStep()
        {
            if (_preferences.Source == null || _preferences.Target == null)
                return;

            _direction = _preferences.Target.position - _preferences.Source.position;

            _currentRotation = _preferences.Source.rotation;
            _targetRotation = Quaternion.LookRotation(_direction, _preferences.Upwards) * Quaternion.Euler(_preferences.Offset);
            _intermediateRotation = Quaternion.Lerp(_currentRotation, _targetRotation, _preferences.LookSpeed * Time.deltaTime);
            _preferences.Source.rotation = _intermediateRotation;
        }
    }
}
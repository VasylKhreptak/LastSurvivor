using UnityEngine;
using Zenject;

namespace TransformUtilities.Looker
{
    public class TransformLooker : ITickable
    {
        private readonly Transform _source;
        private readonly Transform _target;
        private readonly Vector3 _upwards;
        private readonly float _rotationSpeed;

        public TransformLooker(Transform source, Transform target, Vector3 upwards, float rotationSpeed)
        {
            _source = source;
            _target = target;
            _upwards = upwards;
            _rotationSpeed = rotationSpeed;
        }

        public void Tick() => LookStep();

        private void LookStep()
        {
            if (_source == null || _target == null)
                return;

            Vector3 direction = _target.position - _source.position;

            Quaternion targetRotation = Quaternion.LookRotation(direction, _upwards);

            _source.rotation = Quaternion.Lerp(_source.rotation, targetRotation, _rotationSpeed * Time.deltaTime);
        }
    }
}
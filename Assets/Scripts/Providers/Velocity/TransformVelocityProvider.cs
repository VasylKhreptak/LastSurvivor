using Providers.Velocity.Core;
using UnityEngine;
using Zenject;

namespace Providers.Velocity
{
    public class TransformVelocityProvider : IVelocityProvider, ITickable
    {
        private readonly Transform _transform;

        public TransformVelocityProvider(Transform transform)
        {
            _transform = transform;
            _previousPosition = _transform.position;
        }

        private Vector3 _position;
        private Vector3 _previousPosition;

        public Vector3 Value { get; private set; }

        public void Tick()
        {
            _position = _transform.position;
            Value = (_position - _previousPosition) / Time.deltaTime;
            _previousPosition = _position;
        }
    }
}
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
        private Vector3 _velocity;

        public Vector3 Value => _velocity;

        public void Tick()
        {
            _position = _transform.position;
            _velocity = (_position - _previousPosition) / Time.deltaTime;
            _previousPosition = _position;
        }
    }
}
using Adapters.Velocity.Core;
using UnityEngine;
using Zenject;

namespace Adapters.Velocity
{
    public class AdaptedTransformForVelocity : IVelocityAdapter, ITickable
    {
        private readonly Transform _transform;

        public AdaptedTransformForVelocity(Transform transform)
        {
            _transform = transform;
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
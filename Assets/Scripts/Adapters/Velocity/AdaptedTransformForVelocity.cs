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

        public Vector3 Value { get; private set; }

        public void Tick()
        {
            _position = _transform.position;
            Value = (_position - _previousPosition) / Time.deltaTime;
            _previousPosition = _position;
        }
    }
}
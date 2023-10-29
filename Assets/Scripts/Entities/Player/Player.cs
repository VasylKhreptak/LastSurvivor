using UnityEngine;
using Zenject;

namespace Entities.Player
{
    public class Player : MonoBehaviour
    {
        private Transform _transform;

        [Inject]
        private void Constructor(Transform transform)
        {
            _transform = transform;
        }

        public Transform Transform => _transform;
    }
}
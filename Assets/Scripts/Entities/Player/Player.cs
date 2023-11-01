using UnityEngine;
using Zenject;

namespace Entities.Player
{
    public class Player : MonoBehaviour
    {
        [Inject]
        private void Constructor(Transform transform)
        {
            Transform = transform;
        }

        public Transform Transform { get; private set; }
    }
}
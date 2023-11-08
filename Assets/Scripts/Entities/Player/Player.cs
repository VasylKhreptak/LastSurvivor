using Grid;
using UnityEngine;
using Zenject;

namespace Entities.Player
{
    public class Player : MonoBehaviour
    {
        [Inject]
        private void Constructor(Transform transform, GridStack gridStack)
        {
            Transform = transform;
            BarrelGridStack = gridStack;
        }

        public Transform Transform { get; private set; }
        public GridStack BarrelGridStack { get; private set; }
    }
}
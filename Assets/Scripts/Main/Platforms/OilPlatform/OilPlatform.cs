using Grid;
using UnityEngine;
using Zenject;

namespace Main.Platforms.OilPlatform
{
    public class OilPlatform : MonoBehaviour
    {
        private GridStack _gridStack;

        [Inject]
        private void Constructor(GridStack gridStack)
        {
            _gridStack = gridStack;
        }

        public Transform GridStackTransform => _gridStack.Root;
    }
}
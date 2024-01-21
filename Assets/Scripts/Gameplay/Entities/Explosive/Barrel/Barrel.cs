using Gameplay.Entities.Health.Core;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Gameplay.Entities.Explosive.Barrel
{
    public class Barrel : MonoBehaviour
    {
        private IHealth _health;

        [Inject]
        private void Constructor(IHealth health)
        {
            _health = health;
        }

        [Button]
        public void Explode()
        {
            _health.Kill();
        }
    }
}
using Gameplay.Entities.Health.Core;
using UnityEngine;
using Zenject;

namespace Gameplay.Entities.Health.DamageHandlers.Core
{
    public class BaseDamageHandler : MonoBehaviour
    {
        protected IHealth _health;

        [Inject]
        private void Constructor(IHealth health)
        {
            _health = health;
        }
    }
}
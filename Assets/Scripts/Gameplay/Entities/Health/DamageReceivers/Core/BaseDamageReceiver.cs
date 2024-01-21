using Gameplay.Entities.Health.Core;
using UnityEngine;
using Zenject;

namespace Gameplay.Entities.Health.DamageReceivers.Core
{
    public class BaseDamageReceiver : MonoBehaviour
    {
        private IHealth _health;

        [Inject]
        private void Constructor(IHealth health)
        {
            _health = health;
        }
        
        protected IHealth Health => _health;
    }
}
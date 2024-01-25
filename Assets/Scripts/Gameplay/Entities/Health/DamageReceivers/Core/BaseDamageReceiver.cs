using Gameplay.Entities.Health.Core;
using UnityEngine;
using Zenject;

namespace Gameplay.Entities.Health.DamageReceivers.Core
{
    public class BaseDamageReceiver : MonoBehaviour
    {
        [Inject]
        private void Constructor(IHealth health)
        {
            Health = health;
        }

        protected IHealth Health { get; private set; }
    }
}
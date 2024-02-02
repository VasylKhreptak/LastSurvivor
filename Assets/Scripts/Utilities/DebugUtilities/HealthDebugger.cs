using Gameplay.Entities.Health.Core;
using Sirenix.OdinInspector;
using UnityEngine;
using Zenject;

namespace Utilities.DebugUtilities
{
    public class HealthDebugger : MonoBehaviour
    {
        private IHealth _health;

        [Inject]
        private void Constructor(IHealth health)
        {
            _health = health;
        }

        [Button] private void Kill() => _health.Kill();
    }
}
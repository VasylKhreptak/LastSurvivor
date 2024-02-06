using Entities.AI;
using UnityEngine;
using Zenject;

namespace Gameplay.Entities.Player
{
    public class PlayerAnimatorCallbackReceiver : MonoBehaviour
    {
        private MeleeAttacker _meleeAttacker;

        [Inject]
        private void Constructor(MeleeAttacker meleeAttacker)
        {
            _meleeAttacker = meleeAttacker;
        }

        private void OnMeleeAttacked() => _meleeAttacker.ApplyDamage();
    }
}
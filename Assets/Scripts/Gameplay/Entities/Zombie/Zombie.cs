using UnityEngine;
using Zenject;

namespace Gameplay.Entities.Zombie
{
    public class Zombie : MonoBehaviour
    {
        public ZombieTargetFollower TargetFollower { get; private set; }
        public ZombieAttacker Attacker { get; private set; }
        
        [Inject]
        private void Constructor(ZombieTargetFollower targetFollower, ZombieAttacker attacker)
        {
            TargetFollower = targetFollower;
            Attacker = attacker;
        }
    }
}
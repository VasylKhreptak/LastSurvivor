using UnityEngine;
using Zenject;

namespace Gameplay.Entities.Zombie
{
    public class Zombie : MonoBehaviour
    {
        [Inject]
        private void Constructor(ZombieTargetFollower targetFollower)
        {
            TargetFollower = targetFollower;
        }

        public ZombieTargetFollower TargetFollower { get; private set; }
    }
}
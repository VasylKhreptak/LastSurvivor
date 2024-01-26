using UnityEngine;
using Zenject;

namespace Gameplay.Entities.Zombie
{
    public class Zombie : MonoBehaviour
    {
        private ZombieTargetFollower _targetFollower;

        [Inject]
        private void Constructor(ZombieTargetFollower targetFollower)
        {
            _targetFollower = targetFollower;
        }

        public ZombieTargetFollower TargetFollower => _targetFollower;
    }
}
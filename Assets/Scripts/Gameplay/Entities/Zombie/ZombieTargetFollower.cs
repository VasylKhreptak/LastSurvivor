using System;
using Gameplay.Entities.Player;
using UnityEngine;
using Zenject;

namespace Gameplay.Entities.Zombie
{
    public class ZombieTargetFollower : ITickable
    {
        private readonly PlayerHolder _playerHolder;

        public ZombieTargetFollower(PlayerHolder playerHolder) => _playerHolder = playerHolder;

        public void Tick()
        {
            if (_playerHolder.Instance == null)
                return;
            
            
        }

        [Serializable]
        public class Preferences
        {
            [SerializeField] private float _targetPositionThreshold = 0.5f;
        }
    }
}
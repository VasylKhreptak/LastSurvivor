using System.Collections.Generic;
using Gameplay.Entities.Player;
using Gameplay.Entities.Zombie;
using Levels.StateMachine.States.Core;
using UnityEngine;

namespace Levels.StateMachine.States
{
    public class LevelFinishedState : ILevelState
    {
        private readonly List<Zombie> _zombies;
        private readonly PlayerHolder _playerHolder;

        public LevelFinishedState(List<Zombie> zombies, PlayerHolder playerHolder)
        {
            _zombies = zombies;
            _playerHolder = playerHolder;
        }

        public void Enter()
        {
            _zombies.ForEach(zombie => zombie.TargetFollower.Stop());
            if (_playerHolder.Instance != null)
                _playerHolder.Instance.WaypointFollower.Stop();

            Debug.Log("Level Finished State!");
        }
    }
}
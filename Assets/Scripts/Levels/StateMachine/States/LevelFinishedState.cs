using System.Collections.Generic;
using Gameplay.Aim;
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
        private readonly Trackpad _trackpad;

        public LevelFinishedState(List<Zombie> zombies, PlayerHolder playerHolder, Trackpad trackpad)
        {
            _zombies = zombies;
            _playerHolder = playerHolder;
            _trackpad = trackpad;
        }

        public void Enter()
        {
            _zombies.ForEach(zombie => zombie.TargetFollower.Stop());
            if (_playerHolder.Instance != null)
                _playerHolder.Instance.WaypointFollower.Stop();
            _trackpad.enabled = false;

            Debug.Log("Level Finished State!");
        }
    }
}
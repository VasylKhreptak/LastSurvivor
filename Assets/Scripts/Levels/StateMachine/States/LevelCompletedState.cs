using System.Collections.Generic;
using Gameplay.Aim;
using Gameplay.Entities.Player;
using Gameplay.Entities.Zombie;
using Levels.StateMachine.States.Core;
using UI.Gameplay.Windows;

namespace Levels.StateMachine.States
{
    public class LevelCompletedState : ILevelState
    {
        private readonly List<Zombie> _zombies;
        private readonly PlayerHolder _playerHolder;
        private readonly Trackpad _trackpad;
        private LevelCompletedWindow _levelCompletedWindow;

        public LevelCompletedState(List<Zombie> zombies, PlayerHolder playerHolder, Trackpad trackpad,
            LevelCompletedWindow levelCompletedWindow)
        {
            _zombies = zombies;
            _playerHolder = playerHolder;
            _trackpad = trackpad;
            _levelCompletedWindow = levelCompletedWindow;
        }

        public void Enter()
        {
            _zombies.ForEach(zombie => zombie.TargetFollower.Stop());
            if (_playerHolder.Instance != null)
                _playerHolder.Instance.WaypointFollower.Stop();
            _trackpad.enabled = false;
            _levelCompletedWindow.Show();
        }
    }
}
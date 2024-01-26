using System.Collections.Generic;
using Gameplay.Aim;
using Gameplay.Entities.Player;
using Gameplay.Entities.Zombie;
using Gameplay.Weapons;
using Levels.StateMachine.States.Core;
using UI.Gameplay.Windows;

namespace Levels.StateMachine.States
{
    public class LevelFailedState : ILevelState
    {
        private readonly List<Zombie> _zombies;
        private readonly PlayerHolder _playerHolder;
        private readonly Trackpad _trackpad;
        private readonly WeaponAim _weaponAim;
        private readonly WeaponAimer _weaponAimer;

        public LevelFailedState(List<Zombie> zombies, PlayerHolder playerHolder, Trackpad trackpad, WeaponAim weaponAim,
            WeaponAimer weaponAimer)
        {
            _zombies = zombies;
            _playerHolder = playerHolder;
            _trackpad = trackpad;
            _weaponAim = weaponAim;
            _weaponAimer = weaponAimer;
        }

        public void Enter()
        {
            _zombies.ForEach(zombie => zombie.TargetFollower.Stop());

            if (_playerHolder.Instance != null)
                _playerHolder.Instance.WaypointFollower.Stop();

            _trackpad.enabled = false;
            _weaponAim.Hide();
            _weaponAimer.Enabled = false;
        }
    }
}
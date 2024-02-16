using System.Collections.Generic;
using Gameplay.Aim;
using Gameplay.Entities.Collector;
using Gameplay.Entities.Platoon;
using Gameplay.Entities.Player;
using Gameplay.Entities.Zombie;
using Gameplay.Entities.Zombie.StateMachine.States;
using Gameplay.Levels.StateMachine.States.Core;
using Gameplay.Weapons;
using Infrastructure.Services.PersistentData.Core;
using Infrastructure.StateMachine.Main.States.Core;
using UI.Gameplay.Windows;

namespace Gameplay.Levels.StateMachine.States
{
    public class LevelCompletedState : ILevelState, IState
    {
        private readonly List<Zombie> _zombies;
        private readonly PlayerHolder _playerHolder;
        private readonly Trackpad _trackpad;
        private readonly LevelCompletedWindow _levelCompletedWindow;
        private readonly WeaponAim _weaponAim;
        private readonly WeaponAimer _weaponAimer;
        private readonly HUD _hud;
        private readonly Platoon _platoon;
        private readonly IPersistentDataService _persistentDataService;
        private readonly List<Collector> _collectors;

        public LevelCompletedState(List<Zombie> zombies, PlayerHolder playerHolder, Trackpad trackpad,
            LevelCompletedWindow levelCompletedWindow, WeaponAim weaponAim, WeaponAimer weaponAimer, HUD hud, Platoon platoon,
            IPersistentDataService persistentDataService, List<Collector> collectors)
        {
            _zombies = zombies;
            _playerHolder = playerHolder;
            _trackpad = trackpad;
            _levelCompletedWindow = levelCompletedWindow;
            _weaponAim = weaponAim;
            _weaponAimer = weaponAimer;
            _hud = hud;
            _platoon = platoon;
            _persistentDataService = persistentDataService;
            _collectors = collectors;
        }

        public void Enter()
        {
            _zombies.ForEach(zombie => zombie.StateMachine.Enter<IdleState>());

            if (_playerHolder.Instance != null)
                _playerHolder.Instance.StateMachine.Enter<Gameplay.Entities.Player.StateMachine.States.IdleState>();

            _trackpad.enabled = false;
            _levelCompletedWindow.Show();
            _weaponAim.Hide();
            _weaponAimer.Enabled = false;
            _hud.Hide();

            _collectors.ForEach(collector =>
                collector.StateMachine.Enter<Gameplay.Entities.Collector.StateMachine.States.IdleState>());

            _platoon.Soldiers.ForEach(soldier =>
                soldier.StateMachine.Enter<Gameplay.Entities.Soldier.StateMachine.States.IdleState>());

            _persistentDataService.PersistentData.PlayerData.Level++;
        }
    }
}
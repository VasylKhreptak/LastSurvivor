using System.Collections.Generic;
using Gameplay.Aim;
using Gameplay.Entities.Collector;
using Gameplay.Entities.Helicopter;
using Gameplay.Entities.Platoon;
using Gameplay.Entities.Player;
using Gameplay.Entities.Zombie;
using Gameplay.Entities.Zombie.StateMachine.States;
using Gameplay.Weapons;
using Infrastructure.Services.PersistentData.Core;
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
        private readonly LevelFailedWindow _levelFailedWindow;
        private readonly HUD _hud;
        private readonly Helicopter _helicopter;
        private readonly Platoon _platoon;
        private readonly List<Collector> _collectors;
        private readonly IPersistentDataService _persistentDataService;

        public LevelFailedState(List<Zombie> zombies, PlayerHolder playerHolder, Trackpad trackpad, WeaponAim weaponAim,
            WeaponAimer weaponAimer, LevelFailedWindow levelFailedWindow, HUD hud, Helicopter helicopter, Platoon platoon,
            List<Collector> collectors, IPersistentDataService persistentDataService)
        {
            _zombies = zombies;
            _playerHolder = playerHolder;
            _trackpad = trackpad;
            _weaponAim = weaponAim;
            _weaponAimer = weaponAimer;
            _levelFailedWindow = levelFailedWindow;
            _hud = hud;
            _helicopter = helicopter;
            _platoon = platoon;
            _collectors = collectors;
            _persistentDataService = persistentDataService;
        }

        public void Enter()
        {
            _zombies.ForEach(zombie => zombie.StateMachine.Enter<IdleState>());

            if (_playerHolder.Instance != null)
                _playerHolder.Instance.StateMachine.Enter<Gameplay.Entities.Player.StateMachine.States.IdleState>();

            _trackpad.enabled = false;
            _weaponAim.Hide();
            _weaponAimer.Enabled = false;
            _levelFailedWindow.Show();
            _hud.Hide();
            _helicopter.TargetFollower.Target = null;

            _collectors.ForEach(collector =>
                collector.StateMachine.Enter<Gameplay.Entities.Collector.StateMachine.States.IdleState>());

            _platoon.Soldiers.ForEach(soldier =>
                soldier.StateMachine.Enter<Gameplay.Entities.Soldier.StateMachine.States.IdleState>());

            _persistentDataService.PersistentData.PlayerData.PlatformsData.CollectorsPlatformData.CollectorsBank.Clear();
            _persistentDataService.PersistentData.PlayerData.PlatformsData.BarracksPlatformData.SoldiersBank.Clear();
        }
    }
}
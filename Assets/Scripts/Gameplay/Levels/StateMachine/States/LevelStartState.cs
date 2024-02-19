using System.Collections.Generic;
using Gameplay.Entities.Collector;
using Gameplay.Entities.Platoon;
using Gameplay.Entities.Player;
using Gameplay.Entities.Zombie;
using Gameplay.Entities.Zombie.StateMachine.States;
using Gameplay.Levels.StateMachine.States.Core;
using Infrastructure.StateMachine.Main.Core;
using Infrastructure.StateMachine.Main.States.Core;
using UI.Gameplay.Windows;

namespace Gameplay.Levels.StateMachine.States
{
    public class LevelStartState : ILevelState, IState
    {
        private readonly IStateMachine<ILevelState> _levelStateMachine;
        private readonly PlayerHolder _playerHolder;
        private readonly List<Zombie> _zombies;
        private readonly StartWindow _startWindows;
        private readonly WeaponAim _weaponAim;
        private readonly HUD _hud;
        private readonly Platoon _platoon;
        private readonly List<Collector> _collectors;
        private readonly List<ZombieSpawner.ZombieSpawner> _zombieSpawners;

        public LevelStartState(IStateMachine<ILevelState> levelStateMachine, PlayerHolder playerHolder, List<Zombie> zombies,
            StartWindow startWindow, WeaponAim weaponAim, HUD hud, Platoon platoon, List<Collector> collectors,
            List<ZombieSpawner.ZombieSpawner> zombieSpawners)
        {
            _levelStateMachine = levelStateMachine;
            _playerHolder = playerHolder;
            _zombies = zombies;
            _startWindows = startWindow;
            _weaponAim = weaponAim;
            _hud = hud;
            _platoon = platoon;
            _collectors = collectors;
            _zombieSpawners = zombieSpawners;
        }

        public void Enter()
        {
            _zombieSpawners.ForEach(spawner => spawner.Enable());
            _zombies.ForEach(zombie => zombie.StateMachine.Enter<NavigationState>());

            _playerHolder.Instance.StateMachine.Enter<Entities.Player.StateMachine.States.NavigationState>();

            _levelStateMachine.Enter<LevelLoopState>();
            _startWindows.Hide();
            _weaponAim.Show();
            _hud.Show();

            _collectors.ForEach(collector =>
                collector.StateMachine.Enter<Entities.Collector.StateMachine.States.NavigationState>());

            _platoon.Soldiers.ForEach(soldier =>
                soldier.StateMachine.Enter<Entities.Soldier.StateMachine.States.NavigationState>());
        }
    }
}
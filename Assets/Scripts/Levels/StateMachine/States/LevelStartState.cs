using System.Collections.Generic;
using Gameplay.Entities.Platoon;
using Gameplay.Entities.Player;
using Gameplay.Entities.Zombie;
using Infrastructure.StateMachine.Main.Core;
using Levels.StateMachine.States.Core;
using UI.Gameplay.Windows;

namespace Levels.StateMachine.States
{
    public class LevelStartState : ILevelState
    {
        private readonly IStateMachine<ILevelState> _levelStateMachine;
        private readonly PlayerHolder _playerHolder;
        private readonly List<Zombie> _zombies;
        private readonly StartWindow _startWindows;
        private readonly WeaponAim _weaponAim;
        private readonly HUD _hud;
        private readonly Platoon _platoon;

        public LevelStartState(IStateMachine<ILevelState> levelStateMachine, PlayerHolder playerHolder, List<Zombie> zombies,
            StartWindow startWindow, WeaponAim weaponAim, HUD hud, Platoon platoon)
        {
            _levelStateMachine = levelStateMachine;
            _playerHolder = playerHolder;
            _zombies = zombies;
            _startWindows = startWindow;
            _weaponAim = weaponAim;
            _hud = hud;
            _platoon = platoon;
        }

        public void Enter()
        {
            _zombies.ForEach(zombie =>
            {
                zombie.TargetFollower.Start();
                zombie.Attacker.Start();
            });
            _playerHolder.Instance.WaypointFollower.Start();
            _levelStateMachine.Enter<LevelLoopState>();
            _startWindows.Hide();
            _weaponAim.Show();
            _hud.Show();
            _platoon.Soldiers.ForEach(soldier => soldier.Aimer.Enabled = true);
        }
    }
}
using Entities.AI;
using Gameplay.Entities.Soldier.StateMachine.States.Core;
using Infrastructure.StateMachine.Main.States.Core;
using UnityEngine;

namespace Gameplay.Entities.Soldier.StateMachine.States
{
    public class NavigationState : ISoldierState, IState, IExitable
    {
        private readonly Transform _placeInPlatoon;
        private readonly AgentTransformFollower _transformFollower;
        private readonly SoldierAimer _aimer;
        private readonly SoldierShooter _shooter;

        public NavigationState(Transform placeInPlatoon, AgentTransformFollower transformFollower, SoldierAimer aimer,
            SoldierShooter shooter)
        {
            _placeInPlatoon = placeInPlatoon;
            _transformFollower = transformFollower;
            _aimer = aimer;
            _shooter = shooter;
        }

        public void Enter()
        {
            _transformFollower.Start(_placeInPlatoon);
            _aimer.Enabled = true;
            _shooter.Enable();
        }

        public void Exit()
        {
            _transformFollower.Stop();
            _aimer.Enabled = false;
            _shooter.Disable();
        }
    }
}
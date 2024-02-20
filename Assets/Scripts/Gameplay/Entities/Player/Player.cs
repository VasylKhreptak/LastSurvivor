using System.Collections.Generic;
using Gameplay.Entities.Health.Core;
using Gameplay.Entities.Health.Damages;
using Gameplay.Entities.Player.StateMachine.States.Core;
using Infrastructure.Services.Vibration.Core;
using Infrastructure.StateMachine.Main.Core;
using Lofelt.NiceVibrations;
using UnityEngine;
using Visitor;
using Zenject;

namespace Gameplay.Entities.Player
{
    [DisallowMultipleComponent]
    public class Player : MonoBehaviour, IVisitable<ZombieDamage>
    {
        private List<Transform> _collectorFollowPoints;
        private IVibrationService _vibrationService;
        public IStateMachine<IPlayerState> StateMachine { get; private set; }

        public IHealth Health { get; private set; }

        [Inject]
        private void Constructor(List<Transform> collectorFollowPoints, IVibrationService vibrationService,
            IStateMachine<IPlayerState> stateMachine, IHealth health)
        {
            Health = health;
            _collectorFollowPoints = collectorFollowPoints;
            StateMachine = stateMachine;
            _vibrationService = vibrationService;
        }

        public void Accept(ZombieDamage visitor)
        {
            Health.TakeDamage(visitor.Value);
            _vibrationService.Vibrate(HapticPatterns.PresetType.RigidImpact);
        }

        public IReadOnlyList<Transform> CollectorFollowPoints => _collectorFollowPoints;
    }
}
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
        private IHealth _health;
        private List<Transform> _collectorFollowPoints;
        public IStateMachine<IPlayerState> StateMachine { get; private set; }
        private IVibrationService _vibrationService;

        [Inject]
        private void Constructor(IHealth health, List<Transform> collectorFollowPoints, IStateMachine<IPlayerState> stateMachine,
            IVibrationService vibrationService)
        {
            _health = health;
            _collectorFollowPoints = collectorFollowPoints;
            StateMachine = stateMachine;
            _vibrationService = vibrationService;
        }

        public void Accept(ZombieDamage visitor)
        {
            _health.TakeDamage(visitor.Value);
            _vibrationService.Vibrate(HapticPatterns.PresetType.RigidImpact);
        }

        public IReadOnlyList<Transform> CollectorFollowPoints => _collectorFollowPoints;
    }
}
using System;
using System.ComponentModel;
using Gameplay.Entities.Collector;
using Gameplay.Entities.Player;
using Gameplay.Entities.Soldier;
using Gameplay.Levels.StateMachine.States;
using Gameplay.Levels.StateMachine.States.Core;
using Gameplay.Weapons.Core;
using Infrastructure.Services.PersistentData.Core;
using Infrastructure.StateMachine.Main.Core;
using UniRx;
using UnityEngine;
using Zenject;
using Object = UnityEngine.Object;

namespace DebuggerOptions
{
    public class LevelOptions : IInitializable, IDisposable
    {
        private readonly IStateMachine<ILevelState> _stateMachine;
        private readonly IPersistentDataService _persistentDataService;

        public LevelOptions(IStateMachine<ILevelState> stateMachine, IPersistentDataService persistentDataService)
        {
            _stateMachine = stateMachine;
            _persistentDataService = persistentDataService;
        }

        private readonly CompositeDisposable _godModeSubscriptions = new CompositeDisposable();

        [Category("Level")] [DisplayName("Initialize Options")]
        public void Initialize()
        {
            SRDebug.Instance.RemoveOptionContainer(this);
            SRDebug.Instance.AddOptionContainer(this);
        }

        [Category("Level")] [DisplayName("Dispose Options")]
        public void Dispose()
        {
            SRDebug.Instance.RemoveOptionContainer(this);
            ExitGodMode();
        }

        [Category("Level")]
        public void PauseLevel() => _stateMachine.Enter<PauseLevelState>();

        [Category("Level")]
        public void ResumeLevel() => _stateMachine.Enter<ResumeLevelState>();

        [Category("Level")]
        public void FailLevel() => _stateMachine.Enter<LevelFailedState>();

        [Category("Level")]
        public void CompleteLevel() => _stateMachine.Enter<LevelCompletedState>();

        [Category("Level")]
        public int Soldiers
        {
            get => _persistentDataService.Data.PlayerData.PlatformsData.BarracksPlatformData.SoldiersBank.Value.Value;
            set => _persistentDataService.Data.PlayerData.PlatformsData.BarracksPlatformData.SoldiersBank.SetValue(value);
        }

        [Category("Level")]
        public int Collectors
        {
            get => _persistentDataService.Data.PlayerData.PlatformsData.CollectorsPlatformData.CollectorsBank.Value.Value;
            set => _persistentDataService.Data.PlayerData.PlatformsData.CollectorsPlatformData.CollectorsBank.SetValue(value);
        }

        [Category("Level")]
        public void EnterGodMode()
        {
            ExitGodMode();

            MonoBehaviour[] behaviours = Object.FindObjectsOfType<MonoBehaviour>();

            foreach (MonoBehaviour behaviour in behaviours)
            {
                if (behaviour is Player player)
                {
                    player.Health.Value.Subscribe(_ => player.Health.Restore()).AddTo(_godModeSubscriptions);
                    return;
                }

                if (behaviour is Soldier soldier)
                {
                    soldier.Health.Value.Subscribe(_ => soldier.Health.Restore()).AddTo(_godModeSubscriptions);
                    return;
                }

                if (behaviour is Collector collector)
                {
                    collector.Health.Value.Subscribe(_ => collector.Health.Restore()).AddTo(_godModeSubscriptions);
                    return;
                }

                if (behaviour.gameObject.TryGetComponent(out IWeapon weapon))
                    weapon.Ammo.Value.Subscribe(_ => weapon.Ammo.Fill()).AddTo(_godModeSubscriptions);
            }
        }

        [Category("Level")]
        public void ExitGodMode() => _godModeSubscriptions.Clear();
    }
}
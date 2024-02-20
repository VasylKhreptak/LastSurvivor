using System;
using System.ComponentModel;
using Gameplay.Entities.Collector;
using Gameplay.Entities.Player;
using Gameplay.Entities.Soldier;
using Gameplay.Weapons.Core;
using Infrastructure.Services.PersistentData.Core;
using UniRx;
using UnityEngine;
using Object = UnityEngine.Object;

namespace DebuggerOptions
{
    public class GameplayOptions
    {
        private readonly IPersistentDataService _persistentDataService;

        public GameplayOptions(IPersistentDataService persistentDataService)
        {
            _persistentDataService = persistentDataService;
        }

        private IDisposable _godModeSubscription;

        [Category("Gameplay")]
        public bool GodMode
        {
            get => _godModeSubscription != null;
            set
            {
                if (value)
                    EnterGodMode();
                else
                    ExitGodMode();
            }
        }

        [Category("Gameplay")]
        public int Soldiers
        {
            get => _persistentDataService.Data.PlayerData.PlatformsData.BarracksPlatformData.SoldiersBank.Value.Value;
            set => _persistentDataService.Data.PlayerData.PlatformsData.BarracksPlatformData.SoldiersBank.SetValue(value);
        }

        [Category("Gameplay")]
        public int Collectors
        {
            get => _persistentDataService.Data.PlayerData.PlatformsData.CollectorsPlatformData.CollectorsBank.Value.Value;
            set => _persistentDataService.Data.PlayerData.PlatformsData.CollectorsPlatformData.CollectorsBank.SetValue(value);
        }

        private void EnterGodMode()
        {
            ExitGodMode();

            _godModeSubscription = Observable.EveryUpdate().Subscribe(_ => GodModeUpdate());
        }

        private void ExitGodMode()
        {
            _godModeSubscription?.Dispose();
            _godModeSubscription = null;
        }

        private void GodModeUpdate()
        {
            MonoBehaviour[] behaviours = Object.FindObjectsOfType<MonoBehaviour>();

            foreach (MonoBehaviour behaviour in behaviours)
            {
                if (behaviour is Player player)
                    player.Health.Restore();

                if (behaviour is Soldier soldier)
                    soldier.Health.Restore();

                if (behaviour is Collector collector)
                    collector.Health.Restore();

                if (behaviour.gameObject.TryGetComponent(out IWeapon weapon))
                    weapon.Ammo.Fill();
            }
        }
    }
}
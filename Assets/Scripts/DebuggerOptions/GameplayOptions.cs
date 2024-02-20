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

        private readonly CompositeDisposable _godModeSubscriptions = new CompositeDisposable();

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

        [Category("Gameplay")]
        public void EnterGodMode()
        {
            ExitGodMode();

            MonoBehaviour[] behaviours = Object.FindObjectsOfType<MonoBehaviour>();

            foreach (MonoBehaviour behaviour in behaviours)
            {
                if (behaviour is Player player)
                    player.Health.Value.Subscribe(_ => player.Health.Restore()).AddTo(_godModeSubscriptions);

                if (behaviour is Soldier soldier)
                    soldier.Health.Value.Subscribe(_ => soldier.Health.Restore()).AddTo(_godModeSubscriptions);

                if (behaviour is Collector collector)
                    collector.Health.Value.Subscribe(_ => collector.Health.Restore()).AddTo(_godModeSubscriptions);

                if (behaviour.gameObject.TryGetComponent(out IWeapon weapon))
                    weapon.Ammo.Value.Subscribe(_ => weapon.Ammo.Fill()).AddTo(_godModeSubscriptions);
            }
        }

        [Category("Gameplay")]
        public void ExitGodMode() => _godModeSubscriptions.Clear();
    }
}
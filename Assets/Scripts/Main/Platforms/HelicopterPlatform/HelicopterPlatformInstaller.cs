﻿using Audio.Players;
using Data.Persistent.Platforms;
using Data.Static.Balance.Platforms;
using Infrastructure.Services.PersistentData.Core;
using Infrastructure.Services.StaticData.Core;
using Main.Platforms.Zones;
using Plugins.Banks;
using UI.ClampedBanks;
using UnityEngine;
using Zenject;

namespace Main.Platforms.HelicopterPlatform
{
    public class HelicopterPlatformInstaller : MonoInstaller
    {
        [Header("References")]
        [SerializeField] private OilBarrelReceiver _oilBarrelReceiver;
        [SerializeField] private ReceiveZone _receiveZone;
        [SerializeField] private AudioPlayer.Preferences _barrelPopAudioPreferences;

        private IntegerBank _bank;
        private ClampedIntegerBank _upgradeContainer;
        private HelicopterPlatformData _platformData;
        private HelicopterPlatformPreferences _helicopterPlatformPreferences;

        [Inject]
        private void Constructor(IPersistentDataService persistentDataService, IStaticDataService staticDataService)
        {
            _bank = persistentDataService.Data.PlayerData.Resources.Gears;
            _upgradeContainer = persistentDataService.Data.PlayerData.PlatformsData.HelicopterPlatformData.UpgradeContainer;
            _platformData = persistentDataService.Data.PlayerData.PlatformsData.HelicopterPlatformData;
            _helicopterPlatformPreferences = staticDataService.Balance.HelicopterPlatformPreferences;
        }

        #region MonoBehaviour

        private void OnValidate()
        {
            _oilBarrelReceiver ??= GetComponentInChildren<OilBarrelReceiver>(true);
            _receiveZone ??= GetComponentInChildren<ReceiveZone>(true);
        }

        #endregion

        public override void InstallBindings()
        {
            Container.BindInstance(_platformData).AsSingle();
            Container.BindInstance(_oilBarrelReceiver).AsSingle();

            BindUpgradeLogic();
            BindReceiveZoneVibration();
            BindBarrelPopAudioPlayer();
            BindPlayerGridSizeUpdater();
        }

        private void BindUpgradeLogic()
        {
            Container.BindInstance(_upgradeContainer).WhenInjectedInto<ClampedBankLeftValueText>();

            Container.BindInstance(_bank).WhenInjectedInto<ReceiveZone>();
            Container.BindInstance(_upgradeContainer).WhenInjectedInto<ReceiveZone>();
            Container.BindInstance(_receiveZone).AsSingle();

            Container.BindInstance(_helicopterPlatformPreferences).WhenInjectedInto<HelicopterPlatformUpgrader>();
            Container.BindInterfacesTo<HelicopterPlatformUpgrader>().AsSingle();
        }

        private void BindReceiveZoneVibration() => Container.BindInterfacesTo<ReceiveZoneVibration>().AsSingle();

        private void BindBarrelPopAudioPlayer() => Container.Bind<AudioPlayer>().AsSingle().WithArguments(_barrelPopAudioPreferences);

        private void BindPlayerGridSizeUpdater() => Container.BindInterfacesTo<PlayerGridSizeUpdater>().AsSingle();
    }
}
using Data.Persistent.Platforms;
using Data.Static.Balance.Upgrade;
using Infrastructure.Data.Static;
using Infrastructure.Data.Static.Core;
using Infrastructure.Services.PersistentData.Core;
using Infrastructure.Services.StaticData.Core;
using Plugins.Banks;
using UI.ClampedBanks;
using UnityEngine;
using Zenject;

namespace Platforms.HelicopterPlatform
{
    public class HelicopterPlatformInstaller : MonoInstaller
    {
        [Header("References")]
        [SerializeField] private OilBarrelReceiver _oilBarrelReceiver;
        [SerializeField] private ReceiveZone _receiveZone;

        private IntegerBank _bank;
        private ClampedIntegerBank _upgradeContainer;
        private HelicopterPlatformData _platformData;
        private HelicopterPlatformUpgradePreferences _helicopterPlatformUpgradePreferences;

        [Inject]
        private void Constructor(IPersistentDataService persistentDataService, IStaticDataService staticDataService)
        {
            _bank = persistentDataService.PersistentData.PlayerData.Resources.Gears;
            _upgradeContainer = persistentDataService.PersistentData.PlayerData.HelicopterPlatformData.UpgradeContainer;
            _platformData = persistentDataService.PersistentData.PlayerData.HelicopterPlatformData;
            _helicopterPlatformUpgradePreferences = staticDataService.Balance.HelicopterPlatformUpgradePreferences;
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
        }

        private void BindUpgradeLogic()
        {
            Container.BindInstance(_upgradeContainer).WhenInjectedInto<ClampedBankLeftValueText>();

            Container.BindInstance(_bank).WhenInjectedInto<ReceiveZone>();
            Container.BindInstance(_upgradeContainer).WhenInjectedInto<ReceiveZone>();
            Container.BindInstance(_receiveZone).AsSingle();

            Container.BindInstance(_helicopterPlatformUpgradePreferences).WhenInjectedInto<HelicopterPlatformUpgrader>();
            Container.BindInterfacesTo<HelicopterPlatformUpgrader>().AsSingle();
        }
    }
}
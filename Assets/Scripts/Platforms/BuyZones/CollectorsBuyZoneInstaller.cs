using Infrastructure.Data.Static.Core;
using Infrastructure.Services.PersistentData.Core;
using Platforms.BuyZones.Core;
using Platforms.Zones;
using Plugins.Banks;
using UI.ClampedBanks;
using UnityEngine;
using Zenject;

namespace Platforms.BuyZones
{
    public class CollectorsBuyZoneInstaller : MonoInstaller
    {
        [Header("References")]
        [SerializeField] private ReceiveZone _receiveZone;

        private ClampedIntegerBank _buyContainer;
        private IntegerBank _bank;

        [Inject]
        private void Constructor(IPersistentDataService persistentDataService)
        {
            _buyContainer = persistentDataService.PersistentData.PlayerData.PlatformsData.CollectorsPlatformData.BuyContainer;
            _bank = persistentDataService.PersistentData.PlayerData.Resources.Money;
        }

        #region MonoBehaviour

        private void OnValidate() => _receiveZone ??= GetComponentInChildren<ReceiveZone>(true);

        #endregion

        public override void InstallBindings()
        {
            BindBuyZone();
            BindPlatformBuyer();
        }

        private void BindBuyZone()
        {
            Container.BindInstance(_buyContainer).WhenInjectedInto<ClampedBankLeftValueText>();
            Container.BindInstance(_bank).WhenInjectedInto<ReceiveZone>();
            Container.BindInstance(_buyContainer).WhenInjectedInto<ReceiveZone>();
            Container.BindInstance(_receiveZone).AsSingle();
        }

        private void BindPlatformBuyer()
        {
            Container.BindInterfacesAndSelfTo<PlatformBuyer>().AsSingle().WithArguments(Prefab.CollectorsPlatform);
        }
    }
}
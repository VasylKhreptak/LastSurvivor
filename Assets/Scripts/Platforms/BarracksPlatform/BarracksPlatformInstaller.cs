using Data.Persistent.Platforms;
using Data.Static.Balance.Platforms;
using Infrastructure.Services.PersistentData.Core;
using Infrastructure.Services.StaticData.Core;
using Platforms.RecruitmentLogic;
using Platforms.Zones;
using Plugins.Banks;
using UI.ClampedBanks;
using UnityEngine;
using Zenject;

namespace Platforms.BarracksPlatform
{
    public class BarracksPlatformInstaller : MonoInstaller
    {
        [Header("References")]
        [SerializeField] private ReceiveZone _hireSoldierZone;
        [SerializeField] private EntityRecruiter _soldiersRecruiter;

        private IntegerBank _bank;
        private BarracksPlatformData _platformData;
        private BarracksPlatformPreferences _platformPreferences;

        [Inject]
        private void Constructor(IPersistentDataService persistentDataService, IStaticDataService staticDataService)
        {
            _bank = persistentDataService.PersistentData.PlayerData.Resources.Money;
            _platformData = persistentDataService.PersistentData.PlayerData.PlatformsData.BarracksPlatformData;
            _platformPreferences = staticDataService.Balance.BarracksPlatformPreferences;
        }

        #region

        private void OnValidate()
        {
            _hireSoldierZone ??= GetComponentInChildren<ReceiveZone>(true);
            _soldiersRecruiter ??= GetComponentInChildren<EntityRecruiter>(true);
        }

        #endregion

        public override void InstallBindings()
        {
            Container.BindInstance(_platformData).AsSingle();
            BindSoldierHireZone();
            BindSoldiersCountText();
            BindSoldiersRecruiter();
            BindSoldierPriceUpdater();
        }

        private void BindSoldierHireZone()
        {
            Container.BindInstance(_platformData.SoldiersBank).WhenInjectedInto<ReceiveZoneDrawer>();
            Container.BindInstance(_platformData.HireSoldierBank).WhenInjectedInto<ClampedBankLeftValueText>();
            Container.BindInstance(_bank).WhenInjectedInto<ReceiveZone>();
            Container.BindInstance(_platformData.HireSoldierBank).WhenInjectedInto<ReceiveZone>();
            Container.BindInstance(_hireSoldierZone).AsSingle();
        }

        private void BindSoldiersCountText()
        {
            Container.BindInstance(_platformData.SoldiersBank).WhenInjectedInto<ClampedBankValuesText>();
        }

        private void BindSoldiersRecruiter()
        {
            Container.BindInstance(_platformData.SoldiersBank).WhenInjectedInto<EntityRecruiter>();
            Container.BindInstance(_soldiersRecruiter).AsSingle();
        }

        private void BindSoldierPriceUpdater()
        {
            Container
                .BindInterfacesTo<EntityPriceUpdater>()
                .AsSingle()
                .WithArguments(_platformData.SoldiersBank,
                    _platformData.HireSoldierBank,
                    _platformPreferences.SoldierHirePricePreferences);
        }
    }
}
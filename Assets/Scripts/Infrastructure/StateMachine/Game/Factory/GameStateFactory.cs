using System;
using System.Collections.Generic;
using Infrastructure.StateMachine.Game.States;
using Infrastructure.StateMachine.Game.States.Core;
using Infrastructure.StateMachine.Main.States.Core;
using Infrastructure.StateMachine.Main.States.Factory;
using Zenject;

namespace Infrastructure.StateMachine.Game.Factory
{
    public class GameStateFactory : StateFactory
    {
        public GameStateFactory(DiContainer container) : base(container) { }

        protected override Dictionary<Type, Func<IBaseState>> BuildStatesRegister() =>
            new Dictionary<Type, Func<IBaseState>>
            {
                //chained
                [typeof(BootstrapState)] = _container.Resolve<BootstrapState>,
                [typeof(BootstrapPlayServicesState)] = _container.Resolve<BootstrapPlayServicesState>,
                [typeof(LoginState)] = _container.Resolve<LoginState>,
                [typeof(LoadDataState)] = _container.Resolve<LoadDataState>,
                [typeof(ApplySavedSettingsState)] = _container.Resolve<ApplySavedSettingsState>,
                [typeof(BootstrapFirebaseState)] = _container.Resolve<BootstrapFirebaseState>,
                [typeof(BootstrapAnalyticsState)] = _container.Resolve<BootstrapAnalyticsState>,
                [typeof(BootstrapCrashlyticsState)] = _container.Resolve<BootstrapCrashlyticsState>,
                [typeof(BootstrapAdvertisementsState)] = _container.Resolve<BootstrapAdvertisementsState>,
                [typeof(BootstrapMessagingState)] = _container.Resolve<BootstrapMessagingState>,
                [typeof(SetupBackgroundMusicState)] = _container.Resolve<SetupBackgroundMusicState>,
                [typeof(SetupAutomaticDataSaveState)] = _container.Resolve<SetupAutomaticDataSaveState>,
                [typeof(ScheduleNotificationsState)] = _container.Resolve<ScheduleNotificationsState>,
                [typeof(LoadMainSceneState)] = _container.Resolve<LoadMainSceneState>,
                [typeof(GameLoopState)] = _container.Resolve<GameLoopState>,
                //other
                [typeof(ReloadState)] = _container.Resolve<ReloadState>,
                [typeof(SaveDataState)] = _container.Resolve<SaveDataState>,
                [typeof(LoadSceneAsyncState)] = _container.Resolve<LoadSceneAsyncState>,
                [typeof(LoadSceneWithTransitionAsyncState)] = _container.Resolve<LoadSceneWithTransitionAsyncState>,
                [typeof(PlayState)] = _container.Resolve<PlayState>,
                [typeof(LoadLevelState)] = _container.Resolve<LoadLevelState>
            };
    }
}
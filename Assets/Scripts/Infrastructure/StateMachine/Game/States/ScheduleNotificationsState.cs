using System;
using Extensions;
using Infrastructure.Services.Log.Core;
using Infrastructure.Services.Notification.Core;
using Infrastructure.Services.PersistentData.Core;
using Infrastructure.Services.StaticData.Core;
using Infrastructure.StateMachine.Game.States.Core;
using Infrastructure.StateMachine.Main.Core;
using Infrastructure.StateMachine.Main.States.Core;
using Notifications;

namespace Infrastructure.StateMachine.Game.States
{
    public class ScheduleNotificationsState : IGameState, IState
    {
        private readonly IStateMachine<IGameState> _stateMachine;
        private readonly INotificationService _notificationService;
        private readonly IPersistentDataService _persistentDataService;
        private readonly ILogService _logService;
        private readonly IStaticDataService _staticDataService;

        public ScheduleNotificationsState(IStateMachine<IGameState> stateMachine, INotificationService notificationService,
            IPersistentDataService persistentDataService, ILogService logService, IStaticDataService staticDataService)
        {
            _stateMachine = stateMachine;
            _notificationService = notificationService;
            _persistentDataService = persistentDataService;
            _logService = logService;
            _staticDataService = staticDataService;
        }

        public void Enter()
        {
            _logService.Log("ScheduleNotificationsState");
            CancelScheduledNotifications();
            ScheduleNotifications();
            LoadNextState();
        }

        private void CancelScheduledNotifications()
        {
            foreach (var notificationID in _persistentDataService.Data.PlayerData.ScheduledNotificationIDs)
                _notificationService.CancelScheduledNotification(notificationID);

            _persistentDataService.Data.PlayerData.ScheduledNotificationIDs.Clear();
        }

        private void ScheduleNotifications() => ScheduleRetentionNotification();

        private void ScheduleRetentionNotification()
        {
            ScheduleRetentionNotification(TimeSpan.FromSeconds(10f));
            ScheduleRetentionNotification(TimeSpan.FromSeconds(10f));
            ScheduleRetentionNotification(TimeSpan.FromSeconds(10f));
            ScheduleRetentionNotification(TimeSpan.FromSeconds(10f));
            ScheduleRetentionNotification(TimeSpan.FromSeconds(10f));
            ScheduleRetentionNotification(TimeSpan.FromSeconds(10f));
            ScheduleRetentionNotification(TimeSpan.FromDays(1));
            ScheduleRetentionNotification(TimeSpan.FromDays(3));
            ScheduleRetentionNotification(TimeSpan.FromDays(7));
            ScheduleRetentionNotification(TimeSpan.FromDays(14));
            ScheduleRetentionNotification(TimeSpan.FromDays(30));
        }

        private void ScheduleRetentionNotification(TimeSpan delay)
        {
            BaseNotificationData baseNotificationData = _staticDataService.Config.RetentionNotifications.Random();
            int id = _notificationService.SendNotification(baseNotificationData.Title, baseNotificationData.Message, delay);
            _persistentDataService.Data.PlayerData.ScheduledNotificationIDs.Add(id);
        }

        private void LoadNextState() => _stateMachine.Enter<LoadMainSceneState>();
    }
}
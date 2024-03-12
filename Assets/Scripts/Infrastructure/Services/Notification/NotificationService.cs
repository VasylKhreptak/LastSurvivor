using System;
using Infrastructure.Services.Notification.Core;
using Unity.Notifications.Android;
using Zenject;

namespace Infrastructure.Services.Notification
{
    public class NotificationService : INotificationService, IInitializable
    {
        private const string SmallIcon = "default_small";
        private const string LargeIcon = "default_large";

        private AndroidNotificationChannel _defaultChannel;

        public void Initialize()
        {
            _defaultChannel = CreateDefaultNotificationChannel();
            RegisterNotificationChannel(_defaultChannel);
        }

        private void RegisterNotificationChannel(AndroidNotificationChannel channel) =>
            AndroidNotificationCenter.RegisterNotificationChannel(channel);

        private AndroidNotificationChannel CreateDefaultNotificationChannel()
        {
            AndroidNotificationChannel channel = new AndroidNotificationChannel
            {
                Id = "default_channel",
                Name = "Default Channel",
                Importance = Importance.Default,
                Description = "Generic notifications",
            };

            return channel;
        }

        public int SendNotification(string title, string text, TimeSpan delay)
        {
            AndroidNotification notification = new AndroidNotification
            {
                Title = title,
                Text = text,
                FireTime = DateTime.Now.AddSeconds(delay.TotalSeconds),
                SmallIcon = SmallIcon,
                LargeIcon = LargeIcon
            };

            return AndroidNotificationCenter.SendNotification(notification, _defaultChannel.Id);
        }

        public void CancelScheduledNotification(int id) => AndroidNotificationCenter.CancelScheduledNotification(id);
    }
}
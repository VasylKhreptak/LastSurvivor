using System;

namespace Infrastructure.Services.Notification.Core
{
    public interface INotificationService
    {
        public int SendNotification(string title, string text, TimeSpan delay);

        public void CancelScheduledNotification(int id);
    }
}
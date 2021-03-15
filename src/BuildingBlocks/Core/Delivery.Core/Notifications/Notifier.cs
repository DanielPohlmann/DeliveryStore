using Delivery.Core.DomainObjects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Delivery.Core.Notifications
{
    public class Notifier : INotifier
    {
        private List<Notification> _notifications;

        public Notifier()
        {
            _notifications = new List<Notification>();
        }

        public void Handle(Notification notification)
        {
            _notifications.Add(notification);
        }
        public void Handle(IEnumerable<Notification> notifications)
        {
            _notifications.AddRange(notifications);
        }

        public List<Notification> GetNotification()
        {
            return _notifications;
        }

        public bool ExistNotification()
        {
            return _notifications.Any();
        }
    }
}

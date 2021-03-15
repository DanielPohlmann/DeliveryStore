using Delivery.Core.DomainObjects;
using System.Collections.Generic;

namespace Delivery.Core.Notifications
{
    public interface INotifier
    {
        bool ExistNotification();
        List<Notification> GetNotification();
        void Handle(Notification notification);
        void Handle(IEnumerable<Notification> notification);
    }
}

using System;
using System.Collections.Generic;
using System.Text;

namespace WePray.Services.GeneralServices
{
    public interface ILocalNotificationService
    {
        void LocalNotification(string title, string body, int id, DateTime notifyTime);
       
    }
}

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Serialization;
using Android.App;
using Android.Content;
using Android.Graphics;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Support.V4.App;
using Android.Views;
using Android.Widget;
using AndroidX.Core.App;
using Java.Lang;
using WePray.Droid.Services;
using WePray.Models;
using WePray.Services.GeneralServices;
using AndroidApp = Android.App.Application;

[assembly: Xamarin.Forms.Dependency(typeof(LocalNotificationService))]
namespace WePray.Droid.Services
{
    public class LocalNotificationService : ILocalNotificationService
    {
        int _notificationIconId { get; set; }
        readonly DateTime _jan1st1970 = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        internal string _randomNumber;

        public const string channelId = "default";
        const string channelName = "Default";
        const string channelDescription = "The default channel for notifications.";
        const int pendingIntentId = 0;

       
        public const string TitleKey = "title";
        public const string MessageKey = "message";

        bool channelInitialized = false;
        int messageId = -1;
        NotificationManager manager;

        public void Initialize()
        {
            CreateNotificationChannel();
        }

        void CreateNotificationChannel()
        {
            manager = (NotificationManager)AndroidApp.Context.GetSystemService(AndroidApp.NotificationService);

            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                var channelNameJava = new Java.Lang.String(channelName);
                var channel = new NotificationChannel(channelId, channelNameJava, NotificationImportance.Default)
                {
                    Description = channelDescription
                };
                manager.CreateNotificationChannel(channel);
            }

            channelInitialized = true;
        }

        public void LocalNotification(string title, string body, int id, DateTime notifyTime)
        {

            if (!channelInitialized)
            {
                CreateNotificationChannel();
            }
            //long repeateDay = 1000 * 60 * 60 * 24;    
            long repeateForMinute = 60000; // In milliseconds   
            long totalMilliSeconds = (long)(notifyTime.ToUniversalTime() - _jan1st1970).TotalMilliseconds;
            if (totalMilliSeconds < JavaSystem.CurrentTimeMillis())
            {
                totalMilliSeconds = totalMilliSeconds + repeateForMinute;
            }

            var intent = new Intent(AndroidApp.Context, typeof(MainActivity));
            intent.PutExtra(TitleKey, title);
            intent.PutExtra(MessageKey, body);

            PendingIntent pendingIntent = PendingIntent.GetActivity(AndroidApp.Context, pendingIntentId, intent, PendingIntentFlags.OneShot);

            var localNotification = new LocalNotification();
            localNotification.Title = title;
            localNotification.Body = body;
            localNotification.Id = id;
            localNotification.NotifyTime = notifyTime;
            

            if (_notificationIconId != 0)
            {
                localNotification.IconId = _notificationIconId;
            }
            else
            {
                localNotification.IconId = Resource.Drawable.ASwePray;
            }

            var serializedNotification = SerializeNotification(localNotification);
            intent.PutExtra(ScheduledAlarmHandler.LocalNotificationKey, serializedNotification);
            
             Random generator = new Random();
            _randomNumber = generator.Next(100000, 999999).ToString("D6");

          //  var pendingIntent = PendingIntent.GetBroadcast(Application.Context, Convert.ToInt32(_randomNumber), intent, PendingIntentFlags.Immutable);
            var alarmManager = GetAlarmManager();
            alarmManager.SetRepeating(AlarmType.RtcWakeup, totalMilliSeconds, repeateForMinute, pendingIntent);
        }

        public void Cancel(int id)
        {

            var intent = CreateIntent(id);
            var pendingIntent = PendingIntent.GetBroadcast(Application.Context, Convert.ToInt32(_randomNumber), intent, PendingIntentFlags.Immutable);
            var alarmManager = GetAlarmManager();
            alarmManager.Cancel(pendingIntent);
            var notificationManager = NotificationManagerCompat.From(Application.Context);
            notificationManager.CancelAll();
            notificationManager.Cancel(id);
        }

        public static Intent GetLauncherActivity()
        {

            var packageName = Application.Context.PackageName;
            return Application.Context.PackageManager.GetLaunchIntentForPackage(packageName);
        }


        private Intent CreateIntent(int id)
        {

            return new Intent(Application.Context, typeof(ScheduledAlarmHandler))
                .SetAction("LocalNotifierIntent" + id);
        }

        private AlarmManager GetAlarmManager()
        {

            var alarmManager = Application.Context.GetSystemService(Context.AlarmService) as AlarmManager;
            return alarmManager;
        }

        private string SerializeNotification(LocalNotification notification)
        {

            var xmlSerializer = new XmlSerializer(notification.GetType());

            using (var stringWriter = new StringWriter())
            {
                xmlSerializer.Serialize(stringWriter, notification);
                return stringWriter.ToString();
            }
        }
    }

    [BroadcastReceiver(Enabled = true, Label = "Local Notifications Broadcast Receiver")]
    public class ScheduledAlarmHandler : BroadcastReceiver
    {

        public const string LocalNotificationKey = "LocalNotification";
        public override void OnReceive(Context context, Intent intent)
        {
            var extra = intent.GetStringExtra(LocalNotificationKey);

            var notification = DeserializeNotification(extra);
            //Generating notification   
            //NotificationCompat.Builder builder = new NotificationCompat.Builder(AndroidApp.Context, channelId)
            //    .SetContentIntent(pendingIntent)
            //    .SetContentTitle(title)
            //    .SetContentText(message)
            //    .SetLargeIcon(BitmapFactory.DecodeResource(AndroidApp.Context.Resources, Resource.Drawable.xamagonBlue))
            //    .SetSmallIcon(Resource.Drawable.xamagonBlue)
            //    .SetDefaults((int)NotificationDefaults.Sound | (int)NotificationDefaults.Vibrate);
            NotificationCompat.Builder builder = new NotificationCompat.Builder(Application.Context, LocalNotificationService.channelId)
                .SetContentTitle(notification.Title)
                .SetContentText(notification.Body)
                .SetLargeIcon(BitmapFactory.DecodeResource(AndroidApp.Context.Resources, Resource.Drawable.ASwePray))
                .SetSmallIcon(notification.IconId)
                .SetDefaults((int)NotificationDefaults.Sound | (int)NotificationDefaults.Vibrate)
                .SetAutoCancel(true);

            var resultIntent = LocalNotificationService.GetLauncherActivity();
            resultIntent.SetFlags(ActivityFlags.NewTask | ActivityFlags.ClearTask);
            Android.App.TaskStackBuilder stackBuilder = Android.App.TaskStackBuilder.Create(Application.Context);
            stackBuilder.AddNextIntent(resultIntent);

            Random random = new Random();
            int randomNumber = random.Next(9999 - 1000) + 1000;

            var resultPendingIntent =
                stackBuilder.GetPendingIntent(randomNumber, PendingIntentFlags.Immutable);
            builder.SetContentIntent(resultPendingIntent);
            // Sending notification    
            
            //   var notificationManager = NotificationManagerCompat.From(Application.Context);
              var notificationManager = (NotificationManager)AndroidApp.Context.GetSystemService(AndroidApp.NotificationService);
            notificationManager.Notify(randomNumber, builder.Build());
        }

        private LocalNotification DeserializeNotification(string notificationString)
        {

            var xmlSerializer = new XmlSerializer(typeof(LocalNotification));
            using (var stringReader = new StringReader(notificationString))
            {
                var notification = (LocalNotification)xmlSerializer.Deserialize(stringReader);
                return notification;
            }
        }
    }
}  

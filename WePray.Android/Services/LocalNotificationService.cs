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
using AndroidX.Work;
using Java.Lang;
using WePray.Droid.Services;
using WePray.Models;
using WePray.Services.GeneralServices;
using AndroidApp = Android.App.Application;

[assembly: Xamarin.Forms.Dependency(typeof(LocalNotificationService))]
namespace WePray.Droid.Services
{
    public class LocalNotificationService 
    {
        
    }

    public class NotifyWorker : Worker
    {
        //we set a tag to be able to cancel all work of this type if needed
        public const string workTag = "notificationWork";

        private const string _weprayNotificationChannel = "Wepray Prayer Reminder";

        //notification id i use to get the notification ?
        private const int _notificationId = 111;

        
        public NotifyWorker(Context context, WorkerParameters workerParams) : base(context, workerParams)
        {

        }


        public override Result DoWork()
        {
            // Method to trigger an instant notification
            LocalNotification("Daily Devotional", "Dont forget to pray today");

            return Result.InvokeSuccess();

        }


        // Create the NotificationChannel, but only on API 26+ because
        // the NotificationChannel class is new and not in the support library
        private void BuildnotificationChannel()
        {
            if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            {
                //define the importance level of the notification
                var importance = NotificationImportance.High;

                //build the actual notification channel, giving it a unique ID and name
                NotificationChannel channel =
                  new NotificationChannel(_weprayNotificationChannel, _weprayNotificationChannel, importance);

                //we can optionally add a description for the channel
                string description = "A channel which shows notifications to remind people to open app";
                channel.Description = description;

                //we can optionally set notification LED colour
                channel.EnableLights(true);
                channel.LightColor = Color.ParseColor("#0f1ea5");

                // Register the channel with the system
                NotificationManager notificationManager = (NotificationManager)AndroidApp.Context.GetSystemService(Context.NotificationService);


                if (notificationManager != null)
                {
                    notificationManager.CreateNotificationChannel(channel);
                }
            }
        }

        private void LocalNotification(string title, string body)
        {

            BuildnotificationChannel();

            // create an intent to open the event details activity
            Intent intent = new Intent(AndroidApp.Context, typeof(SplashActivity));

            //put together the PendingIntent
            PendingIntent pendingIntent = PendingIntent.GetActivity(AndroidApp.Context, 1, intent, PendingIntentFlags.UpdateCurrent);

            CreateNotification(title, body, pendingIntent);
        }


        private void CreateNotification(string title, string body, PendingIntent pendingIntent)
        {
            // get latest event details
            string notificationTitle = title;
            string notificationText = body;

            //build the notification
            NotificationCompat.Builder notificationBuilder =
            new NotificationCompat.Builder(AndroidApp.Context, _weprayNotificationChannel)
            .SetSmallIcon(Resource.Drawable.pray)
            .SetContentTitle(notificationTitle)
            .SetContentText(notificationText)
            .SetContentIntent(pendingIntent)
            .SetAutoCancel(true)
            .SetPriority(NotificationCompat.PriorityHigh);

            //trigger the notification
            NotificationManagerCompat notificationManager =
            NotificationManagerCompat.From(AndroidApp.Context);

            //we give each notification the ID of the event it's describing, 
            //to ensure they all show up and there are no duplicates
            notificationManager.Notify(_notificationId, notificationBuilder.Build());

        }
    }
}  

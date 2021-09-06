using System;

using Android.App;
using Android.Content.PM;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Android.OS;
using Android.Gms.Ads;
using Android.Content;
using WePray.Droid.DependencyInject;
using AndroidX.Work;
using WePray.Droid.Services;
using Java.Lang;
using Android.Icu.Util;

namespace WePray.Droid
{
    [Activity(Theme = "@style/MainTheme", MainLauncher = false)]
    public class MainActivity : global::Xamarin.Forms.Platform.Android.FormsAppCompatActivity
    {
        protected override void OnCreate(Bundle savedInstanceState)
        {
            TabLayoutResource = Resource.Layout.Tabbar;
            ToolbarResource = Resource.Layout.Toolbar;

            base.OnCreate(savedInstanceState);
            MobileAds.Initialize(this); 
            global::Xamarin.Forms.Forms.SetFlags("CollectionView_Experimental");
            Rg.Plugins.Popup.Popup.Init(this);
            Xamarin.Essentials.Platform.Init(this, savedInstanceState);
            global::Xamarin.Forms.Forms.Init(this, savedInstanceState);
            Bootstrapper.Init();
            LoadApplication(new App());


            var timeinmilliseconds = GetNotifyTime();

            //This handles the background task and the repeating even when app is killed
            PeriodicWorkRequest taxWorkRequest = PeriodicWorkRequest.Builder.From<NotifyWorker>(TimeSpan.FromMilliseconds(timeinmilliseconds)).Build();

            WorkManager.GetInstance(this).Enqueue(taxWorkRequest);

        }
        public override void OnRequestPermissionsResult(int requestCode, string[] permissions, [GeneratedEnum] Android.Content.PM.Permission[] grantResults)
        {
            Xamarin.Essentials.Platform.OnRequestPermissionsResult(requestCode, permissions, grantResults);

            base.OnRequestPermissionsResult(requestCode, permissions, grantResults);
        }

        private long GetNotifyTime()
        {
            // Set the alarm to start at approximately 8:00 a.m.
            Calendar calendar = Calendar.Instance;
            calendar.TimeInMillis = JavaSystem.CurrentTimeMillis();
            calendar.Set(CalendarField.HourOfDay, 8);
            return calendar.TimeInMillis;
        }

    }
}
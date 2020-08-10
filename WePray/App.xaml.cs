using System;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;
using WePray.Services;
using WePray.Views;
using Xamarin.Essentials;
using WePray.Dependencies;
using WePray.Services.GeneralServices;

namespace WePray
{
    public partial class App : Application
    {
        //to get 6am tomorrow add 24 to 6 = 30
        //it takes 30 hours from start of the day to get to 6am tomorrow
        //so you can get current timr and subtract it from 30 to get the remeining time to get to 6am
        public App()
        {
            InitializeComponent();

            DependencyService.Get<ILocalNotificationService>().LocalNotification("Daily Prayer", "Dont forget to pray today", 0, DateTime.Now.AddHours(30 - DateTime.Now.Hour));
            DependencyService.Register<IAudio>();
            if (!Preferences.ContainsKey("BibleID"))
            {
                Preferences.Set("Bversion", "NIV");
                Preferences.Set("BibleID","de4e12af7f28f599-02");
                Preferences.Get("BookPickedID", "GEN");
                Preferences.Set("ChapterPicked", "1");
                Preferences.Set("BookChapter", "GEN 1");
            }
            Preferences.Set("TodaysDate", DateTime.Now.Date);
            MainPage = new AppShell();
        }

        protected override void OnStart()
        {
        }

        protected override void OnSleep()
        {
        }

        protected override void OnResume()
        {
        }
    }
}

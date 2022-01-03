using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WePray.Models;
using WePray.Repository;
using WePray.Services.Connection;
using WePray.Services.WordPressServices;
using Xamarin.Forms;

namespace WePray.ViewModels
{
    class DailyDevotionalViewModel : BaseViewModel
    {      
        INavigation navigation;

        IWPServices wPServices;

        public DailyDevotionalViewModel(IConnection connection, IRepository repository) : base(connection, repository)
        {
            wPServices = new WPServices();
            ConnectionServices.CheckWifiContinuously();

        }


        bool actSeen;
        public bool ActSeen
        {
            get => actSeen;
            set => SetProperty(ref actSeen, value);
        }


        ObservableCollection<Prayer> Devotional = new ObservableCollection<Prayer>();

        public ObservableCollection<Prayer> Devotionals { get => Devotional; set => SetProperty(ref Devotional, value); }


        public ICommand LoadItemsCommand => new Command(async () =>
        {
            if (ConnectionServices.CheckWifi())
            {
                IsBusy = true;

                Setlastdatechecked();
                IsBusy = false;
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("", "Please connect to the internet to refresh", "ok");
            }
        });


        public async Task GetAllDevotionals()
        {
            if (ConnectionServices.CheckWifi())
            {
                try
                {
                    ActSeen = true;
                    Devotionals = await Repository.GetAllDevotionalsFromDatabase();
                    ActSeen = false;
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    ActSeen = false;
                    await Application.Current.MainPage.DisplayAlert("No internet connection", "Please connect to the internet", "ok");
                }
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("No internet connection", "Please connect to the internet", "ok");
            }

        }

        private void Setlastdatechecked()
        {
            Repository.GetPrayersFromWP(Devotionals);
        }

    }
}

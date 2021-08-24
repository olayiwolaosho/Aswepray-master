using HtmlAgilityPack;
using Newtonsoft.Json;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using WePray.AllConstants;
using WePray.Models;
using WePray.Repository;
using WePray.Services.Connection;
using WePray.Services.WordPressServices;
using WePrayWPResponseObject;
using WordPressPCL;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace WePray.ViewModels
{
    class DailyPrayerViewModel : BaseViewModel
    {
        ObservableCollection<Prayer> prayers = new ObservableCollection<Prayer>();
        public ObservableCollection<Prayer> Prayers { get => prayers; set => SetProperty(ref prayers,value); }

        INavigation navigation;
        IWPServices wPServices;
        
        public DailyPrayerViewModel(IConnection connection, IRepository repository) : base(connection,repository)
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


        public ICommand LoadItemsCommand => new Command(async() => 
        {
            if(ConnectionServices.CheckWifi())
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


        /// <summary>
        /// Code to update app will have to change the way it works it hould not no about wp services from view model
        /// </summary>
        /// <returns></returns>
        private async Task UpdateApp()
        {
            var update = await wPServices.GetTagName<WPResponseObject>();
            string stringAfterChar = update.Name.Substring(update.Name.IndexOf("_") + 1);

            var version = AppInfo.BuildString.ToString();

            if (version != stringAfterChar)
            {
                await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PushAsync(new Views.Popups.UpdateModal());
            }

        }


        public async Task GetAllPrayers()
        {
            if(ConnectionServices.CheckWifi()){
                try
                {
                    ActSeen = true;
                    Prayers = await Repository.GetAllPrayersFromDatabase();
                    ActSeen = false;
                    await UpdateApp();
                }
                catch(Exception e)
                {
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
             Repository.GetPrayersFromWP(Prayers);
        }

        
        public void Search(string Newtextvalue)
        {
          Prayers =  new  ObservableCollection<Prayer>(Prayers.Where(c => c.Title.ToUpper().Contains(Newtextvalue) || c.Title.ToLower().Contains(Newtextvalue)));
        }

    }
}

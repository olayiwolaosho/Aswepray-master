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
using WePray.Services.WordPressServices;
using WePrayWPResponseObject;
using WordPressPCL;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace WePray.ViewModels
{
    class DailyPrayerViewModel : BaseViewModel
    {
        ObservableCollection<Prayer> prayers;
        public ObservableCollection<Prayer> Prayers { get => prayers; set => SetProperty(ref prayers,value); }

        ObservableCollection<WPResponseObject> wpsongs;

        List<string> htmltext;

 
        public ObservableCollection<WPResponseObject> WpSongs { get => wpsongs; set => SetProperty(ref wpsongs, value); }
        INavigation navigation;
        IWPServices wPServices;
        public DailyPrayerViewModel(INavigation navigation)
        {
            this.navigation = navigation;
            wPServices = new WPServices();
            Checkwifionstart();
            Checkwificontinuously();

        }


        bool actSeen;
        public bool ActSeen
        {
            get => actSeen;
            set => SetProperty(ref actSeen, value);
        }

        string conn;

        public string Conn
        {
            get => conn;
            set => SetProperty(ref conn, value);
        }

        public ICommand LoadItemsCommand => new Command(async() => 
        {
            if(Conn == "online")
            {
                IsBusy = true;

                await Setlastdatechecked();
                IsBusy = false;
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("", "Please connect to the internet to refresh", "ok");
            }
        });


        public async Task GetAllPrayers()
        {
            //  if(Connectivity.NetworkAccess == NetworkAccess.Internet)
            //{
            try
            {
                ActSeen = true;
                Prayers = new ObservableCollection<Prayer>();
                if (Preferences.ContainsKey("Lastdatechecked"))
                {
              
                    var d1 = Preferences.Get("Lastdatechecked", default(DateTime));
                    var d2 = Preferences.Get("TodaysDate", default(DateTime));
                    if (d1 != d2)
                    {
                        WpSongs = new ObservableCollection<WPResponseObject>(await wPServices.GetAllPrayers<WPResponseObject>());

                        foreach (var item in WpSongs)
                        {
                            Prayers.Add(new Prayer
                            {
                                Id = item.Id,
                                Title = item.Title.Rendered,
                                image = await GetFeaturedImage.featuredimage(Constants.Image_Url + item.FeaturedMedia.ToString()),
                                Date = item.Date,
                                Description = Gettext(item.Content.Rendered)
                            });
                        }
                        Application.Current.Properties["allprayersjson"] = JsonConvert.SerializeObject(Prayers);
                        Preferences.Set("Lastdatechecked", DateTime.Now.Date);
                    }
                    else
                    {
                        var allsongsjson = Application.Current.Properties["allprayersjson"].ToString();
                        Prayers = new ObservableCollection<Prayer>(JsonConvert.DeserializeObject<IEnumerable<Prayer>>(allsongsjson));
                    }
                    ActSeen = false;
                  
                }
                else
                {
                   await Setlastdatechecked();
                    ActSeen = false;
                }
            }
            catch(Exception)
            {
                ActSeen = false;
                await Application.Current.MainPage.DisplayAlert("No internet connection", "Please connect to the internet", "ok");
            }
        }

        private async Task Setlastdatechecked()
        {
            WpSongs = new ObservableCollection<WPResponseObject>(await wPServices.GetAllPrayers<WPResponseObject>());
            Prayers = new ObservableCollection<Prayer>();
            foreach (var item in WpSongs)
            {
                Prayers.Add(new Prayer
                {
                    Id = item.Id,
                    Title = item.Title.Rendered,
                    image = await GetFeaturedImage.featuredimage(Constants.Image_Url + item.FeaturedMedia.ToString()),
                    Date = item.Date,
                    Description = Gettext(item.Content.Rendered)
                });
            }
           
            Application.Current.Properties["allprayersjson"] = JsonConvert.SerializeObject(Prayers);
            await Application.Current.SavePropertiesAsync();
            Preferences.Set("Lastdatechecked", DateTime.Now.Date);
        }

        private string Gettext(string content)
        {

            var html = content;
            htmltext = new List<string>();
           var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            var htmlBody = htmlDoc.DocumentNode.SelectNodes("//p");

            foreach(var item in htmlBody)
            {
                htmltext.Add(item.InnerText);
            }

            var ans = string.Concat(htmltext);
            return ans;
        }


        public void Search(string Newtextvalue)
        {
          Prayers =  new  ObservableCollection<Prayer>(Prayers.Where(c => c.Title.ToUpper().Contains(Newtextvalue) || c.Title.ToLower().Contains(Newtextvalue)));
        }

        public void Checkwifionstart()
        {
            conn = CrossConnectivity.Current.IsConnected ? "online" : "offline";
        }
        public void Checkwificontinuously()
        {
            CrossConnectivity.Current.ConnectivityChanged += async (sender, args) =>
            {
                conn = args.IsConnected ? "online" : "offline";
                if(conn == "online" && !Preferences.ContainsKey("Lastdatechecked"))
                {
                    ActSeen = true;
                    await Setlastdatechecked();
                    ActSeen = false;
                }
            };
        }

    }
}

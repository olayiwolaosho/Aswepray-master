using GetAllBibleResponse;
using Newtonsoft.Json;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WePray.Services;
using WePray.Services.BibleServices;
using WePray.Services.RequestProviders;
using WePrayGetVersesandContent;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace WePray.ViewModels.PopupViewModel
{
    class BibleVersionViewModel : BaseViewModel
    {
        IBibleService bibleService;
        public ObservableCollection<GetAllBibleResponse.Datum> Bibleversions
        {
            get => bibleversions;
            set
            {
                SetProperty(ref bibleversions, value);
            }
        }
        private ObservableCollection<GetAllBibleResponse.Datum> bibleversions;

        public ObservableCollection<WePrayGetVersesandContent.ContentItem> Biblechaptersverses
        {
            get => biblechaptersverses;
            set
            {
                SetProperty(ref biblechaptersverses, value);
            }
        }
        private ObservableCollection<WePrayGetVersesandContent.ContentItem> biblechaptersverses;

        public BibleVersionViewModel()
        {
            bibleService = new BibleService(new RequestProvider());
            Checkwifionstart();
            Checkwificontinuously();
        }

        private bool actind;
        public bool Actind
        {
            get => actind;
            set => SetProperty(ref actind, value);
        } 
        
        private bool enable = true;
        public bool Enable
        {
            get => enable;
            set => SetProperty(ref enable, value);
        }

        private bool nointernet;
        public bool Nointernet
        {
            get => nointernet;
            set => SetProperty(ref nointernet, value);
        }

        string conn;

        public string Conn
        {
            get => conn;
            set => SetProperty(ref conn, value);
        }

        public async Task GetEnglishVersions()
        {
            try
            {
                if (!Preferences.ContainsKey("AllEngVersions"))
                {
                    Nointernet = false;
                    Actind = true;
                    var success = await bibleService.GetEnglishBibleVersions<AllBibleResponse>();
                    if (success != null)
                    {

                        Bibleversions = new ObservableCollection<GetAllBibleResponse.Datum>(success.Data);
                        Preferences.Set("AllEngVersions", JsonConvert.SerializeObject(success));
                        Actind = false;
                    }
                    else
                    {
                        Nointernet = true;
                         Actind = false;
                    }
                }
                else
                {
                    Nointernet = false;
                    Actind = true;
                    Bibleversions = new ObservableCollection<GetAllBibleResponse.Datum>(JsonConvert.DeserializeObject<AllBibleResponse>(Preferences.Get("AllEngVersions", null)).Data);
                    Actind = false;
                }
            }
            catch(Exception ex)
            {
                Actind = true;
                Debug.WriteLine(ex.Message);
            }
        }

        public async Task ChangeVersion(GetAllBibleResponse.Datum datum)
        {
            if(conn == "online")
            {
                try
                {
                    Enable = false;
                    Actind = true;
                    Nointernet = false;
                    Preferences.Set("Bversion", datum.AbbreviationLocal);
                    Preferences.Set("BibleID", datum.Id);
                    await getChapter();
                    MessagingCenter.Send(this, "ChangeChangeChapter");
                    MessagingCenter.Send(this, "ChangeVerse");
                }
                catch
                {
                    Nointernet = true;
                }
            }
            else
            {
                Nointernet = true;
            }
        }

        private async Task getChapter()
        {
            var Allverses = await bibleService.GetAllBibleBookChapterVerses<GetVersesandContent>();
            Application.Current.Properties["Allverses"] = JsonConvert.SerializeObject(Allverses);
            Seperatearrays(Allverses);
            await Application.Current.SavePropertiesAsync();
        }

        private void Seperatearrays(GetVersesandContent Allverses)
        {
            var bibleverses = new ObservableCollection<PurpleAttrs>();
            var bibleversescontent = new ObservableCollection<Content>();
            biblechaptersverses = new ObservableCollection<ContentItem>();
            foreach (var Citem in Allverses.Data.Content)
            {
                bibleversescontent.Add(Citem);
            }

            foreach (var CIitem in bibleversescontent.SelectMany(c => c.Items))
            {
                Biblechaptersverses.Add(CIitem);
            }
            Application.Current.Properties["AllverseContent"] = JsonConvert.SerializeObject(Biblechaptersverses);
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
                if (conn == "online")
                {
                    await GetEnglishVersions();
                }
            };
        }
    }
}


using GetaParticularBibleRequest;
using Newtonsoft.Json;
using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using WePray.Services;
using WePray.Services.BibleServices;
using WePray.Services.RequestProviders;
using WePrayAllBiblesBooksandChapters;
using WePrayGetVersesandContent;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace WePray.ViewModels.PopupViewModel
{
    class BibleRefPopupViewmodel : BaseViewModel
    {
        IBibleService bibleService;
        public ObservableCollection<WePrayAllBiblesBooksandChapters.Datum> Biblebooks
        {
            get => biblebooks;
            set
            {
              SetProperty(ref biblebooks, value);
            }
        }
        private ObservableCollection<WePrayAllBiblesBooksandChapters.Datum> biblebooks;    

        public ObservableCollection<WePrayAllBiblesBooksandChapters.Chapter> Biblebookchapters
        {
            get => biblebookchapters;
            set
            {
              SetProperty(ref biblebookchapters, value);
            }
        }
        private ObservableCollection<WePrayAllBiblesBooksandChapters.Chapter> biblebookchapters; 
        
        public ObservableCollection<WePrayGetVersesandContent.ContentItem> Biblechaptersverses
        {
            get => biblechaptersverses;
            set
            {
              SetProperty(ref biblechaptersverses, value);
            }
        }
        private ObservableCollection<WePrayGetVersesandContent.ContentItem> biblechaptersverses; 
        
        
        public ObservableCollection<WePrayGetVersesandContent.Content> Bibleversescontent
        {
            get => bibleversescontent;
            set
            {
              SetProperty(ref bibleversescontent, value);
            }
        }
        private ObservableCollection<WePrayGetVersesandContent.Content> bibleversescontent; 
        
        
        public ObservableCollection<WePrayGetVersesandContent.PurpleAttrs> Bibleverses
        {
            get => bibleverses;
            set
            {
              SetProperty(ref bibleverses, value);
            }
        }
        private ObservableCollection<WePrayGetVersesandContent.PurpleAttrs> bibleverses;


        public BibleRefPopupViewmodel()
        {
          
            bibleService = new BibleService(new RequestProvider());
            Checkwifionstart();
            Checkwificontinuously();

        }

        public async Task LoadBiblesChaptrersVerses()
        {
       
            await LoadBibles();
          
        }


        string conn;

        public string Conn
        {
            get => conn;
            set => SetProperty(ref conn, value);
        }

        bool loadingSign;
        public bool LoadingSign
        {
            get => loadingSign;
            set => SetProperty(ref loadingSign, value);
        }
        
        bool noNetwork;
        public bool NoNetwork
        {
            get => noNetwork;
            set => SetProperty(ref noNetwork, value);
        } 
        
        bool actChap;
        public bool ActChap
        {
            get => actChap;
            set => SetProperty(ref actChap, value);
        }

        bool chapNoNetwork;
        public bool ChapNoNetwork
        {
            get => chapNoNetwork;
            set => SetProperty(ref chapNoNetwork, value);
        }

        bool loadingVerseSign;
        public bool LoadingVerseSign
        {
            get => loadingVerseSign;
            set => SetProperty(ref loadingVerseSign, value);
        }


        bool chaptersStack = false;
        public bool ChaptersStack
        {
            get => chaptersStack;
            set => SetProperty(ref chaptersStack, value);
        }

        bool versesStack = false;
        public bool VersesStack
        {
            get => versesStack;
            set => SetProperty(ref versesStack, value);
        }

     
        bool booksStack = true;
        public bool BooksStack
        {
            get => booksStack;
            set => SetProperty(ref booksStack, value);
        } 
        
        bool noBookPicked = !Preferences.ContainsKey("BookPicked");
        public bool NoBookPicked
        {
            get => noBookPicked;
            set => SetProperty(ref noBookPicked, value);
        }


        public ICommand LoadItemsCommand => new Command(() => IsBusy = false);

        // Swipe gesture event
        public ICommand SwipeCommand => new Command((object obj) =>
        {
            var direction = (string)obj;

            switch (direction)
            {
                case "Left":
                    BooksStack = false;
                    VersesStack = false;
                    ChaptersStack = true;
                    break;
                case "Right":
                    BooksStack = false;
                    VersesStack = false;
                    ChaptersStack = true;
                    break;
                case "ChapterLeft":
                    ChaptersStack = false;
                    BooksStack = false;
                    VersesStack = true;
                    break;
                case "ChapterRight":
                    ChaptersStack = false;
                    VersesStack = false;
                    BooksStack = true;
                    break;
            }
        });

        public ICommand OnSelectTaps => new Command((object obj) =>
        {
            var parameter = (string)obj;

            switch (parameter)
            {
                case "books":
                    ChaptersStack = false;
                    VersesStack = false;
                    BooksStack = true;
                    break;
                case "chapters":
                    BooksStack = false;
                    VersesStack = false;
                    ChaptersStack = true;
                    break;
                case "verses":
                    ChaptersStack = false;
                    BooksStack = false;
                    VersesStack = true;
                    break;
            }
        });

        private async Task LoadBibles()
        {
            try
            {

                if (Application.Current.Properties.ContainsKey("Biblebooks"))
                {
                    LoadingSign = true;
                    var Allbooks = JsonConvert.DeserializeObject<AllBiblesBooksandChapters>(Application.Current.Properties["Biblebooks"].ToString());
                    Biblebooks = new ObservableCollection<WePrayAllBiblesBooksandChapters.Datum>(Allbooks.Data);
                    LoadingSign = false;
                }
                else
                {
                    LoadingSign = true;
                    var Allbooks = await bibleService.GetAllBibleBookChapters<AllBiblesBooksandChapters>();
                    Application.Current.Properties["Biblebooks"] = JsonConvert.SerializeObject(Allbooks);
                    await Application.Current.SavePropertiesAsync();
                    Biblebooks = new ObservableCollection<WePrayAllBiblesBooksandChapters.Datum>(Allbooks.Data);
                    LoadingSign = false;
                }

            }
            catch
            {
                LoadingSign = false;
                NoNetwork = true;
            }

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
                if (conn == "online" && !Preferences.ContainsKey("Lastdatechecked"))
                {
                    NoNetwork = false;
                    await LoadBibles();
                }
            };
        }

        public void PicBook(object obj)
        {
            var parameter = (WePrayAllBiblesBooksandChapters.Datum)obj;
            Preferences.Set("BookPicked", parameter.Name);
            Preferences.Set("BookPickedID", parameter.Id);
            Biblebookchapters = new ObservableCollection<WePrayAllBiblesBooksandChapters.Chapter>(biblebooks.Where(c => c.Name == parameter.Name).Select(a => a.Chapters).FirstOrDefault());
            BooksStack = false;
            VersesStack = false;
            ChaptersStack = true;
        } 
        
        public async Task PicChapter(object obj)
        {
            if (CrossConnectivity.Current.IsConnected)
            {
                ChapNoNetwork = false;
                try
                {
                    LoadingVerseSign = true;
                    var parameter = (WePrayAllBiblesBooksandChapters.Chapter)obj;
                    ChaptersStack = false;
                    VersesStack = true;
                    ActChap = true;
                    Preferences.Set("ChapterPicked", parameter.Number);
                    var Allverses = await bibleService.GetAllBibleBookChapterVerses<GetVersesandContent>();
                    Application.Current.Properties["Allverses"] = JsonConvert.SerializeObject(Allverses);
                    Seperatearrays(Allverses);
                    await Application.Current.SavePropertiesAsync();
                    Bibleverses = new ObservableCollection<WePrayGetVersesandContent.PurpleAttrs>(Biblechaptersverses.Select(c => c.Attrs).Where(a => a.Number != null));
                    BooksStack = false; 
                    ActChap = false;
                    LoadingVerseSign = false;
                }
                catch (Exception ex)
                {
                    System.Diagnostics.Debug.WriteLine(ex.Message);
                }
            }
            else
            {
                ChapNoNetwork = true;
            }
           
        }

        private void Seperatearrays(GetVersesandContent Allverses)
        {
            bibleverses = new ObservableCollection<PurpleAttrs>();
            bibleversescontent = new ObservableCollection<Content>();
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

      
    }
}

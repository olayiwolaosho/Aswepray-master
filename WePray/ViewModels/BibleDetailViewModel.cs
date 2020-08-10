using Newtonsoft.Json;
using Plugin.Connectivity;
using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Windows.Input;
using WePray.Services;
using WePray.Services.BibleServices;
using WePray.Services.RequestProviders;
using WePray.ViewModels.Base;
using WePray.ViewModels.PopupViewModel;
using WePray.Views.Popups;
using WePrayGetVersesandContent;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace WePray.ViewModels
{
    class BibleDetailViewModel : BaseViewModel
    {
        IBibleService bibleService;
        int number;
        public ObservableCollection<WePrayGetVersesandContent.ContentItem> Biblechaptersverses
        {
            get => biblechaptersverses;
            set
            {
                SetProperty(ref biblechaptersverses, value);
            }
        }
        private ObservableCollection<WePrayGetVersesandContent.ContentItem> biblechaptersverses;

        public BibleDetailViewModel()
        {
            CreateContent();
            Curbible = Preferences.Get("BookChapter", null);
            BVersion = Preferences.Get("Bversion", null);
            bibleService = new BibleService(new RequestProvider());

        }
        string bible;
        public string Bible
        {
            get => bible;
            set => SetProperty(ref bible, value);
        }
        
        string bVersion;
        public string BVersion
        {
            get => bVersion;
            set => SetProperty(ref bVersion, value);
        }

        string curbible;
        public string Curbible
        {
            get => curbible;
            set => SetProperty(ref curbible, value);
        }

        FormattedString format;

        public FormattedString Format
        {
            get => format;
            set
            {
                SetProperty(ref format, value);

            }

        }

        public ICommand PopBible => new Command(async() =>
        {
            await Application.Current.MainPage.Navigation.PushPopupAsync(new BibleReferencePopup());
        }); 
        
        public ICommand PopBibleVersion => new Command(async() =>
        {
            if(CrossConnectivity.Current.IsConnected)
            {
                await Application.Current.MainPage.Navigation.PushPopupAsync(new ChangeBibleVersion());
            }
            else
            {
                await Application.Current.MainPage.DisplayAlert("", "Please Connect to the internet to change bible version", "OK");
            }
        });

        
        private void CreateContent()
        {
            if (Application.Current.Properties.ContainsKey("AllverseContent"))
            {
                Biblechaptersverses = new ObservableCollection<WePrayGetVersesandContent.ContentItem>(JsonConvert.DeserializeObject<IEnumerable<WePrayGetVersesandContent.ContentItem>>(Application.Current.Properties["AllverseContent"].ToString()));
                FormatContent();
            }
        }

        private void FormatContent()
        {
            var formattedString = new FormattedString();
            foreach (var item in Biblechaptersverses)
            {
                if(item.Items == null)
                {
                    formattedString.Spans.Add(new Span { Text = item.Text });
                }
                else
                {
                    if(Int32.TryParse(item.Items.Select(c => c.Text).FirstOrDefault(), out number))
                    {
                        formattedString.Spans.Add(new Span { Text = Environment.NewLine });
                        formattedString.Spans.Add(new Span { Text = item.Items.Select(c => c.Text).FirstOrDefault() + "." + " ", FontSize = 14});
                        formattedString.Spans.Add(new Span { Text = Environment.NewLine });
                    }
                    else
                    {
                        formattedString.Spans.Add(new Span { Text = item.Items.Select(c => c.Text).FirstOrDefault() });
                    }
                       
                }
            }

            Format = formattedString;
        }

       
    }
}

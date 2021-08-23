using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WePray.DependencyInject;
using WePray.ViewModels.PopupViewModel;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WePray.ViewModels.Base
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BibleReferencePopup : Rg.Plugins.Popup.Pages.PopupPage
    {
        BibleRefPopupViewmodel BRPV;
        public BibleReferencePopup()
        {
            InitializeComponent();
            BindingContext = BRPV = Resolver.Resolve<BibleRefPopupViewmodel>(); 
        }

        protected async override void OnAppearing()
        {
           await BRPV.LoadBiblesChaptrersVerses(); 
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var layout = (BindableObject)sender;
            var parameter = (WePrayAllBiblesBooksandChapters.Datum)layout.BindingContext;
            BRPV.PicBook(parameter);
        }

        private async void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            var layout = (BindableObject)sender;
            var parameter = (WePrayAllBiblesBooksandChapters.Chapter)layout.BindingContext;
            await BRPV.PicChapter(parameter);
        }

        private async void TapGestureRecognizer_Tapped_2(object sender, EventArgs e)
        {
           Preferences.Set("CurrentlyReading", true);
            MessagingCenter.Send(this, "LoadBible");
            await Navigation.PopAllPopupAsync();
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            BibleCollecitonView.ItemsSource = BRPV.Biblebooks.Where(c => c.Name.ToUpper().Contains(e.NewTextValue) || c.Name.ToLower().Contains(e.NewTextValue));

        }

        private void ONSwiped(object sender, SwipedEventArgs e)
        {
            var layout = e.Parameter;
            var parameter = (string)layout;
                //.CommandParameter;

            switch (parameter)
            {
                case "Left":
                    BRPV.BooksStack = false;
                    BRPV.VersesStack = false;
                    BRPV.ChaptersStack = true;
                    break;
                case "Right":
                    BRPV.BooksStack = false;
                    BRPV.VersesStack = false;
                    BRPV.ChaptersStack = true;
                    break;
                case "ChapterLeft":
                    BRPV.ChaptersStack = false;
                    BRPV.BooksStack = false;
                    BRPV.VersesStack = true;
                    break;
                case "ChapterRight":
                    BRPV.ChaptersStack = false;
                    BRPV.VersesStack = false;
                    BRPV.BooksStack = true;
                    break;
            }
        }

    }
}
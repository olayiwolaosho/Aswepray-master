using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WePray.ViewModels.Base;
using WePray.ViewModels.PopupViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WePray.Views.Popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class ChangeBibleVersion : Rg.Plugins.Popup.Pages.PopupPage
    {
        BibleVersionViewModel BVVM;
        public ChangeBibleVersion()
        {
            InitializeComponent();
            BindingContext = BVVM = new BibleVersionViewModel();
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await BVVM.GetEnglishVersions(); 
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            try
            {
                var layout = (BindableObject)sender;
                var parameter = (GetAllBibleResponse.Datum)layout.BindingContext;
                await BVVM.ChangeVersion(parameter);
                MessagingCenter.Send<BibleReferencePopup>(new BibleReferencePopup(), "LoadBible");
                BVVM.Actind = false;
                BVVM.Enable = true;
                await Navigation.PopPopupAsync();
            }
            catch
            {

            }
            
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            BibleCollecitonView.ItemsSource = BVVM.Bibleversions.Where(c => c.AbbreviationLocal.ToUpper().Contains(e.NewTextValue) || c.AbbreviationLocal.ToLower().Contains(e.NewTextValue)|| c.Name.ToUpper().Contains(e.NewTextValue) || c.Name.ToLower().Contains(e.NewTextValue) );

        }
    }
}
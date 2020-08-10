using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WePray.Models;
using WePray.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WePray.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DailyPrayer : ContentPage
    {
        DailyPrayerViewModel DPVM;
        public DailyPrayer()
        {
            InitializeComponent();
            BindingContext = DPVM =new DailyPrayerViewModel(this.Navigation);
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await DPVM.GetAllPrayers();
        }

        private void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
           
        }

        private async void TapGestureRecognizer_Tapped_1(object sender, EventArgs e)
        {
            Routing.RegisterRoute(nameof(PrayerDetail), typeof(PrayerDetail));
            var layout = (BindableObject)sender;
            var item = (Prayer)layout.BindingContext;
            var jsn = JsonConvert.SerializeObject(item);
            Preferences.Set("jsnstr", jsn);
            await Shell.Current.GoToAsync($"{nameof(PrayerDetail)}");
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
            ItemsCollectionView.ItemsSource = DPVM.Prayers.Where(c => c.Title.ToUpper().Contains(e.NewTextValue) || c.Title.ToLower().Contains(e.NewTextValue));

        }
    }
}
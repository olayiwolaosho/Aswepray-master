 using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WePray.DependencyInject;
using WePray.Models;
using WePray.ViewModels;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WePray.Views
{
    /// <summary>
    /// This is actuall daily devotionals what was i thinking when i named it 
    /// Page that shows all devotionals
    /// </summary>
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class DailyPrayer : ContentPage
    {
        DailyPrayerViewModel DPVM;

        bool shouldload = false;

        public DailyPrayer()
        {
            InitializeComponent();
            shouldload = true;
            BindingContext = DPVM = Resolver.Resolve<DailyPrayerViewModel>();
        }

        /// <summary>
        /// Get all Devotionals when the content page appears to reduce the amonut of work being don on page creation this helps the smoothness app flow 
        /// </summary>
        protected async override void OnAppearing()
        {
            base.OnAppearing();
            if (shouldload)
            {
                shouldload = false;
                await DPVM.GetAllPrayers();
            }
            
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
            await Shell.Current.GoToAsync($"{nameof(PrayerDetail)}?pageTitle=Prayer");
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e)
        {
           // ItemsCollectionView.ItemsSource = DPVM.Prayers.Where(c => c.Title.ToUpper().Contains(e.NewTextValue) || c.Title.ToLower().Contains(e.NewTextValue));
        }
    }
}
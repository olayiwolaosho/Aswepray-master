using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WePray.Enums;
using WePray.Models;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WePray.Views.Controls
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class HomePageControl : ContentView
    {

        public static readonly BindableProperty DailyPageTypeProperty = BindableProperty.Create("DailyPageType", typeof(DailyType), typeof(HomePageControl), DailyType.DailyPrayer);


        public static readonly BindableProperty ViewItemSourceProperty = BindableProperty.Create("ViewItemSource", typeof(ObservableCollection<Prayer>), typeof(HomePageControl), new ObservableCollection<Prayer>(), BindingMode.TwoWay);



        public DailyType DailyPageType
        {
            get { return (DailyType)GetValue(DailyPageTypeProperty); }
            set { SetValue(DailyPageTypeProperty, value); }
        }


        public ObservableCollection<Prayer> ViewItemSource
        {
            get { return (ObservableCollection<Prayer>)GetValue(ViewItemSourceProperty); }
            set { SetValue(ViewItemSourceProperty, value); }
        }


        public HomePageControl()
        {
            InitializeComponent();
        }



        private async void NavigateToDetailView(object sender, EventArgs e)
        {
            Routing.RegisterRoute(nameof(PrayerDetail), typeof(PrayerDetail));
            var layout = (BindableObject)sender;
            var item = (Prayer)layout.BindingContext;
            var jsn = JsonConvert.SerializeObject(item);
            Preferences.Set("jsnstr", jsn);
            await Shell.Current.GoToAsync($"{nameof(PrayerDetail)}");
        }
    }
}
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
    public partial class MusicPage : ContentPage
    {
        MusicViewModel MVM;
        public MusicPage()
        {
            InitializeComponent();

            BindingContext = MVM = new MusicViewModel(this.Navigation);
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
            await MVM.GetAllSongs();
        }

        private async void TapGestureRecognizer_Tapped(object sender, EventArgs e)
        {
            var layout = (BindableObject)sender;
            var item = (Music)layout.BindingContext;
            var jsn = JsonConvert.SerializeObject(item);
            Preferences.Set("musicjsnstr", jsn);
            await Shell.Current.GoToAsync($"{nameof(MusicDetail)}");
        }
    }
}
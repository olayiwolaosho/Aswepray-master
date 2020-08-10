using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WePray.Models;
using WePray.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WePray.Views
{
   
    [XamlCompilation(XamlCompilationOptions.Compile)]
    
    public partial class PrayerDetail : ContentPage
    {
        PrayerDetailViewModel PDVM; 
        public PrayerDetail()
        {
            InitializeComponent();
            BindingContext = PDVM = new PrayerDetailViewModel();
        }

        protected override bool OnBackButtonPressed()
        {
            PDVM.stopAudio();
            return base.OnBackButtonPressed();
        }
    }
}
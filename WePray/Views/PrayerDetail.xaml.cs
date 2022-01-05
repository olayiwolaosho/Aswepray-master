using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WePray.DependencyInject;
using WePray.Models;
using WePray.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WePray.Views
{
   
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [QueryProperty(nameof(pageTitle), "pageTitle")]
    public partial class PrayerDetail : ContentPage
    {
        PrayerDetailViewModel PDVM;

        public string pageTitle
        {
            set
            {
                SetPageTitle(value);
            }
        }



        public PrayerDetail()
        {
            InitializeComponent();
            BindingContext = PDVM = Resolver.Resolve<PrayerDetailViewModel>();
        }


        protected override bool OnBackButtonPressed()
        {
            PDVM.stopAudio();
            return base.OnBackButtonPressed();
        }


        public void SetPageTitle(string pageTitle)
		{
            detailsPage.Title = pageTitle;
        }
    }
}
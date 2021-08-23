using Rg.Plugins.Popup.Extensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WePray.DependencyInject;
using WePray.ViewModels;
using WePray.ViewModels.Base;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WePray.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BibleDetailPage : ContentPage
    {
        BibleDetailViewModel BDVM;
        public BibleDetailPage()
        {
            InitializeComponent();
            BindingContext = BDVM = Resolver.Resolve<BibleDetailViewModel>();

            MessagingCenter.Send(this, "LoadBible");
            MessagingCenter.Subscribe<BibleReferencePopup>(this, "LoadBible", (sender) =>
            {
                BindingContext = BDVM = Resolver.Resolve<BibleDetailViewModel>();
                Blabel.FormattedText = BDVM.Format;
            });
        }

        protected async override void OnAppearing()
        {
            base.OnAppearing();
               var page = Preferences.Get("CurrentlyReading", false);
            switch (page)
            {
                case false:
                   await Navigation.PushPopupAsync(new BibleReferencePopup());
                    break;
                case true:
                    BindingContext = BDVM = Resolver.Resolve<BibleDetailViewModel>();
                    Blabel.FormattedText = BDVM.Format;
                    break;
            }
        }
    }
}
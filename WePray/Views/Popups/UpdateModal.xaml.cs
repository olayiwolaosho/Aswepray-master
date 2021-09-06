using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WePray.Views.Popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class UpdateModal : Rg.Plugins.Popup.Pages.PopupPage
    {
        public UpdateModal()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Times button clicked will pop update modal page
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void CancleClicked(object sender, EventArgs e)
        {
            await Rg.Plugins.Popup.Services.PopupNavigation.Instance.PopAsync();
        }


        /// <summary>
        /// Navigates to playstore
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void UpdateClicked(object sender, EventArgs e)
        {
            string url = string.Empty;

            var location = RegionInfo.CurrentRegion.Name.ToLower();

            if (Device.RuntimePlatform == Device.Android)
                url = "https://play.google.com/store/apps/details?id=com.companyname.wepray";

            else if (Device.RuntimePlatform == Device.iOS)
                url = "https://itunes.apple.com/" + location + "/app/contractor-action-solution/id1039202852?mt=8";

            await Browser.OpenAsync(url, BrowserLaunchMode.External);
        }
    }
}
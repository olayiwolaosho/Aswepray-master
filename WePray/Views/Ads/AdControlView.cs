using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Xamarin.Forms;

namespace WePray.Views.Ads
{
    public class AdControlView : View
    {
      
         public enum Sizes { Standardbanner, LargeBanner, MediumRectangle, FullBanner, Leaderboard, SmartBannerPortrait }
        public Sizes Size { get; set; }
        public AdControlView()
        {
          //  this.BackgroundColor = Color.Accent;    
        }

     }
}
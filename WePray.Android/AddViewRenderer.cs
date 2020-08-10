using Android.Content;
using Android.Gms.Ads;
using Android.Widget;
using WePray.Droid;
using WePray.Views.Ads;
using Xamarin.Forms;
using Xamarin.Forms.Platform.Android;

[assembly: ExportRenderer(typeof(AdControlView), typeof(AddViewRenderer))]
namespace WePray.Droid
{
    class AddViewRenderer : ViewRenderer
    {
        Context context;
        public AddViewRenderer(Context _context) : base(_context)
        {
            context = _context;
        }
        protected override void OnElementChanged(ElementChangedEventArgs<View> e)
        {
            base.OnElementChanged(e);
            if (e.OldElement == null)
            {
                var adView = new AdView(Context);
                switch ((Element as AdControlView).Size)
                {
                    case AdControlView.Sizes.Standardbanner:
                        adView.AdSize = AdSize.Banner;
                        break;
                    case AdControlView.Sizes.LargeBanner:
                        adView.AdSize = AdSize.LargeBanner;
                        break;
                    case AdControlView.Sizes.MediumRectangle:
                        adView.AdSize = AdSize.MediumRectangle;
                        break;
                    case AdControlView.Sizes.FullBanner:
                        adView.AdSize = AdSize.FullBanner;
                        break;
                    case AdControlView.Sizes.Leaderboard:
                        adView.AdSize = AdSize.Leaderboard;
                        break;
                    case AdControlView.Sizes.SmartBannerPortrait:
                        adView.AdSize = AdSize.SmartBanner;
                        break;
                    default:
                        adView.AdSize = AdSize.Banner;
                        break;
                }
                // TODO: change this id to your admob id  
                adView.AdUnitId = "ca-app-pub-6179024394686042/7038423825";
                var requestbuilder = new AdRequest.Builder();
                adView.LoadAd(requestbuilder.Build());
                SetNativeControl(adView);
            }
        }
    }
}  
  
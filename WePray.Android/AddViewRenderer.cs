using Android.Content;
using Android.Gms.Ads;
using Android.Graphics;
using Android.OS;
using Android.Runtime;
using Android.Util;
using Android.Views;
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
        protected override void OnElementChanged(ElementChangedEventArgs<Xamarin.Forms.View> e)
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
                        adView.AdSize = NewSmartBanner();
                        break;
                    default:
                        adView.AdSize = AdSize.Banner;
                        break;
                }
                // TODO: change this id to your admob id  
                adView.AdUnitId = "ca-app-pub-6179024394686042/7038423825";

                //This is for testads
                //adView.AdUnitId = "ca-app-pub-3940256099942544/6300978111";
                var requestbuilder = new AdRequest.Builder();
                adView.LoadAd(requestbuilder.Build());
                SetNativeControl(adView);
            }
        }

        private AdSize NewSmartBanner()
        {
            Android.Views.IWindowManager windowManager = Context.GetSystemService(Context.WindowService).JavaCast<Android.Views.IWindowManager>();

            int widthPixels = getScreenWidth(windowManager);

            return AdSize.GetCurrentOrientationAnchoredAdaptiveBannerAdSize(Context, widthPixels);
        }

        public static int getScreenWidth(IWindowManager windowManager)
        {
            if (Build.VERSION.SdkInt >= Android.OS.BuildVersionCodes.R)
            {
                WindowMetrics windowMetrics = windowManager.CurrentWindowMetrics;
                Insets insets = windowMetrics.WindowInsets
                        .GetInsetsIgnoringVisibility(WindowInsets.Type.SystemBars());
                return windowMetrics.Bounds.Width() - insets.Left - insets.Right;
            }
            else
            {
                DisplayMetrics displayMetrics = new DisplayMetrics();
#pragma warning disable CS0618 // Type or member is obsolete
                windowManager.DefaultDisplay.GetMetrics(displayMetrics);
#pragma warning restore CS0618 // Type or member is obsolete
                float density = displayMetrics.Density;
                return (int)(displayMetrics.WidthPixels / density);
            }
        }
    }
}  
  
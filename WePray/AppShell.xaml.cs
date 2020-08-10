using System;
using System.Collections.Generic;
using WePray.ViewModels.Base;
using WePray.Views;
using Xamarin.Forms;

namespace WePray
{
    public partial class AppShell : Xamarin.Forms.Shell
    {
        public AppShell()
        {
            InitializeComponent();
            Routing.RegisterRoute(nameof(MusicDetail), typeof(MusicDetail));
            Routing.RegisterRoute(nameof(BibleDetailPage), typeof(BibleDetailPage));
        }
    }
}

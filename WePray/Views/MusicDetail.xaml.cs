using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WePray.DependencyInject;
using WePray.ViewModels;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace WePray.Views
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class MusicDetail : ContentPage
    {
        public MusicDetail()
        {
            InitializeComponent();
            BindingContext =  Resolver.Resolve<MusicDetailViewModel>();
        }
    }
}
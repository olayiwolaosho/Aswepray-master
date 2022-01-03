using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using WePray.AllConstants;
using WePray.Models;
using WePray.Repository;
using WePray.Services.Connection;
using WePray.Services.WordPressServices;
using WePrayWPResponseObject;
using WordPressPCL;
using Xamarin.Forms;

namespace WePray.ViewModels
{
    class MusicViewModel : BaseViewModel
    {

        ObservableCollection<Music> songs;
        public ObservableCollection<Music> Songs { get => songs; set => SetProperty(ref songs,value); }    
        
        ObservableCollection<WPResponseObject> wpsongs;
        public ObservableCollection<WPResponseObject> WpSongs { get => wpsongs; set => SetProperty(ref wpsongs,value); }
        INavigation navigation;
        IWPServices wPServices;

        public MusicViewModel(IConnection connection, IRepository repository) : base(connection, repository)
        {
            wPServices = new WPServices();
           
        }

        public async Task GetAllSongs()
        {
            Songs = new ObservableCollection<Music>();
            WpSongs = new ObservableCollection<WPResponseObject>(await wPServices.GetAllSongs<WPResponseObject>());

            foreach(var item in WpSongs)
            {
                Songs.Add(new Music
                {
                    Id = item.Id,
                    Songtitle = item.Title.Rendered,
                    image = await GetFeaturedImage.featuredimage(Constants.Image_Url + item.FeaturedMedia.ToString()),
                    Song = Getsong(item.Content.Rendered)
                });
            }

        }

        //we use this to get all songs
        private string Getsong(string content)
        {

            var html = content;

            var htmlDoc = new HtmlDocument();
            htmlDoc.LoadHtml(html);

            var htmlBody = htmlDoc.DocumentNode.SelectSingleNode("//a");

            return htmlBody.InnerHtml;
        }
    }
}

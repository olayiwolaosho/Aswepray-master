using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using WePray.Dependencies;
using WePray.Models;
using WePray.Repository;
using WePray.Services.Connection;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace WePray.ViewModels
{
    class MusicDetailViewModel : BaseViewModel
    {
        string content;
        public MusicDetailViewModel(IConnection connection, IRepository repository) : base(connection, repository)
        {
            if (Preferences.ContainsKey("musicjsnstr"))
            {
                content = Preferences.Get("musicjsnstr", null);
                PerformOperation(content);
            }
        }

        string artist = "";

        public string Artist
        {
            get => artist;
            set
            {
                SetProperty(ref artist, value);

            }

        }

        string image = "";

        public string Image
        {
            get => image;
            set
            {
                SetProperty(ref image, value);

            }

        } 
        
        string songtitle = "";

        public string Songtitle
        {
            get => songtitle;
            set
            {
                SetProperty(ref songtitle, value);

            }

        } 
        
        string song = "";

        public string Song
        {
            get => song;
            set
            {
                SetProperty(ref song, value);

            }

        }

        bool isplaying = false;

        public bool isPlaying
        {
            get => isplaying;
            set
            {
                SetProperty(ref isplaying, value);

            }

        }

        bool ispaused = true;

        public bool isPaused
        {
            get => ispaused;
            set
            {
                SetProperty(ref ispaused, value);

            }

        }

        bool indicate;

        public bool Indicate
        {
            get => indicate;
            set
            {
                SetProperty(ref indicate, value);

            }

        }
        FormattedString format;

        public FormattedString Format
        {
            get => format;
            set
            {
                SetProperty(ref format, value);

            }

        }

        public ICommand play => new Command(() =>
        {
            DependencyService.Get<IAudio>().Play_Pause(Song);
        });

        private void PerformOperation(string jsnstr)
        {
            var newcontent = JsonConvert.DeserializeObject<Music>(jsnstr);
            artist = newcontent.Artist;
            songtitle = newcontent.Songtitle;
            Image = newcontent.image;
            Song = newcontent.Song;
        }

    }
}

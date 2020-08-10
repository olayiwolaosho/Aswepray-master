using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Android.App;
using Android.Content;
using Android.Media;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using WePray.Dependencies;
using WePray.Droid;
using Xamarin.Forms;

[assembly: Dependency(typeof(AudioSerivce))]
namespace WePray.Droid
{
    class AudioSerivce : IAudio
    {
        int clicks = 0;
        MediaPlayer player;

        public AudioSerivce()
        {
        }

        public async Task<bool> Play_Pause(string url)
        {
            if (clicks == 0)
            {
                this.player = new MediaPlayer();
                this.player.SetDataSource(url);
                var attribs = new AudioAttributes.Builder().SetFlags(AudioFlags.None).SetLegacyStreamType(Stream.Music).Build();
                this.player.SetAudioAttributes(attribs);
                //  this.player.SetAudioStreamType(Stream.Music);
                await Task.Run(() => {
                    this.player.PrepareAsync();
                    this.player.Start();
                    clicks++;
                }).ConfigureAwait(false); 
               
            }
            else if (clicks % 2 != 0)
            {
                this.player.Pause();
                clicks++;

            }
            else
            {
                this.player.Start();
                clicks++;
            }


            return true;
        }

        public bool Stop(bool val)
        {
            this.player.Stop();
            clicks = 0;
            return true;
        }
    }
}
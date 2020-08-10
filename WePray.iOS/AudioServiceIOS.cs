using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AVFoundation;
using Foundation;
using UIKit;
using WePray.Dependencies;
using WePray.iOS;
using Xamarin.Forms;

[assembly: Dependency(typeof(AudioServiceIOS))]

namespace WePray.iOS
{
    class AudioServiceIOS : IAudio
    {
        int clicks = 0;

        public AudioServiceIOS() { }
        AVPlayer player;
        public async Task<bool> Play_Pause(string url)
        {
            if (clicks == 0)
            {
                //this.player1 = new AVAudioPlayer(NSUrl.FromString(url), "mp3", out err);
                this.player = new AVPlayer();
                this.player = AVPlayer.FromUrl(NSUrl.FromString(url));
                this.player.Play();
                clicks++;
            }
            else if (clicks % 2 != 0)
            {
                this.player.Pause();
                clicks++;

            }
            else
            {
                this.player.Play();
                clicks++;
            }
            return true;

        }

        public bool Stop(bool val)
        {
            if (player != null)
            {
                player.Dispose();
                clicks = 0;
            }
            return true;
        }
    }
}
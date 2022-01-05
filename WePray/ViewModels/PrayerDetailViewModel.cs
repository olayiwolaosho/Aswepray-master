using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using WePray.Models;
using WePray.Repository;
using WePray.Services.Connection;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace WePray.ViewModels
{

    public class PrayerDetailViewModel : BaseViewModel
    {
        CancellationTokenSource cts;
        string[] strList;
        string content;
        int a = 0;



        public PrayerDetailViewModel(IConnection connection, IRepository repository) : base(connection, repository)
        {
            if (Preferences.ContainsKey("jsnstr"))
            {
                content = Preferences.Get("jsnstr", null);
                PerformOperation(content);
            }
        }



        string title = "";

        public new string Title
        {
            get => title;
            set
            {
                SetProperty(ref title, value);

            }

        }

        string description = "";

        public string Description
        {
            get => description;
            set
            {
                SetProperty(ref description, value);

            }

        }

        string strdate = "";

        public string strDate
        {
            get => strdate;
            set
            {
                SetProperty(ref strdate, value);

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

        public ICommand navback => new Command(() =>
        {
            stopAudio();
            Application.Current.MainPage.Navigation.PopAsync();
        });
        

        private void PerformOperation(string jsnstr)
        {
            var newcontent = JsonConvert.DeserializeObject<Prayer>(jsnstr);
            title = newcontent.Title;
            Description = newcontent.Description;
            strdate = newcontent.strDate;
            image = newcontent.image;
        }

        public ICommand play => new Command(async() =>
        {
            try
            {
                Indicate = true;
                init();

                isPlaying = true;
                isPaused = false;

                Preferences.Set("isPlaying", isPlaying);

                var settings = new SpeechOptions()
                {
                    Volume = .65f,
                    Pitch = 0.1f
                };


                cts = new CancellationTokenSource();
                await TextToSpeech.SpeakAsync(Title, cancelToken: cts.Token);
                await highlighttext();
            }
            catch
            {
                Indicate = false;
            }
        });

        //This method is to seperate all the description texts
        private void init()
        {
            string str = ".";
            char character = char.Parse(str);

            string str2 = ",";
            char character2 = char.Parse(str2);
            
            string str3 = ">";
            char character3 = char.Parse(str3);

            strList = Description.Split(new char[] { character, character2, character3 });

        } 
        
        private void Checkisplaying()
        {
            if (Preferences.ContainsKey("isPlaying"))
            {
              var changeAudio = Preferences.Get("isPlaying",false);

                switch (changeAudio)
                {
                    case true:
                        stopAudio();
                        break;

                }
               
            }
        }

        //This method is for the formatted text highlight
        private async Task highlighttext()
        {
            for (int i = a; i < strList.Length ; i++,a++)
            {
                string content = strList[i];

                var formattedString = new FormattedString();

                for (int j = 0; j < strList.Length; j++)
                {

                    if (i == j)
                    {
                        formattedString.Spans.Add(new Span { Text = strList[j] , ForegroundColor = Color.Black, BackgroundColor = Color.LightGray });
                    }
                    else
                    {
                        formattedString.Spans.Add(new Span { Text = strList[j], ForegroundColor = Color.Black, });
                    }


                }

                Format = formattedString;

                //Using a bool varibale we can pause the TTS fucntion, when press back button set the value of StopTTS to true.
                //When loading this set the value back to false.
                if (isplaying && i != strList.Length - 1)
                {
                    Indicate = false;
                    await TextToSpeech.SpeakAsync(content,cancelToken: cts.Token);
                }
            }
            stopAudio();
          
            a = 0;
        }

        public ICommand Pause => new Command(() =>
        {
            stopAudio();
        }); 
        
        public void stopAudio()
        {
            if (cts?.IsCancellationRequested ?? true)
                return;
            isPlaying = false;
            Preferences.Set("isPlaying", isPlaying);
            cts.Cancel();
            isPaused = true;
            PerformOperation(content);
        }

        public ICommand Share => new Command(async() =>
        {                                                                                                            
            string uri = "https://bible.com/reading-plans/13685/day/2?segment=0";

            await Xamarin.Essentials.Share.RequestAsync(new ShareTextRequest
            {
                Text = "EveryDay Prayers",
                Uri = uri,
            });
        });
    }
}

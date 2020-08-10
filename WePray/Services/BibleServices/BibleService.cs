using GetAllBibleResponse;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WePray.AllConstants;
using WePray.Services.BibleServices;
using WePray.Services.RequestProviders;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace WePray.Services
{
    public class BibleService : IBibleService
    {
        IRequestProvider requestProvider;
        public BibleService(IRequestProvider requestProvider)
        {
            this.requestProvider = requestProvider; 
        }

        public async Task<TResult> GetAllBibleBookChapters<TResult>()
        {
            TResult allbibles = await requestProvider.GetAsync<TResult>(Constants.AllBiblesVersesandChapters, Constants.Bible_app_key);
            return allbibles;
        }

        public async Task<TResult> GetAllBibleBookChapterVerses<TResult>()
        {
           var OneBible = Constants.Base_Url + "bibles/" +  Preferences.Get("BibleID", null);
            var BookChapter = Preferences.Get("BookPickedID", null) + "." + Preferences.Get("ChapterPicked", null);
            var endurl = "?content-type=json&include-notes=false&include-titles=false&include-chapter-numbers=false&include-verse-numbers=true&include-verse-spans=false";
          string AllBiblesVersesandContent = OneBible + "/chapters/" + $"{BookChapter}" + endurl;
           TResult allbibles = await requestProvider.GetAsync<TResult>(AllBiblesVersesandContent, Constants.Bible_app_key);
           Preferences.Set("BookChapter", BookChapter);
            return allbibles;
        }

        public Task<TResult> GetAllBibleBookChapterVerseScriptures<TResult>()
        {
            throw new NotImplementedException();
        }

        public Task<TResult> GetAllBibleBooks<TResult>()
        {
            throw new NotImplementedException();
        } 
        
        public async Task<TResult> GetEnglishBibleVersions<TResult>()
        {
            TResult allbibles = await requestProvider.GetAsync<TResult>(Constants.GetEnglishVersion, Constants.Bible_app_key);
            return allbibles;
        }

        public async Task<TResult> GetAllBibles<TResult>()
        {

            TResult allbibles = await requestProvider.GetAsync<TResult>(Constants.Bibles, Constants.Bible_app_key);
            return allbibles;
            
          
        }

        public async Task<TResult> GetOneBibleBooks<TResult>()
        {
            TResult allbibles = await requestProvider.GetAsync<TResult>(Constants.OneBible, Constants.Bible_app_key);
            return allbibles;
        }
    }
}

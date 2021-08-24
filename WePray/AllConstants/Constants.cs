using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace WePray.AllConstants
{
    internal static class Constants
    {
        public static string Bible_app_key = "1ff1ca8bb0002dd4bf1c901716fe93e9";
        public static string Base_Url = "https://api.scripture.api.bible/v1/";
        public static string Bibles = Base_Url + "bibles?language=eng";
        public static string OneBible = Base_Url + "bibles/" + BibleId;
        public static string GetEnglishVersion = Base_Url + "bibles?language=Eng";
        public static string AllBiblesVersesandChapters = OneBible + "/books?include-chapters=true&include-chapters-and-sections=false";
        //public static string AllBiblesVersesandContent = OneBible + "/chapters/" + $"{BookChapter}" + "?content-type=json&include-notes=false&include-titles=false&include-chapter-numbers=false&include-verse-numbers=true&include-verse-spans=false";

        private static string BibleId => Preferences.Get("BibleID", null);

        //private static string BookChapter => Preferences.Get("BookPickedID", null) + "." + Preferences.Get("ChapterPicked", null).ToString();


        //WordPress

        public static string Image_Url = "https://wepray.com.ng/wp-json/wp/v2/media/";
        public static string BaseWP_Url = "https://wepray.com.ng/wp-json/";
        public static string GetallWPposts = BaseWP_Url + "wp/v2/posts?context=view&orderby=date";
        public static string GetallWPSongs = BaseWP_Url + "wp/v2/posts?&per_page=20&orderby=date&categories=6";
        public static string GetallWPPrayers = BaseWP_Url + "wp/v2/posts?&per_page=20&orderby=date&categories=4";


        /// <summary>
        /// currently we are getting just the category of version number
        /// </summary>
        public static string GetcategorybyId = BaseWP_Url + $"wp/v2/categories/{6}";


    }
}

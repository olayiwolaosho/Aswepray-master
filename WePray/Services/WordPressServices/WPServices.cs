using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using WePray.AllConstants;
using WePray.Services.RequestProviders;
using WePrayWPResponseObject;
using WordPressPCL;
using WordPressPCL.Models;

namespace WePray.Services.WordPressServices
{
    class WPServices : IWPServices
    {
        IRequestProvider requestProvider;
        public WPServices()
        {
            requestProvider = new RequestProvider();    
        }

        public async Task<TResult> GetAllPosts<TResult>()
        {
            
            TResult allbibles = await requestProvider.GetAsync<TResult>(Constants.GetallWPposts);
            return allbibles;
        }

        public async Task<IEnumerable<TResult>> GetAllPrayers<TResult>()
        {
            IEnumerable<TResult> allbibles = await requestProvider.GetAsync<IEnumerable<TResult>>(Constants.GetallWPPrayers);
            return allbibles;
        }

        public async Task<IEnumerable<TResult>> GetAllSongs<TResult>()
        {

            IEnumerable<TResult> allbibles = await requestProvider.GetAsync<IEnumerable<TResult>>(Constants.GetallWPSongs);
            return allbibles;
        }   
        
        //public async Task<WPResponseObject> GetAllSongsHttP<TResult>()
        //{
        //    var httpClient = new HttpClient();
        //    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

        //    HttpResponseMessage response = await httpClient.GetAsync();

        //    string serialized = await response.Content.ReadAsStringAsync();

        //    var obj = WPResponseObject.FromJson(serialized);

        //    return obj;
        //}

        private static async Task<WordPressClient> GetClient()
        {
            // JWT authentication
            var client = new WordPressClient(Constants.BaseWP_Url, "wp/v2/posts?orderby=date&categories=2");
            client.AuthMethod = AuthMethod.JWT;
            await client.RequestJWToken("Origin", "V)n!mOd5^^t6)wR6P#rVpWrL");
            return client;
        }
    }
}

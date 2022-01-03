using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Reactive.Linq;
using WePray.Models;
using WePray.Repository._class;
using WePray.Services.WordPressServices;
using WePrayWPResponseObject;
using Akavache;
using System.Reactive;
using WePray.Enums;

namespace WePray.Repository
{
    /// <summary>
    /// using Akavache
    /// </summary>
    public class Repository : IRepository
    {

        string _prayercachekey = "DailyPrayers";

        string _devotionalcachekey = "DailyDevotioals";

        IConvertModel _convertModel;

        ObservableCollection<Prayer> _prayercollection = new ObservableCollection<Prayer>();



        DateTimeOffset CacheExpiry { get { return DateTime.Now.Add(TimeSpan.FromHours(1)); } }




        public Repository(IConvertModel convertModel)
        {
            this._convertModel = convertModel;
        }


        /// <summary>
        /// Get all prayers from cache/database (Akavache)
        /// </summary>
        /// <returns></returns>
        public async Task<ObservableCollection<Prayer>> GetAllPrayersFromDatabase()
        {
            return await InitializeLoading(DailyType.DailyPrayer);
        }

        /// <summary>
        /// This one is for refresh
        /// </summary>
        /// <returns></returns>
        void RefreshPrayer(ObservableCollection<Prayer> prayers)
        {
          
            BlobCache.LocalMachine.GetAndFetchLatest(_prayercachekey , async () => await GetAllPrayersFromWPwithInvalidate(), null , absoluteExpiration:CacheExpiry)
                .Subscribe(
                cachedThenUpdated => 
                {
                    prayers = cachedThenUpdated;
                });
        }


        /// <summary>
        /// This loads all the daily prayers will have to use  BlobCache.LocalMachine.GetAndFetchLatest  for better performsnce
        /// </summary>
        /// <returns></returns>
        async Task<ObservableCollection<Prayer>> LoadPrayersFromCache()
        {
            return await BlobCache.LocalMachine.GetOrFetchObject(_prayercachekey, GetAllPrayersFromWP, CacheExpiry);
        }
        
        
        /// <summary>
        /// This loads all the daily devotionals
        /// </summary>
        /// <returns></returns>
        async Task<ObservableCollection<Prayer>> LoadDevotionalsFromCache()
        {
            return await BlobCache.LocalMachine.GetOrFetchObject(_devotionalcachekey, GetAllDevotionalsFromWP, CacheExpiry);
        }


        /// <summary>
        /// This method gets all prayers from wordpress and wraps it inside an observable which is returned
        /// </summary>
        /// <returns>IObservable Instance</returns>
        async Task<ObservableCollection<Prayer>> GetAllPrayersFromWP()
        {
             return await _convertModel.ConvertAllWPResponseObjectToPrayers();
        }


        /// <summary>
        /// This method gets all devotionals from wordpress and wraps it inside an observable which is returned
        /// </summary>
        /// <returns>IObservable Instance</returns>
        async Task<ObservableCollection<Prayer>> GetAllDevotionalsFromWP()
        {
            return await _convertModel.ConvertAllWPResponseObjectToDevotionals();
        }


        /// <summary>
        /// This method gets all prayers from wordpress and wraps it inside an observable which is returned
        /// </summary>
        /// <returns>IObservable Instance</returns>
        async Task<ObservableCollection<Prayer>> GetAllPrayersFromWPwithInvalidate()
        {
            
             await BlobCache.LocalMachine.InvalidateObject<ObservableCollection<Prayer>>(_prayercachekey);
            await BlobCache.LocalMachine.Vacuum();
             return await _convertModel.ConvertAllWPResponseObjectToPrayers();
        }


        /// <summary>
        /// I'm using this for refresh it would get data from wordpress and store it in db
        /// </summary>
        /// <param name="Collectionempty">True if collection is emptu and false if collection is not empty</param>
        /// <returns></returns>
        public void GetPrayersFromWP(ObservableCollection<Prayer> prayers)
        {  
            RefreshPrayer(prayers);
        }


        //Currently I do not know how this really works why are we still using select many ? and what is an IObservable
        // I've named this method in this way because I didn't want to surface a null
        // when you don't have the key (trust me, the code looks awful) and I wanted
        // to show a bit more about Observables
        static IObservable<T> GetObjectSafe<T>(IBlobCache cache, string key)
        {
            // from the original IObservable<string> we can use LINQ-like operators
            // to transform the results and build up more complex sequences
            return cache.GetAllKeys()
                // Select() is a way to map the value received to a different type
                // other programming languages call this a "map"
                // In this case, we're mapping IEnumerable<string> to a boolean
                // this then flows to the subsequent method
                .Select(keys => keys.Contains(key))
                // SelectMany is a way to flatten Observable<Observable<T>> to Observable<T>
                // that probably sounds scary, but all we're doing here is, based on
                // whether we found the key, to lookup the value or return an empty observable.
                // to the caller this just looks like an Observable<T>, which makes their lives easier 
                .SelectMany(found => found ? cache.GetObject<T>(key) : Observable.Empty<T>());
        }


        /// <summary>
        /// Get all Devotionals from cache/database (Akavache)
        /// </summary>
        /// <returns></returns>
        public async Task<ObservableCollection<Prayer>> GetAllDevotionalsFromDatabase()
        {
            return await InitializeLoading(DailyType.DailyDevotionals);
        }


        /// <summary>
        /// This determines if we are loading prayes or daily devotionals
        /// </summary>
        /// <param name="dailyType"></param>
        /// <returns></returns>
        private Task<ObservableCollection<Prayer>> InitializeLoading(DailyType dailyType)
        {
           
           var devotionalOrPrayer = dailyType == DailyType.DailyPrayer ? LoadPrayersFromCache() : LoadDevotionalsFromCache();

            return devotionalOrPrayer;
        }
    }
}

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

namespace WePray.Repository
{
    /// <summary>
    /// using Akavache
    /// </summary>
    public class Repository : IRepository
    {

        IConvertModel _convertModel;
        ObservableCollection<Prayer> _prayercollection = new ObservableCollection<Prayer>();

        public Repository(IConvertModel convertModel)
        {
            this._convertModel = convertModel;
        }


        /// <summary>
        /// Gets the Daily Prayer from IconvertModelThis will still be worked on Need to make everything generic 
        /// </summary>
        /// <returns></returns>
        private async Task<ObservableCollection<Prayer>> GetconvertModelsPrayer()
        {
            var DailyPrayers = await _convertModel.ConvertAllWPResponseObjectToPrayers();
            return DailyPrayers;
        }


        /// <summary>
        /// Stores the Daily Prayer from IconvertModel into a blobcach 
        /// </summary>
        /// <returns></returns>
        private async Task StorePrayerInBlob()
        {
            var DailyPrayers = await GetconvertModelsPrayer();

            //Adds Daily Prayer to blob cache but is it always successful ?
            await BlobCache.LocalMachine.InsertObject("DailyPrayers", DailyPrayers,TimeSpan.FromDays(1));
             
        }


        /// <summary>
        /// Need to change this it looks too clustered
        /// </summary>
        /// <returns></returns>
        public async Task<ObservableCollection<Prayer>> GetAllPrayersFromDatabase()
        {
            var observable = GetObjectSafe<ObservableCollection<Prayer>>(BlobCache.LocalMachine, "DailyPrayers");
            observable.Subscribe(result => _prayercollection = result);
            if(_prayercollection != null && _prayercollection.Count != 0)
            {
               return _prayercollection;
            }
            await StorePrayerInBlob();
            var allprayers = await BlobCache.LocalMachine.GetObject<ObservableCollection<Prayer>>("DailyPrayers");
            return allprayers;
        }


        /// <summary>
        /// I'm using this for refresh it would always get data from wordpress and store it in db
        /// </summary>
        /// <returns></returns>
        public async Task<ObservableCollection<Prayer>> GetAllPrayersFromWP()
        {
            await StorePrayerInBlob();
            _prayercollection = await BlobCache.LocalMachine.GetObject<ObservableCollection<Prayer>>("DailyPrayers");
            return _prayercollection;
        }


        static IDisposable RunObservable(IBlobCache cache, string key)
        {
            return GetObjectSafe<Prayer>(cache, key)
                .Subscribe(
                    result => Console.WriteLine($"Found value: {result}"),
                    () => Console.WriteLine("Completed"));
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

    }
}

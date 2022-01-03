using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using WePray.Models;

namespace WePray.Repository
{
    /// <summary>
    /// Get data from IConvertMode, store it in database and return database to view model
    /// </summary>
    public interface IRepository
    {
        /// <summary>
        /// This actually gets all devotionals (wrong naming)
        /// </summary>
        /// <returns></returns>
        Task<ObservableCollection<Prayer>> GetAllPrayersFromDatabase();

        /// <summary>
        ///  This gets all the prayers from word press
        /// </summary>
        /// <returns></returns>
        Task<ObservableCollection<Prayer>> GetAllDevotionalsFromDatabase();

        /// <summary>
        /// I'm using this for refresh it would get data from wordpress and store it in db
        /// </summary>
        /// <param name="Collectionempty">True if collection is emptu and false if collection is not empty</param>
        /// <returns></returns>
        void GetPrayersFromWP(ObservableCollection<Prayer> prayers);
    }
}

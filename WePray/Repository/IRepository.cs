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
        Task<ObservableCollection<Prayer>> GetAllPrayersFromDatabase();

        Task<ObservableCollection<Prayer>> GetAllPrayersFromWP();
    }
}

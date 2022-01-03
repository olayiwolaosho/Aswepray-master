using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using WePray.Models;

namespace WePray.Repository._class
{
    /// <summary>
    /// This Interface is what communicates with the service that gets the data from wordpress, converts the data to app models,stores it in database and sends it to the view model
    /// </summary>
    public interface IConvertModel
    {
        /// <summary>
        /// This methods converts all wpresponse objects to prayer objects and returns it 
        /// </summary>
        /// <returns></returns>
        Task<ObservableCollection<Prayer>> ConvertAllWPResponseObjectToPrayers();
        Task<ObservableCollection<Prayer>> ConvertAllWPResponseObjectToDevotionals();
    }
}

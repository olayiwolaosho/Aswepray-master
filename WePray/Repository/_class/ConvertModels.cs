using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using WePray.Models;
using WePray.Services.WordPressServices;
using WePrayWPResponseObject;

namespace WePray.Repository._class
{
    /// <summary>
    /// What this class really does is map the  WPResponseObject to Prayer object
    /// </summary>
    public class ConvertModels : IConvertModel
    {
        IWPServices wPServices;

        public ConvertModels(IWPServices wPServices)
        {
            this.wPServices = wPServices;
        }

        /// <summary>
        /// Making prayers an observable collection so that no overhead of converting it from one type to another when it is returned to the view model
        /// </summary>
        ObservableCollection<Prayer> Prayers = new ObservableCollection<Prayer>();

        ObservableCollection<Prayer> Devotionals = new ObservableCollection<Prayer>();


        /// <summary>
        ///  Method that does the mappng from WPResponseObject to Prayer
        /// </summary>
        /// <param name="WPObject"></param>
        /// <returns></returns>
        private ObservableCollection<Prayer> Convert(IEnumerable<WPResponseObject> WPObject, ObservableCollection<Prayer> prayerOrDevotional)
        {
            if (prayerOrDevotional.Any())
            {
                prayerOrDevotional.Clear();
            }

            foreach (var item in WPObject)
            {
                prayerOrDevotional.Add(new Prayer
                {
                    Id = item.Id,
                    Title = item.Title.Rendered,
                    image = "ASwePray.png",
                    Date = item.Date,
                    Description = HtmlConvert.Gettext(item.Content.Rendered)
                });
            }
            return prayerOrDevotional;
        }


        public async Task<ObservableCollection<Prayer>> ConvertAllWPResponseObjectToPrayers()
        {
            var WPObject = await wPServices.GetAllPrayers<WPResponseObject>();
            return Convert(WPObject, Prayers);
        }


        public async Task<ObservableCollection<Prayer>> ConvertAllWPResponseObjectToDevotionals()
        {
            var WPObject = await wPServices.GetAllDevotionals<WPResponseObject>();
            return Convert(WPObject, Devotionals);
        }

    }
}

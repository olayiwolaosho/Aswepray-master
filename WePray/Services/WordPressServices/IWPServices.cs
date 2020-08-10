using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using WordPressPCL;

namespace WePray.Services.WordPressServices
{
    public interface IWPServices
    {
        Task<TResult> GetAllPosts<TResult>();
        Task<IEnumerable<TResult>> GetAllSongs<TResult>();
        Task<IEnumerable<TResult>> GetAllPrayers<TResult>();
    }
}

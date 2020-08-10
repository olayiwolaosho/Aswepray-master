using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WePray.Services.BibleServices
{
    public interface IBibleService
    {
        Task<TResult> GetAllBibles<TResult>();
        Task<TResult> GetAllBibleBooks<TResult>();
        Task<TResult> GetOneBibleBooks<TResult>();
        Task<TResult> GetEnglishBibleVersions<TResult>();
        Task<TResult> GetAllBibleBookChapters<TResult>();
        Task<TResult> GetAllBibleBookChapterVerses<TResult>();
        Task<TResult> GetAllBibleBookChapterVerseScriptures<TResult>();
    }
}

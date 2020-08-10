using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace WePray.Dependencies
{
    public interface IAudio
    {
        Task<bool> Play_Pause(string url);
        bool Stop(bool val);

    }

}
using System;
using System.Collections.Generic;
using System.Text;

namespace WePray.Services.Connection
{
    /// <summary>
    /// Interface to check if there is internet connection
    /// </summary>
    public interface IConnection
    {

        /// <summary>
        /// Check is there is internet connection
        /// </summary>
        /// <returns></returns>
        bool CheckWifi();

        /// <summary>
        /// Check wifi whenever there is a change in in network connection
        /// </summary>
        /// <returns></returns>
        void CheckWifiContinuously();
    }
}

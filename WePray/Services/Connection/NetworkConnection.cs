using Plugin.Connectivity;
using System;
using System.Collections.Generic;
using System.Text;

namespace WePray.Services.Connection
{
    public class NetworkConnection : IConnection
    {
        public bool CheckWifi()
        {
            return CrossConnectivity.Current.IsConnected;
        }

        public void CheckWifiContinuously()
        {
            CrossConnectivity.Current.ConnectivityChanged += (sender, args) =>
            {
                CheckWifi();
            };
        }
    }
}

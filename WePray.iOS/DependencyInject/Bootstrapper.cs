using Foundation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UIKit;
using WePray;

namespace WePray.iOS.DependencyInject
{
    public class Bootstrapper : WePray.DependencyInject.Bootstrapper
    {
        public static void Init()
        {
            var instance = new Bootstrapper();
        }
    }
}
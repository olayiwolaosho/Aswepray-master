using System;
using System.Collections.Generic;
using System.Text;

namespace WePray.Models
{
    /// <summary>
    /// This interface is used as a base type for prayers because riht now both Dailydevotionals and daily prayers use the same model but this might change soon 
    /// when the chanfe happens i dont want it o affect the observable collections they are wrappend around in their respective views
    /// </summary>
    public interface IWPObject
    {
    }
}

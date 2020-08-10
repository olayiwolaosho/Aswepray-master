using System;
using System.Collections.Generic;
using System.Text;

namespace WePray.ViewModels.Base
{
    public abstract class BaseViewModel : ExtendedBindableObject
    {

        private bool _isBusy;

        public bool IsBusy
        {
            get
            {
                return _isBusy;
            }

            set
            {
                _isBusy = value;
                RaisePropertyChanged(() => IsBusy);
            }
        }
    }
}

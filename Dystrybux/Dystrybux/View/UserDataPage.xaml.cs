using Dystrybux.ViewModel;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Dystrybux.View
{
    public partial class UserDataPage : ContentPage{

        UserDataViewModel _userDataViewModel;

        public UserDataPage(){
            InitializeComponent();
            BindingContext = _userDataViewModel = new UserDataViewModel();
           
        }

    }
}

using Dystrybux.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Dystrybux.View {
    public partial class LoginPage : ContentPage {
        public LoginPage() {
            InitializeComponent();
            NavigationPage.SetHasNavigationBar(this, false);
            this.BindingContext = new LoginViewModel();
        }
    }
}
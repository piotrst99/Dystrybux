using Dystrybux.Model;
using Dystrybux.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Dystrybux.View {
    public partial class MainMenu : ContentPage {
        
        public MainMenu(User user) {
            InitializeComponent();
            //NavigationPage.SetHasNavigationBar(this, false);
            NavigationPage.SetHasBackButton(this, false);
            this.BindingContext = new MainMenuViewModel(user);
        }
    }
}
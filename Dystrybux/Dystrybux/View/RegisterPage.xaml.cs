using Dystrybux.ViewModel;
using SQLite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Dystrybux.View {
    public partial class RegisterPage : ContentPage {
        public RegisterPage() {
            InitializeComponent();
            this.BindingContext = new RegisterViewModel();
        }
    }
}
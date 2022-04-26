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
    public partial class NewItemPage : ContentPage {
        public Product Product { get; set; }

        NewItemViewModel _newItemViewModel;

        public NewItemPage() {
            InitializeComponent();
            BindingContext = _newItemViewModel = new NewItemViewModel();
            //this.BindingContext = new NewItemViewModel();
        }
    }
}
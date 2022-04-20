using Dystrybux.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Dystrybux.View {
    public partial class ProductPage : ContentPage {
        ProductViewModel _productViewModel;

        public ProductPage() {
            InitializeComponent();
            //this.BindingContext = new ProductViewModel();
            BindingContext = _productViewModel = new ProductViewModel();
        }

        protected override void OnAppearing() {
            base.OnAppearing();
            _productViewModel.OnAppearing();
        }
    }
}   
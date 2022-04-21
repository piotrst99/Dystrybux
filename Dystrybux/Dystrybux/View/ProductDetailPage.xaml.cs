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
    public partial class ProductDetailPage : ContentPage {
        public ProductDetailPage(Product product) {
            InitializeComponent();
            this.BindingContext = new ProductDetailViewModel(product);
        }

    }
}
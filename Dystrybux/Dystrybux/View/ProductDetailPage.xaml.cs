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
        ProductDetailViewModel _productDetailViewModel;

        public ProductDetailPage(Product product) {
            InitializeComponent();
            this.BindingContext = _productDetailViewModel  = new ProductDetailViewModel(product);
        
            if(App.User.Role == "Business"){
                var deleteProductTollBarItem = new ToolbarItem
                {
                    Text = "Usuń",
                    Command = _productDetailViewModel.DeleteItemCommand
                };

                var editProductTollBarItem = new ToolbarItem
                {
                    Text = "Edytuj",
                    Command = _productDetailViewModel.EditItemCommand
                };

                this.ToolbarItems.Add(deleteProductTollBarItem);
                this.ToolbarItems.Add(editProductTollBarItem);

            }

        }

    }
}
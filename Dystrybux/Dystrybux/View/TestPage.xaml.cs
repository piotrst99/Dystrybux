using Dystrybux.Model;
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
    public partial class TestPage : ContentPage{
        TestViewModel _productViewModel;

        public TestPage(){
            InitializeComponent();
            //this.BindingContext = new ProductViewModel();
            BindingContext = _productViewModel = new TestViewModel();

            if (App.User.Role == "Business") {
                var addProductToolBarItem = new ToolbarItem {
                    Text = "Dodaj",
                    Command = _productViewModel.AddItemCommand,
                };
                this.ToolbarItems.Add(addProductToolBarItem);
            }

        }

        public TestPage(bool isSearch, Order order) {
            InitializeComponent();
            //this.BindingContext = new ProductViewModel();
            BindingContext = _productViewModel = new TestViewModel(isSearch, order);

            if (App.User.Role == "Business") {
                var addProductToolBarItem = new ToolbarItem {
                    Text = "Dodaj",
                    Command = _productViewModel.AddItemCommand,
                };
                this.ToolbarItems.Add(addProductToolBarItem);
            }

        }

        protected override void OnAppearing() {
            base.OnAppearing();
            _productViewModel.OnAppearing();
        }

        private void SearchBar_TextChanged(object sender, TextChangedEventArgs e) {

        }
    }
}

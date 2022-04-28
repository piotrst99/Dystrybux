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
    public partial class NewOrderPage : ContentPage {

        NewOrderViewModel _newOrderViewModel;

        public NewOrderPage(string orderName) {
            InitializeComponent();
            //this.BindingContext = new NewOrderViewModel();
            BindingContext = _newOrderViewModel = new NewOrderViewModel(orderName);

            if (App.User.Role == "Client") {
                var searchProductToolBarItem = new ToolbarItem {
                    Text = "Przeglądaj",
                    Command = _newOrderViewModel.SearchProductCommand,
                };
                this.ToolbarItems.Add(searchProductToolBarItem);
            }
        }

        public NewOrderPage(Order order) {
            InitializeComponent();
            //this.BindingContext = new NewOrderViewModel(order);
            BindingContext = _newOrderViewModel = new NewOrderViewModel(order);

            if (App.User.Role == "Client") {
                var searchProductToolBarItem = new ToolbarItem {
                    Text = "Przeglądaj",
                    Command = _newOrderViewModel.SearchProductCommand,
                };
                this.ToolbarItems.Add(searchProductToolBarItem);
            }
        }

        protected override void OnAppearing() {
            base.OnAppearing();
            _newOrderViewModel.OnAppearing();
        }

    }
}
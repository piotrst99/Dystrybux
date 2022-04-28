using Dystrybux.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Dystrybux.View {
    public partial class OrderPage : ContentPage {
        OrderViewModel _orderViewModell;
        
        public OrderPage() {
            InitializeComponent();
            BindingContext = _orderViewModell = new OrderViewModel();

            if (App.User.Role == "Client") {
                var searchProductToolBarItem = new ToolbarItem {
                    Text = "Stwórz zamówienie",
                    Command = _orderViewModell.CreateOrderCommand,
                };
                this.ToolbarItems.Add(searchProductToolBarItem);
            }

        }

        protected override void OnAppearing() {
            base.OnAppearing();
            _orderViewModell.OnAppearing();
        }

    }
}
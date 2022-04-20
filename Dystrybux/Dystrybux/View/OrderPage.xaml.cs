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
        }

        protected override void OnAppearing() {
            base.OnAppearing();
            _orderViewModell.OnAppearing();
        }

    }
}
using Dystrybux.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using Xamarin.Forms;

namespace Dystrybux.ViewModel {
    public class PlacedOrderViewModel : BaseViewModel {

        private Order _selectedOrder = null;
        private Delivery _delivery = null;
        private User _user = null;
        private double _totalCostProduct = 0.0;
        private bool _IsEmployee = false;

        public ObservableCollection<OrderProduct> ProductsFromOrder { set; get; }

        public PlacedOrderViewModel(Order order) {
            Order = order;
            LoadData();
            LoadProducts();
            AcceptOrderCommand = new Command(() => AcceptOrder());
            RejectOrderCommand = new Command(() => RejectOrder());
        }

        void LoadData() {
            User = App.Database.GetUserAsync(Order.UserID).Result;
            Delivery = App.Database.GetDeliveryFromOrderAsync(Order.DeliveryID).Result;
            IsEmployee = App.User.Role != "Client";
        }

        void LoadProducts() {
            ProductsFromOrder = new ObservableCollection<OrderProduct>();
            var items = App.Database.GetOrderProductsAsync(Order.ID).Result;
            foreach (var p in items) { ProductsFromOrder.Add(p); }
            TotalCostProduct = ProductsFromOrder.Sum(q => q.TotalCostForProduct);
        }

        void AcceptOrder() {
            Device.BeginInvokeOnMainThread(async () => {
                bool choice = await App.Current.MainPage.DisplayAlert("", "Czy przyjmujesz zamówienie do realizacji?", "Tak", "Nie");
                if (choice){
                    Order.Status = "W realizacji";
                    await App.Database.UpdateOrderAsync(Order);
                    await App.Navigation.PopAsync();
                }
            });
        }

        void RejectOrder() {
            Device.BeginInvokeOnMainThread(async () => {
                bool choice = await App.Current.MainPage.DisplayAlert("", "Czy odrzucić zamówienie?", "Tak", "Nie");
                if (choice){
                    Order.Status = "Anulowane";
                    await App.Database.UpdateOrderAsync(Order);
                    await App.Navigation.PopAsync();
                }
            });
        }

        public Order Order {
            get => _selectedOrder;
            set => _selectedOrder = value;
        }

        public Delivery Delivery {
            get => _delivery;
            set => _delivery = value;
        }

        public User User {
            get => _user;
            set => _user = value;
        }

        public double TotalCostProduct {
            get => _totalCostProduct;
            set => _totalCostProduct = value;
        }

        public bool IsEmployee {
            get => _IsEmployee;
            set => _IsEmployee = value;
        }

        public Command AcceptOrderCommand { protected set; get; }
        public Command RejectOrderCommand { protected set; get; }

    }
}

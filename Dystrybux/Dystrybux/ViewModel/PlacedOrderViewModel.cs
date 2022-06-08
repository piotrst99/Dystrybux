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
        List<string> sciezki = new List<string>();

        private bool _IsOrdered = false;
        private bool _IsProgress = false;
        private bool _IsDelivery = false;
        private bool _IsEnd = false;

        public ObservableCollection<OrderProduct> ProductsFromOrder { set; get; }

        public PlacedOrderViewModel(Order order) {
            Order = order;
            LoadData();
            LoadProducts();
            AcceptOrderCommand = new Command(() => AcceptOrder());
            RejectOrderCommand = new Command(() => RejectOrder());
            SendOrderToDeliveryCommand = new Command(() => SendOrderToDelivery());
            DoneOrderCommand = new Command(() => DoneOrder());
        }

        void LoadData() {

            //Order.DeadlinePassed = false;
            //App.Database.UpdateOrderAsync(Order);

            // ID = 5
            /*Device.BeginInvokeOnMainThread(async () => {
                await App.Current.MainPage.DisplayAlert("Result", Order.ID.ToString(), "OK");
            });*/


            IsOrdered = Order.Status == "Złożono";
            IsProgress = Order.Status == "W realizacji";
            IsDelivery = Order.Status == "W dostawie";
            //IsEnd = Order.Status == "W dostawie";

            foreach (var fileName in System.IO.Directory.GetFiles("/storage/emulated/0/Pictures/Dystrybux.Android")) {
                sciezki.Add(fileName);
            }

            User = App.Database.GetUserAsync(Order.UserID).Result;
            Delivery = App.Database.GetDeliveryFromOrderAsync(Order.DeliveryID).Result;
            IsEmployee = App.User.Role != "Client";
        }

        void LoadProducts() {
            ProductsFromOrder = new ObservableCollection<OrderProduct>();
            var items = App.Database.GetOrderProductsAsync(Order.ID).Result;
            foreach (var p in items) {
                string imagePath = sciezki.Where(q => q.Contains(p.Product.ImagePath)).FirstOrDefault();

                if (!string.IsNullOrEmpty(imagePath)) {
                    p.Product.Image = (Device.RuntimePlatform == Device.Android) ?
                        ImageSource.FromFile(imagePath) :
                        ImageSource.FromFile("/storage/emulated/0/Pictures/Dystrybux.Android/productImage_1704_495_20220606_105829.png");
                }

                ProductsFromOrder.Add(p); 
            }
            TotalCostProduct = Order.DeadlinePassed ? 0.7 * ProductsFromOrder.Sum(q => q.TotalCostForProduct) : 
                ProductsFromOrder.Sum(q => q.TotalCostForProduct);
        }

        void AcceptOrder() {
            Device.BeginInvokeOnMainThread(async () => {
                bool choice = await App.Current.MainPage.DisplayAlert("", "Czy przyjmujesz zamówienie do realizacji?", "Tak", "Nie");
                if (choice){
                    Order.Status = "W realizacji";

                    IsOrdered = false;
                    IsProgress = true;

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

        void SendOrderToDelivery() {
            Device.BeginInvokeOnMainThread(async () => {
                bool choice = await App.Current.MainPage.DisplayAlert("", "Czy wysłać zamówienie do transportu?", "Tak", "Nie");
                if (choice) {
                    Order.Status = "W dostawie";
                    await App.Database.UpdateOrderAsync(Order);
                    await App.Navigation.PopAsync();
                }
            });
        }

        void DoneOrder() {
            Device.BeginInvokeOnMainThread(async () => {
                bool choice = await App.Current.MainPage.DisplayAlert("", "Czy potwierdzić dostarczenie zamówienia?", "Tak", "Nie");
                if (choice) {
                    Order.Status = "Zakończono";
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

        public bool IsOrdered {
            get => _IsOrdered;
            set => _IsOrdered = value;
        }

        public bool IsProgress {
            get => _IsProgress;
            set => _IsProgress = value;
        }

        public bool IsDelivery {
            get => _IsDelivery;
            set => _IsDelivery = value;
        }

        public bool IsEnd {
            get => _IsEnd;
            set => _IsEnd = value;
        }

        public Command AcceptOrderCommand { protected set; get; }
        public Command RejectOrderCommand { protected set; get; }
        public Command SendOrderToDeliveryCommand { protected set; get; }
        public Command DoneOrderCommand { protected set; get; }

    }
}

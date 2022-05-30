using Dystrybux.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Dystrybux.ViewModel {
    public class OrderSummaryViewModel : BaseViewModel {

        private Order _order = null;
        private Delivery _delivery = null;
        private double _totalCostProduct = 0.0;

        public ObservableCollection<OrderProduct> AddedProductsFromOrder { get; set; }

        public OrderSummaryViewModel() {
            SetData();
            SubmitOrderCommand = new Command(() => SubmitOrder());
        }

        void SetData() {
            Order = App.Database.GetUndoneOrderAsync("Nie złożono").Result;
            Delivery = App.Database.GetDeliveryFromOrderAsync(Order.DeliveryID).Result;


            AddedProductsFromOrder = new ObservableCollection<OrderProduct>();
            try {
                AddedProductsFromOrder.Clear();

                var items = App.Database.GetOrderProductsAsync(Order.ID).Result;
                foreach (var p in items) {
                    AddedProductsFromOrder.Add(p);
                }

                TotalCostProduct = AddedProductsFromOrder.Sum(q => q.TotalCostForProduct);
            }
            catch (Exception) { throw; }
        }

        void SubmitOrder() {
            Device.BeginInvokeOnMainThread(async () => {
            bool choice = await App.Current.MainPage.DisplayAlert("", "Czy wysłać zamówienie do realizacji?", "Tak", "Nie");
            if (choice) {
                   try {
                        Order.Name = $"{new Random().Next(10000, 99999)} + {new Random().Next(10000, 99999)}";
                        Order.Status = "Złożono";
                        Order.OrderedDate = DateTime.Now.ToString();
                        await App.Database.UpdateOrderAsync(_order);
                    }
                    catch (Exception) { throw; }
                }
                App.Navigation.RemovePage(App.Navigation.NavigationStack[App.Navigation.NavigationStack.Count - 2]);
                await App.Navigation.PopAsync(false);
                await App.Navigation.PopAsync(false);
                //await App.Navigation.PopAsync(false);
            });
        }

        public Order Order {
            get => _order;
            set => _order = value;
        }

        public Delivery Delivery {
            get => _delivery;
            set => _delivery = value;
        }

        public double TotalCostProduct {
            get => _totalCostProduct;
            set => _totalCostProduct = value;
        }

        public Command SubmitOrderCommand { protected set; get; }

    }
}

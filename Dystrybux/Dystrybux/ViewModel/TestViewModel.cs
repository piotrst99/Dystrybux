using Dystrybux.Model;
using Dystrybux.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using Xamarin.Forms;

namespace Dystrybux.ViewModel {
    public class TestViewModel : BaseViewModel {

        public ObservableCollection<Order> Orders { get; }
        public ObservableCollection<Order> OrdersInProgress { get; }
        public ObservableCollection<Order> OrdersInDelivery { get; }
        public ObservableCollection<Order> OrdersDone { get; }
        public ObservableCollection<Order> OrdersDenied { get; }


        public TestViewModel() {
            Orders = new ObservableCollection<Order>();
            OrdersInProgress = new ObservableCollection<Order>();
            OrdersInDelivery = new ObservableCollection<Order>();
            OrdersDone = new ObservableCollection<Order>();
            OrdersDenied = new ObservableCollection<Order>();

            OrderTapped = new Command<Order>(OnOrderSelected);
            SetOrdersToLists();
        }
        
        void SetOrdersToLists(){
            List<Order> orders = new List<Order>();
            if (App.User.Role == "Client"){
                orders = App.Database.GetOrdersForUserAsync("Złożono").Result;
                foreach (var o in orders){
                    Orders.Add(o);
                }

                orders = App.Database.GetOrdersForUserAsync("W realizacji").Result;
                foreach (var o in orders){
                    OrdersInProgress.Add(o);
                }

                orders = App.Database.GetOrdersForUserAsync("W dostawie").Result;
                foreach (var o in orders){
                    OrdersInDelivery.Add(o);
                }

                orders = App.Database.GetOrdersForUserAsync("Zakończono").Result;
                foreach (var o in orders){
                    OrdersDone.Add(o);
                }

                orders = App.Database.GetOrdersForUserAsync("Anulowane").Result;
                foreach (var o in orders){
                    OrdersDenied.Add(o);
                }

            }
            else{
                orders = App.Database.GetOrdersForEmployeeAsync("Złożono").Result;
                foreach (var o in orders){
                    Orders.Add(o);
                }

                orders = App.Database.GetOrdersForEmployeeAsync("W realizacji").Result;
                foreach (var o in orders){
                    OrdersInProgress.Add(o);
                }

                orders = App.Database.GetOrdersForEmployeeAsync("W dostawie").Result;
                foreach (var o in orders){
                    OrdersInDelivery.Add(o);
                }

                orders = App.Database.GetOrdersForEmployeeAsync("Zakończono").Result;
                foreach (var o in orders){
                    OrdersDone.Add(o);
                }

                orders = App.Database.GetOrdersForEmployeeAsync("Anulowane").Result;
                foreach (var o in orders){
                    OrdersDenied.Add(o);
                }

            }
        }

        private async void OnOrderSelected(Order order)
        {
            if (order == null)
                return;

            //await App.Navigation.PushAsync(new NewOrderPage(order));
            await App.Navigation.PushAsync(new PlacedOrderPage(order));
        }

        public Command OrderTapped { protected set; get; }

    }
}

using Dystrybux.Model;
using Dystrybux.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Dystrybux.ViewModel {
    public class TestViewModel : BaseViewModel {

        private bool _IsRefreshing = false;
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

            LoadOrdersCommand = new Command(async () => await ExecuteLoadOrdersCommand());
            LoadOrdersCommand2 = new Command(async () => await ExecuteLoadOrdersCommand2());
            LoadOrdersCommand3 = new Command(async () => await ExecuteLoadOrdersCommand3());
            LoadOrdersCommand4 = new Command(async () => await ExecuteLoadOrdersCommand4());
            LoadOrdersCommand5 = new Command(async () => await ExecuteLoadOrdersCommand5());

            OrderTapped = new Command<Order>(OnOrderSelected);
            SetOrdersToLists();
        }
        
        void SetOrdersToLists(){
            List<Order> orders = new List<Order>();
            if (App.User.Role == "Client"){
                orders = App.Database.GetOrdersForUserAsync().Result;
                foreach (var o in orders){
                    o.User = App.Database.GetUserAsync(o.UserID).Result;
                    o.Delivery = App.Database.GetDeliveryFromOrderAsync(o.DeliveryID).Result;
                    o.TotalCost = (int)App.Database.GetOrderProductsAsync(o.ID).Result.Sum(q => q.TotalCostForProduct);
                    //Orders.Add(o);
                }

                foreach (var o in orders) {
                    if (o.Status == "Złożono") Orders.Add(o);
                    else if (o.Status == "W realizacji") OrdersInProgress.Add(o);
                    else if (o.Status == "W dostawie") OrdersInDelivery.Add(o);
                    else if (o.Status == "Zakończono") OrdersDone.Add(o);
                    else if (o.Status == "Anulowane") OrdersDenied.Add(o);

                    if(o.Delivery.LatestDate.Date < DateTime.Now.Date) {
                        o.DeadlinePassed = true;
                        App.Database.UpdateOrderAsync(o);
                    }

                }

                /*orders = App.Database.GetOrdersForUserAsync("W realizacji").Result;
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
                }*/

            }
            else{
                orders = App.Database.GetOrdersForEmployeeAsync().Result;
                foreach (var o in orders) {
                    o.User = App.Database.GetUserAsync(o.UserID).Result;
                    o.Delivery = App.Database.GetDeliveryFromOrderAsync(o.DeliveryID).Result;
                    o.TotalCost = (int)App.Database.GetOrderProductsAsync(o.ID).Result.Sum(q => q.TotalCostForProduct);
                    //Orders.Add(o);
                }

                foreach (var o in orders) {
                    if (o.Status == "Złożono") Orders.Add(o);
                    else if (o.Status == "W realizacji") OrdersInProgress.Add(o);
                    else if (o.Status == "W dostawie") OrdersInDelivery.Add(o);
                    else if (o.Status == "Zakończono") OrdersDone.Add(o);
                    else if (o.Status == "Anulowane") OrdersDenied.Add(o);

                    if (o.Delivery.LatestDate.Date < DateTime.Now.Date) {
                        o.DeadlinePassed = true;
                        App.Database.UpdateOrderAsync(o);
                    }

                }
                /*orders = App.Database.GetOrdersForEmployeeAsync("Złożono").Result;
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
                }*/

            }
        }

        async Task ExecuteLoadOrdersCommand() {
            IsRefreshing = true;

            try {
                Orders.Clear();
                OrdersInProgress.Clear();
                OrdersInDelivery.Clear();
                OrdersDone.Clear();
                OrdersDenied.Clear();

                List<Order> orders = new List<Order>();
                if (App.User.Role == "Client") {
                    orders = await App.Database.GetOrdersForUserAsync();
                    foreach (var o in orders) {
                        o.User = await App.Database.GetUserAsync(o.UserID);
                        o.Delivery = await App.Database.GetDeliveryFromOrderAsync(o.DeliveryID);
                        //o.TotalCost = (int)App.Database.GetOrderProductsAsync(o.ID).Result.Sum(q => q.TotalCostForProduct);

                        var v = await App.Database.GetOrderProductsAsync(o.ID);
                        o.TotalCost = (int)v.Sum(q => q.TotalCostForProduct);

                        //Orders.Add(o);
                    }

                    foreach (var o in orders) {
                        if (o.Status == "Złożono") Orders.Add(o);
                        else if (o.Status == "W realizacji") OrdersInProgress.Add(o);
                        else if (o.Status == "W dostawie") OrdersInDelivery.Add(o);
                        else if (o.Status == "Zakończono") OrdersDone.Add(o);
                        else if (o.Status == "Anulowane") OrdersDenied.Add(o);
                    }

                }
                else {
                    orders = await App.Database.GetOrdersForEmployeeAsync();
                    foreach (var o in orders) {
                        o.User = await App.Database.GetUserAsync(o.UserID);
                        o.Delivery = await App.Database.GetDeliveryFromOrderAsync(o.DeliveryID);
                        var v = await App.Database.GetOrderProductsAsync(o.ID);
                        o.TotalCost = (int)v.Sum(q => q.TotalCostForProduct);
                        
                    }

                    foreach (var o in orders) {
                        if (o.Status == "Złożono") Orders.Add(o);
                        else if (o.Status == "W realizacji") OrdersInProgress.Add(o);
                        else if (o.Status == "W dostawie") OrdersInDelivery.Add(o);
                        else if (o.Status == "Zakończono") OrdersDone.Add(o);
                        else if (o.Status == "Anulowane") OrdersDenied.Add(o);
                    }
                }
            }
            catch (Exception) { throw; }


            IsRefreshing = false;
        }

        

        private async void OnOrderSelected(Order order){
            if (order == null)
                return;

            //await App.Navigation.PushAsync(new NewOrderPage(order));
            await App.Navigation.PushAsync(new PlacedOrderPage(order));
        }

        public void OnAppearing() {
            IsRefreshing = true;
        }

        public bool IsRefreshing {
            set {
                _IsRefreshing = value;
                OnPropertyChanged();
            }
            get => _IsRefreshing;
        }

        public Command OrderTapped { protected set; get; }
        public Command LoadOrdersCommand { protected set; get; }
        public Command LoadOrdersCommand2 { protected set; get; }
        public Command LoadOrdersCommand3 { protected set; get; }
        public Command LoadOrdersCommand4 { protected set; get; }
        public Command LoadOrdersCommand5 { protected set; get; }

        ///////////////////
        ///

        async Task ExecuteLoadOrdersCommand2() {
            IsRefreshing = true;

            try {
                //Orders.Clear();
                OrdersInProgress.Clear();
                //OrdersInDelivery.Clear();
                //OrdersDone.Clear();
                //OrdersDenied.Clear();

                List<Order> orders = new List<Order>();
                if (App.User.Role == "Client") {
                    orders = await App.Database.GetOrdersForUserAsync();
                    foreach (var o in orders) {
                        o.User = await App.Database.GetUserAsync(o.UserID);
                        o.Delivery = await App.Database.GetDeliveryFromOrderAsync(o.DeliveryID);
                        //o.TotalCost = (int)App.Database.GetOrderProductsAsync(o.ID).Result.Sum(q => q.TotalCostForProduct);

                        var v = await App.Database.GetOrderProductsAsync(o.ID);
                        o.TotalCost = (int)v.Sum(q => q.TotalCostForProduct);

                        //Orders.Add(o);
                    }

                    foreach (var o in orders) {
                        //if (o.Status == "Złożono") Orders.Add(o);
                        //else if (o.Status == "W realizacji") OrdersInProgress.Add(o);
                        if (o.Status == "W realizacji") OrdersInProgress.Add(o);
                        //else if (o.Status == "W dostawie") OrdersInDelivery.Add(o);
                        //else if (o.Status == "Zakończono") OrdersDone.Add(o);
                        //else if (o.Status == "Anulowane") OrdersDenied.Add(o);
                    }

                }
                else {
                    orders = await App.Database.GetOrdersForEmployeeAsync();
                    foreach (var o in orders) {
                        o.User = await App.Database.GetUserAsync(o.UserID);
                        o.Delivery = await App.Database.GetDeliveryFromOrderAsync(o.DeliveryID);
                        var v = await App.Database.GetOrderProductsAsync(o.ID);
                        o.TotalCost = (int)v.Sum(q => q.TotalCostForProduct);

                    }

                    foreach (var o in orders) {
                        //if (o.Status == "Złożono") Orders.Add(o);
                        //else if (o.Status == "W realizacji") OrdersInProgress.Add(o);
                        if (o.Status == "W realizacji") OrdersInProgress.Add(o);
                        //else if (o.Status == "W dostawie") OrdersInDelivery.Add(o);
                        //else if (o.Status == "Zakończono") OrdersDone.Add(o);
                        //else if (o.Status == "Anulowane") OrdersDenied.Add(o);
                    }
                }
            }
            catch (Exception) { throw; }


            IsRefreshing = false;
        }

        async Task ExecuteLoadOrdersCommand3() {
            IsRefreshing = true;

            try {
                //Orders.Clear();
                //OrdersInProgress.Clear();
                OrdersInDelivery.Clear();
                //OrdersDone.Clear();
                //OrdersDenied.Clear();

                List<Order> orders = new List<Order>();
                if (App.User.Role == "Client") {
                    orders = await App.Database.GetOrdersForUserAsync();
                    foreach (var o in orders) {
                        o.User = await App.Database.GetUserAsync(o.UserID);
                        o.Delivery = await App.Database.GetDeliveryFromOrderAsync(o.DeliveryID);
                        //o.TotalCost = (int)App.Database.GetOrderProductsAsync(o.ID).Result.Sum(q => q.TotalCostForProduct);

                        var v = await App.Database.GetOrderProductsAsync(o.ID);
                        o.TotalCost = (int)v.Sum(q => q.TotalCostForProduct);

                        //Orders.Add(o);
                    }

                    foreach (var o in orders) {
                        //if (o.Status == "Złożono") Orders.Add(o);
                        //else if (o.Status == "W realizacji") OrdersInProgress.Add(o);
                        //else if (o.Status == "W dostawie") OrdersInDelivery.Add(o);
                        if (o.Status == "W dostawie") OrdersInDelivery.Add(o);
                        //else if (o.Status == "Zakończono") OrdersDone.Add(o);
                        //else if (o.Status == "Anulowane") OrdersDenied.Add(o);
                    }

                }
                else {
                    orders = await App.Database.GetOrdersForEmployeeAsync();
                    foreach (var o in orders) {
                        o.User = await App.Database.GetUserAsync(o.UserID);
                        o.Delivery = await App.Database.GetDeliveryFromOrderAsync(o.DeliveryID);
                        var v = await App.Database.GetOrderProductsAsync(o.ID);
                        o.TotalCost = (int)v.Sum(q => q.TotalCostForProduct);

                    }

                    foreach (var o in orders) {
                        //if (o.Status == "Złożono") Orders.Add(o);
                        //else if (o.Status == "W realizacji") OrdersInProgress.Add(o);
                        //else if (o.Status == "W dostawie") OrdersInDelivery.Add(o);
                        if (o.Status == "W dostawie") OrdersInDelivery.Add(o);
                        //else if (o.Status == "Zakończono") OrdersDone.Add(o);
                        //else if (o.Status == "Anulowane") OrdersDenied.Add(o);
                    }
                }
            }
            catch (Exception) { throw; }


            IsRefreshing = false;
        }

        async Task ExecuteLoadOrdersCommand4() {
            IsRefreshing = true;

            try {
                //Orders.Clear();
                //OrdersInProgress.Clear();
                //OrdersInDelivery.Clear();
                OrdersDone.Clear();
                //OrdersDenied.Clear();

                List<Order> orders = new List<Order>();
                if (App.User.Role == "Client") {
                    orders = await App.Database.GetOrdersForUserAsync();
                    foreach (var o in orders) {
                        o.User = await App.Database.GetUserAsync(o.UserID);
                        o.Delivery = await App.Database.GetDeliveryFromOrderAsync(o.DeliveryID);
                        //o.TotalCost = (int)App.Database.GetOrderProductsAsync(o.ID).Result.Sum(q => q.TotalCostForProduct);

                        var v = await App.Database.GetOrderProductsAsync(o.ID);
                        o.TotalCost = (int)v.Sum(q => q.TotalCostForProduct);

                        //Orders.Add(o);
                    }

                    foreach (var o in orders) {
                        //if (o.Status == "Złożono") Orders.Add(o);
                        //else if (o.Status == "W realizacji") OrdersInProgress.Add(o);
                        //else if (o.Status == "W dostawie") OrdersInDelivery.Add(o);
                        //else if (o.Status == "Zakończono") OrdersDone.Add(o);
                        if (o.Status == "Zakończono") OrdersDone.Add(o);
                        //else if (o.Status == "Anulowane") OrdersDenied.Add(o);
                    }

                }
                else {
                    orders = await App.Database.GetOrdersForEmployeeAsync();
                    foreach (var o in orders) {
                        o.User = await App.Database.GetUserAsync(o.UserID);
                        o.Delivery = await App.Database.GetDeliveryFromOrderAsync(o.DeliveryID);
                        var v = await App.Database.GetOrderProductsAsync(o.ID);
                        o.TotalCost = (int)v.Sum(q => q.TotalCostForProduct);

                    }

                    foreach (var o in orders) {
                        //if (o.Status == "Złożono") Orders.Add(o);
                        //else if (o.Status == "W realizacji") OrdersInProgress.Add(o);
                        //else if (o.Status == "W dostawie") OrdersInDelivery.Add(o);
                        //else if (o.Status == "Zakończono") OrdersDone.Add(o);
                        if (o.Status == "Zakończono") OrdersDone.Add(o);
                        //else if (o.Status == "Anulowane") OrdersDenied.Add(o);
                    }
                }
            }
            catch (Exception) { throw; }


            IsRefreshing = false;
        }

        async Task ExecuteLoadOrdersCommand5() {
            IsRefreshing = true;

            try {
                //Orders.Clear();
                //OrdersInProgress.Clear();
                //OrdersInDelivery.Clear();
                //OrdersDone.Clear();
                OrdersDenied.Clear();

                List<Order> orders = new List<Order>();
                if (App.User.Role == "Client") {
                    orders = await App.Database.GetOrdersForUserAsync();
                    foreach (var o in orders) {
                        o.User = await App.Database.GetUserAsync(o.UserID);
                        o.Delivery = await App.Database.GetDeliveryFromOrderAsync(o.DeliveryID);
                        //o.TotalCost = (int)App.Database.GetOrderProductsAsync(o.ID).Result.Sum(q => q.TotalCostForProduct);

                        var v = await App.Database.GetOrderProductsAsync(o.ID);
                        o.TotalCost = (int)v.Sum(q => q.TotalCostForProduct);

                        //Orders.Add(o);
                    }

                    foreach (var o in orders) {
                        //if (o.Status == "Złożono") Orders.Add(o);
                        //else if (o.Status == "W realizacji") OrdersInProgress.Add(o);
                        //else if (o.Status == "W dostawie") OrdersInDelivery.Add(o);
                        //else if (o.Status == "Zakończono") OrdersDone.Add(o);
                        //else if (o.Status == "Anulowane") OrdersDenied.Add(o);
                        if (o.Status == "Anulowane") OrdersDenied.Add(o);
                    }

                }
                else {
                    orders = await App.Database.GetOrdersForEmployeeAsync();
                    foreach (var o in orders) {
                        o.User = await App.Database.GetUserAsync(o.UserID);
                        o.Delivery = await App.Database.GetDeliveryFromOrderAsync(o.DeliveryID);
                        var v = await App.Database.GetOrderProductsAsync(o.ID);
                        o.TotalCost = (int)v.Sum(q => q.TotalCostForProduct);

                    }

                    foreach (var o in orders) {
                        //if (o.Status == "Złożono") Orders.Add(o);
                        //else if (o.Status == "W realizacji") OrdersInProgress.Add(o);
                        //else if (o.Status == "W dostawie") OrdersInDelivery.Add(o);
                        //else if (o.Status == "Zakończono") OrdersDone.Add(o);
                        //else if (o.Status == "Anulowane") OrdersDenied.Add(o);
                        if (o.Status == "Anulowane") OrdersDenied.Add(o);
                    }
                }
            }
            catch (Exception) { throw; }


            IsRefreshing = false;
        }

    }
}

using Dystrybux.Model;
using Dystrybux.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Dystrybux.ViewModel {
    public class OrderViewModel : BaseViewModel{

        private bool _IsRefreshing = false;
        public ObservableCollection<Order> Orders { get; }

        public OrderViewModel() {
            Orders = new ObservableCollection<Order>();

            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            CreateOrderCommand = new Command(async () => await NewOrderCommand());
            OrderTapped = new Command<Order>(OnOrderSelected);
        }

        async Task ExecuteLoadItemsCommand() {
            IsRefreshing = true;
            try {
                Orders.Clear();
                //var orders = await OrderStore.GetItemsAsync(true);
                List<Order> orders = null;
                //var orders = await App.Database.GetOrdersAsync();
                
                /*if(App.User.Role == "Client")
                    orders = await App.Database.GetOrdersAsync();
                else*/
                if(App.User.Role == "Client"){
                    orders = await App.Database.GetOrdersForUserAsync("Nie złożono");
                }
                else{
                    orders = await App.Database.GetOrdersForEmployeeAsync("Nie złożono");
                }

                foreach (var o in orders) {
                    Orders.Add(o);
                }
            }
            catch (Exception) {

                throw;
            }
            IsRefreshing = false;
        }

        async Task NewOrderCommand() {
            string orderName = "";

            Device.BeginInvokeOnMainThread(async () => {
                orderName = await App.Current.MainPage.DisplayPromptAsync("Nazwa zamówienia", "Podaj nazwę zamówienia");
                if (!string.IsNullOrEmpty(orderName) && !string.IsNullOrWhiteSpace(orderName)) {
                    await App.Navigation.PushAsync(new NewOrderPage(orderName));
                    // Przejscie do zlozonego zamowienia
                    //await App.Navigation.PushAsync(new NewOrderPage(orderName));
                }
            });


            /*if (!string.IsNullOrEmpty(orderName) && !string.IsNullOrWhiteSpace(orderName)) {
                Device.BeginInvokeOnMainThread(async () => {
                    await App.Current.MainPage.DisplayAlert("test", orderName, "ok");
                });
            }
            else {
                Device.BeginInvokeOnMainThread(async () => {
                    await App.Current.MainPage.DisplayAlert("test", "pusta nazwa", "ok");
                });
            }*/


        }

        private async void OnOrderSelected(Order order) {
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

        public Command LoadItemsCommand { protected set; get; }
        public Command CreateOrderCommand { protected set; get; }
        public Command OrderTapped { protected set; get; }
    }
}

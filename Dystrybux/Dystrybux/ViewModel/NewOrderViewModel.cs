using Dystrybux.Model;
using Dystrybux.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Dystrybux.ViewModel {
    public class NewOrderViewModel: BaseViewModel {
        private Order _order;
        private Product _addedProduct;
        private bool _IsRefreshing = false;

        public ObservableCollection<Product> AddedProducts { get; }

        public NewOrderViewModel(string orderName) {
            AddedProducts = new ObservableCollection<Product>();
            
            SearchProductCommand = new Command(async () => await App.Navigation.PushAsync(new TestPage(true, _order)));
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            CancelOrderCommand = new Command(async() => await App.Current.MainPage.DisplayAlert("Result", "Anuluj zamówienie", "OK"));
            SubmitOrderCommand = new Command(async() => await App.Current.MainPage.DisplayAlert("Result", "Wyślij zamówienie", "OK"));

            var date = DateTime.Now;

            /*Device.BeginInvokeOnMainThread(async () => {
                orderName = await App.Current.MainPage.DisplayPromptAsync("Nazwa zamówienia", "Podaj nazwę zamówienia");
            });*/

            _order = new Order {
                Name = orderName + '_' + date.Year + "/" + date.Month + "/" + date.Day,
                OrderedDate = null,
                Products = new List<Product>() { },
                Status = "Nie złożono"
            };

            App.Database.SaveOrderAsync(_order);
        }

        public NewOrderViewModel(Order order) {
            AddedProducts = new ObservableCollection<Product>();

            SearchProductCommand = new Command(async () => await App.Navigation.PushAsync(new TestPage(true, _order)));
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            CancelOrderCommand = new Command(async () => await App.Current.MainPage.DisplayAlert("Result", "Anuluj zamówienie", "OK"));
            SubmitOrderCommand = new Command(async () => await App.Current.MainPage.DisplayAlert("Result", "Wyślij zamówienie", "OK"));

            _order = order;
        }

        async Task ExecuteLoadItemsCommand() {
            IsRefreshing = true;
            try {
                AddedProducts.Clear();
                
                //AddedProducts = products;

                //var order = await OrderStore.GetItemAsync(3);
                //var order = await App.Database.GetOrdersAsync();

                //AddedProducts = order.Products.GetEnumerator();

                //AddedProducts = await App.Database.GetOrderProductsAsync(_order.ID);

                var items = await App.Database.GetOrderProductsAsync(_order.ID);

                foreach (var p in items) {
                    //AddedProducts.Add(await App.Database.GetProductAsync(p.ProductID));
                    AddedProducts.Add(p.Product);
                    //AddedProducts.Add(p);
                }
            }
            catch (Exception) {

                throw;
            }
            IsRefreshing = false;
        }

        public Product AddedProduct {
            get => _addedProduct;
            set {
                _addedProduct = value;
            }
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

        public Command SearchProductCommand { protected set; get; }
        public Command LoadItemsCommand { protected set; get; }
        public Command CancelOrderCommand { protected set; get; }
        public Command SubmitOrderCommand { protected set; get; }
    }
}

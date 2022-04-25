using Dystrybux.Model;
using Dystrybux.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Dystrybux.ViewModel {
    public class NewOrderViewModel: BaseViewModel {
        private Order _order;
        private Product _addedProduct;
        private bool _IsRefreshing = false;

        private DateTime _firstDate = DateTime.Now;
        private DateTime _secondDate = DateTime.Now;

        public ObservableCollection<Product> AddedProducts { get; }

        public NewOrderViewModel(string orderName) {
            AddedProducts = new ObservableCollection<Product>();
            
            SearchProductCommand = new Command(async () => await App.Navigation.PushAsync(new TestPage(true, _order)));
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            CancelOrderCommand = new Command(async() => await App.Current.MainPage.DisplayAlert("Result", "Anuluj zamówienie", "OK"));
            SubmitOrderCommand = new Command(async() => await App.Current.MainPage.DisplayAlert("Result", "Wyślij zamówienie", "OK"));
            SaveOrderCommand = new Command(async () => await SaveOrder());

            var date = DateTime.Now;

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
            SaveOrderCommand = new Command(async () => await SaveOrder());

            _order = order;

            FirstDate = DateTime.Parse(order.FirstDate);
            SecondDate = DateTime.Parse(order.SecondDate);

            //FirstDate = DateTime.ParseExact(order.FirstDate, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
            //SecondDate = DateTime.ParseExact(order.SecondDate, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);

            //DateTime d;
            //FirstDate = DateTime.TryParse(order.FirstDate, "dd-MM-yyyy hh:mm:ss", DateTimeStyles.None, out d);
            //SecondDate = DateTime.TryParse(order.SecondDate, "dd MM yyyy", CultureInfo.InvariantCulture);

            /*Device.BeginInvokeOnMainThread(async () => {
                //await App.Current.MainPage.DisplayAlert("Result", FirstDate.ToString(), "OK");
                await App.Current.MainPage.DisplayAlert("Result", _order.FirstDate, "OK");
            });*/


        }

        async Task ExecuteLoadItemsCommand() {
            IsRefreshing = true;
            try {
                AddedProducts.Clear();

                var items = await App.Database.GetOrderProductsAsync(_order.ID);

                foreach (var p in items) {
                    //AddedProducts.Add(await App.Database.GetProductAsync(p.ProductID));
                    AddedProducts.Add(p.Product);
                }
            }
            catch (Exception) { throw; }
            IsRefreshing = false;
        }

        async Task SaveOrder() {
            try {
                //_order.FirstDate = FirstDate.ToString("dd'.'MM'.'yyyy", CultureInfo.InvariantCulture);
                _order.FirstDate = FirstDate.ToString();
                //_order.SecondDate = SecondDate.ToString("dd'.'MM'.'yyyy", CultureInfo.InvariantCulture);
                _order.SecondDate = SecondDate.ToString();
                await App.Database.UpdateOrderAsync(_order);
            }
            catch (Exception) { throw; }
        }

        public Product AddedProduct {
            get => _addedProduct;
            set { _addedProduct = value; }
        }

        public DateTime FirstDate {
            get => _firstDate;
            set => _firstDate = value;
        }

        public DateTime SecondDate {
            get => _secondDate;
            set => _secondDate = value;
        }

        public void OnAppearing() { IsRefreshing = true; }

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
        public Command SaveOrderCommand { protected set; get; }
    }
}

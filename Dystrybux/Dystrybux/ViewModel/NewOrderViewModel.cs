﻿using Dystrybux.Model;
using Dystrybux.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Dystrybux.ViewModel {
    public class NewOrderViewModel: BaseViewModel {
        private Order _order;
        private Product _addedProduct;
        private bool _IsRefreshing = false;

        private bool _IsClient = true;
        private bool _IsBusiness = true;

        private DateTime _firstDate = DateTime.Now;
        private DateTime _secondDate = DateTime.Now;

        private int _totalCostProduct = 0;
        private int _countOfProduct = 0;

        //private int _countOfProduct = 0;

        public ObservableCollection<Product> AddedProducts { get; }
        //public ObservableCollection<OrderProduct> AddedProductsFromOrder { get; }
        /*public ObservableCollection<OrderProduct> AddedProductsFromOrder {
            get => _AddedProductsFromOrder;
            set => _AddedProductsFromOrder = value;
        }*/

        public ObservableCollection<OrderProduct> AddedProductsFromOrder { get; set; }

        //public static ObservableCollection<OrderProduct> _AddedProductsFromOrder;

        public NewOrderViewModel(string orderName) {
            /*AddedProducts = new ObservableCollection<Product>();
            
            SearchProductCommand = new Command(async () => await App.Navigation.PushAsync(new ProductPage(true, _order)));
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            CancelOrderCommand = new Command(async () => await App.Current.MainPage.DisplayAlert("Result", "Anuluj zamówienie", "OK"));
            SubmitOrderCommand = new Command(async () => await App.Current.MainPage.DisplayAlert("Result", "Wyślij zamówienie", "OK"));
            SaveOrderCommand = new Command(async () => await SaveOrder());
            //SetCount = new Command(() => { TotalCostProduct = 1 * AddedProducts[0].Cost; });

            var date = DateTime.Now;

            _order = new Order {
                Name = orderName + '_' + date.Year + "/" + date.Month + "/" + date.Day,
                OrderedDate = null,
                FirstDate = null,
                SecondDate = null,
                Products = new List<Product>() { },
                Status = "Nie złożono"
            };
            
            App.Database.SaveOrderAsync(_order);*/
        }

        public NewOrderViewModel(Order order) {
            if(App.User.Role == "Business") { IsBusiness = true; IsClient = false; }
            else { IsBusiness = false; IsClient = true; }
            AddedProducts = new ObservableCollection<Product>();
            AddedProductsFromOrder = new ObservableCollection<OrderProduct>();

            SearchProductCommand = new Command(async () => await App.Navigation.PushAsync(new ProductPage(true, _order)));
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            CancelOrderCommand = new Command(() => CancelOrder());
            SubmitOrderCommand = new Command(() => SubmitOrder());
            SaveOrderCommand = new Command(async () => await SaveOrder());

            DiscardOrderCommand = new Command(async () => await App.Current.MainPage.DisplayAlert("Result", "Odrzuć zamówienie", "OK"));
            AcceptOrderCommand = new Command(async () => await App.Current.MainPage.DisplayAlert("Result", "Przyjąć zamówienie", "OK"));

            IncrementCountCommand = new Command(obj => IncrementCount((int)obj));
            /*IncrementCountCommand = new Command(()=>{
                Device.BeginInvokeOnMainThread(async () => {
                    await App.Current.MainPage.DisplayAlert("Result", 1.ToString(), "OK");
                });
            });*/

            //DecrementCountCommand = new Command(() => DecrementCount());
            DecrementCountCommand = new Command(obj => DecrementCount((int)obj));

            _order = order;

            if(order.FirstDate == null || SecondDate == null) {
                FirstDate = DateTime.Now;
                SecondDate = DateTime.Now;
            }
            else {
                FirstDate = DateTime.Parse(order.FirstDate);
                SecondDate = DateTime.Parse(order.SecondDate);
            }


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
                AddedProductsFromOrder.Clear();

                var items = await App.Database.GetOrderProductsAsync(_order.ID);

                foreach (var p in items) {
                    //AddedProducts.Add(await App.Database.GetProductAsync(p.ProductID));
                    AddedProducts.Add(p.Product);
                    //p.Product = App.Database.GetProductAsync(p.ProductID).Result;
                    AddedProductsFromOrder.Add(p);
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

        void SubmitOrder() {
            Device.BeginInvokeOnMainThread(async () => {
                bool choice = await App.Current.MainPage.DisplayAlert("", "Czy złożyć zamówienie do realizacji?", "Tak", "Nie");
                if (choice) {
                    try {
                        _order.Name = new Random().Next(10000, 99999).ToString() + new Random().Next(10000, 99999).ToString();
                        _order.Status = "Złożono";
                        _order.OrderedDate = DateTime.Now.ToString();
                        await App.Database.UpdateOrderAsync(_order);
                    }
                    catch (Exception) { throw; }
                }
            });
        }

        void CancelOrder() {
            Device.BeginInvokeOnMainThread(async () => {
                bool choice = await App.Current.MainPage.DisplayAlert("", "Czy anulować zamówienie?", "Tak", "Nie");
                if (choice) {
                    try {
                        await App.Database.DeleteOrderAsync(_order);
                        await App.Navigation.PopAsync();
                    }
                    catch (Exception) { throw; }
                }
            });
        }

        public void SetCountCommand(int val) {
            TotalCostProduct = val * AddedProducts[0].Cost;
            /*Device.BeginInvokeOnMainThread(async () => {
                await App.Current.MainPage.DisplayAlert("Result", "test", "OK");
            });*/
        }

        void IncrementCount(int ID) {

            var item = AddedProductsFromOrder.Where(q => q.ID == ID).FirstOrDefault();
            item.CountOfProducts += 1;
            /*Device.BeginInvokeOnMainThread(async () => {
                await App.Current.MainPage.DisplayAlert("Result", item.CountOfProducts.ToString(), "OK");
            });*/

            AddedProductsFromOrder[ID-1] = item;
            App.Database.UpdateProductOrderAsync(item);
            //OnPropertyChanged();
            //_ = ExecuteLoadItemsCommand();
        }

        void DecrementCount(int ID) {
            var item = AddedProductsFromOrder.Where(q => q.ID == ID).FirstOrDefault();
            if(item.CountOfProducts > 1) {
                item.CountOfProducts -= 1;
                /*Device.BeginInvokeOnMainThread(async () => {
                    await App.Current.MainPage.DisplayAlert("Result", item.CountOfProducts.ToString(), "OK");
                });*/

                App.Database.UpdateProductOrderAsync(item);
                AddedProductsFromOrder[ID-1] = item;
                //_ = ExecuteLoadItemsCommand();
                //OnPropertyChanged();
            }
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

        public bool IsClient {
            get => _IsClient;
            set => _IsClient = value;
        }

        public bool IsBusiness {
            get => _IsBusiness;
            set => _IsBusiness = value;
        }

        public int TotalCostProduct {
            get => _totalCostProduct;
            set => _totalCostProduct = value;
        }

        public int CountOfProduct {
            get => _countOfProduct;
            set {
                _countOfProduct = value;
                //SetCountCommand(_countOfProduct);
            }
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
        public Command DiscardOrderCommand { protected set; get; }
        public Command AcceptOrderCommand { protected set; get; }
        public Command SetCount { protected set; get; }
        public Command IncrementCountCommand { protected set; get; }
        public Command DecrementCountCommand { protected set; get; }
        
    }
}

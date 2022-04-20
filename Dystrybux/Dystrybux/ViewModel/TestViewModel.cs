using Dystrybux.Model;
using Dystrybux.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Dystrybux.ViewModel
{
    public class TestViewModel: BaseViewModel{
        private Product _selectedProduct;
        private Order _selectedOrder;
        private bool _IsRefreshing = false;
        private bool _IsSearching = false;
        private string _SearchProductsName = "";

        public ObservableCollection<Product> Products { get; }

        // search bar
        // https://docs.microsoft.com/pl-pl/xamarin/xamarin-forms/user-interface/searchbar

        public TestViewModel(){
            Products = new ObservableCollection<Product>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand(null));
            ProductTapped = new Command<Product>(OnProductSelected);
            SearchProductsCommand = new Command(async () => await ExecuteLoadItemsCommand(SearchProducts));
        }

        public TestViewModel(bool isSearch, Order order) {
            Products = new ObservableCollection<Product>();
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand(null));
            ProductTapped = new Command<Product>(OnProductSelected);
            SearchProductsCommand = new Command(async () => await ExecuteLoadItemsCommand(SearchProducts));
            _IsSearching = isSearch;
            _selectedOrder = order;
        }

        async Task ExecuteLoadItemsCommand(string productName){
            IsRefreshing = true;
            try{
                Products.Clear();
                //var products = await ProductStore.GetItemsAsync(true);
                var products = new List<Product>();
                /*Device.BeginInvokeOnMainThread(async () => {
                    await App.Current.MainPage.DisplayAlert("Result",  productName , "OK");
                });*/
                if (!string.IsNullOrEmpty(productName) && !string.IsNullOrWhiteSpace(productName)) {
                    products = await App.Database.GetProductsAsync(productName);
                }
                else {
                    products = await App.Database.GetProductsAsync();
                }
                foreach (var p in products){
                    Products.Add(p);
                }
            }
            catch (Exception){
                throw;
            }
            IsRefreshing = false;
        }

        async Task OnSearchedProductName(object sender, EventArgs e) {

        }

        async Task ExecuteLoadSearchedItems(string productName) {
            IsRefreshing = true;
            try {
                Products.Clear();
                //var products = await ProductStore.GetItemsAsync(true);
                var products = await App.Database.GetProductsAsync(productName);
                foreach (var p in products) {
                    Products.Add(p);
                }
            }
            catch (Exception) {

                throw;
            }
            IsRefreshing = false;
        }

        public void OnAppearing()
        {
            IsRefreshing = true;
        }

        public Product SelectedProduct {
            get => _selectedProduct;
            set {
                _selectedProduct = value;
                OnProductSelected(value);
            }
        }

        public string SearchProducts {
            get => _SearchProductsName;
            set {
                _SearchProductsName = value;
            }
        }

        private async void OnProductSelected(Product product) {
            if (product == null)
                return;

            if (_IsSearching) {
                //bool choice;
                Device.BeginInvokeOnMainThread(async () => {
                    bool choice = await App.Current.MainPage.DisplayAlert("", "Czy dodać produkt do zamówienia?", "Tak", "Nie");
                    if (choice) {
                        //var order = await OrderStore.GetItemAsync(3);
                        //var order = await OrderStore.GetItemAsync(3);

                        //var order = await App.Database.GetOrderAsync(1);

                        //order.Products = new List<Product>() { product };

                        await App.Database.SaveProductOrderAsync(_selectedOrder, product);

                        //order.Products = new List<Product>(){ product };

                        await App.Navigation.PopAsync();
                    }
                    
                    //await App.Navigation.PopAsync();
                });


            }
            else {
                await App.Navigation.PushAsync(new ProductDetailPage(product));
            }
        }


        public bool IsRefreshing
        {
            set
            {
                _IsRefreshing = value;
                OnPropertyChanged();
            }
            get => _IsRefreshing;
        }

        public Command LoadItemsCommand { protected set; get; }
        public Command<Product> ProductTapped { protected set; get; }
        public Command SearchProductsCommand { protected set; get; }
    }
}

using Dystrybux.Model;
using Dystrybux.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Dystrybux.ViewModel
{
    public class ProductViewModel : BaseViewModel{
        private Product _selectedProduct;
        private Order _selectedOrder;
        private bool _IsRefreshing = false;
        private bool _IsSearching = false;
        private string _SearchProductsName = "";
        private bool _IsBusiness = false;
        List<string> sciezki = new List<string>();

        public ObservableCollection<Product> Products { get; }

        // search bar
        // https://docs.microsoft.com/pl-pl/xamarin/xamarin-forms/user-interface/searchbar

        public ProductViewModel(){
            Products = new ObservableCollection<Product>();
            AddItemCommand = new Command(async () => { await App.Navigation.PushAsync(new NewItemPage()); });
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            ProductTapped = new Command<Product>(OnProductSelected);
            SearchProductsCommand = new Command(async () => await ExecuteLoadSearchedItems(SearchProducts));
            DetailsOrderCommand = new Command(async () => await DetailsOrder());

            IsBusiness = App.User.Role == "Business" ? true : false;

            foreach (var fileName in System.IO.Directory.GetFiles("/storage/emulated/0/Pictures/Dystrybux.Android")) {
                sciezki.Add(fileName);
            }

            /*Device.BeginInvokeOnMainThread(async () => {
                await App.Current.MainPage.DisplayAlert("Result", sciezki[0], "OK");
            });*/

        }

        public ProductViewModel(bool isSearch, Order order) {
            Products = new ObservableCollection<Product>();
            AddItemCommand = new Command(async () => { await App.Navigation.PushAsync(new NewItemPage()); });
            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());
            ProductTapped = new Command<Product>(OnProductSelected);
            SearchProductsCommand = new Command(async () => await ExecuteLoadSearchedItems(SearchProducts));

            DetailsOrderCommand = new Command(() => {
                Device.BeginInvokeOnMainThread(async () => {
                    await App.Current.MainPage.DisplayAlert("Result", "zamówienie", "OK");
                });
            });

            IsBusiness = App.User.Role == "Business" ? true : false;
            _IsSearching = isSearch;
            _selectedOrder = order;

            foreach (var fileName in System.IO.Directory.GetFiles("/storage/emulated/0/Pictures/Dystrybux.Android")) {
                sciezki.Add(fileName);
            }

        }

        async Task ExecuteLoadItemsCommand(){
            IsRefreshing = true;
            try{
                Products.Clear();
                var products = new List<Product>();
                products = await App.Database.GetProductsAsync();
                foreach (var p in products){
                    string imagePath = sciezki.Where(q => q.Contains(p.ImagePath)).FirstOrDefault();

                    /*if (p.ID == 2) {
                        Device.BeginInvokeOnMainThread(async () => {
                            await App.Current.MainPage.DisplayAlert("Result", p.ImagePath, "OK");
                        });
                    }*/
                    

                    if (!string.IsNullOrEmpty(imagePath)) {
                        p.Image = (Device.RuntimePlatform == Device.Android) ?
                            ImageSource.FromFile(imagePath) :
                            ImageSource.FromFile("/storage/emulated/0/Pictures/Dystrybux.Android/productImage_1704_495_20220606_105829.png");
                    }
                    Products.Add(p);

                    // "/storage/emulated/0/Pictures/Dystrybux.Android/productImage_6309_9742_20220606_101827.png"

                }
            }
            catch (Exception){ throw; }
            IsRefreshing = false;
        }

        async Task ExecuteLoadSearchedItems(string productName) {
            try {
                Products.Clear();
                var products = await App.Database.GetProductsAsync(productName);
                foreach (var p in products) { Products.Add(p); }
            }
            catch (Exception) { throw; }
        }

        async Task DetailsOrder() {
            var order = await App.Database.GetUndoneOrderAsync("Nie złożono");
            if (order != null) {
                await App.Navigation.PushAsync(new NewOrderPage(order));
            }
            else {
                Device.BeginInvokeOnMainThread(async () => {
                    await App.Current.MainPage.DisplayAlert("Result", "Zamówienei nie istnieje, dodaj produkt, aby utworzyć", "OK");
                });
            }
        }

        public void OnAppearing(){
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

        public bool IsBusiness
        {
            get => _IsBusiness;
            set => _IsBusiness = value;
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

                        //product.Count -= 1;
                        //await App.Database.UpdateProductAsync(product);
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


        public bool IsRefreshing{
            set{
                _IsRefreshing = value;
                OnPropertyChanged();
            }
            get => _IsRefreshing;
        }

        public Command AddItemCommand { protected set; get; }
        public Command LoadItemsCommand { protected set; get; }
        public Command<Product> ProductTapped { protected set; get; }
        public Command SearchProductsCommand { protected set; get; }
        public Command DetailsOrderCommand { protected set; get; }
    }
}

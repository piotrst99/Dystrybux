using Dystrybux.Model;
using Dystrybux.View;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Dystrybux.ViewModel {
    public class ProductViewModel : BaseViewModel {
        private Product _selectedProduct;
        private bool _IsRefreshing = false;

        public ObservableCollection<Product> Products { get; }

        public ProductViewModel() {
            Products = new ObservableCollection<Product>();
    
            AddItemCommand = new Command(async () => {
                await App.Navigation.PushAsync(new NewItemPage());
            });

            LoadItemsCommand = new Command(async () => await ExecuteLoadItemsCommand());

            ProductTapped = new Command<Product>(OnProductSelected);
        }

        async Task ExecuteLoadItemsCommand() {
            IsRefreshing = true;
            try {
                Products.Clear();
                //var products = await ProductStore.GetItemsAsync(true);

                var products = await App.Database.GetProductsAsync();

                foreach (var p in products) {
                    Products.Add(p);
                }
            }
            catch (Exception) {

                throw;
            }
            IsRefreshing = false;
        }

        public void OnAppearing() {
            IsRefreshing = true;
        }

        public Product SelectedProduct {
            get => _selectedProduct;
            set {
                _selectedProduct = value;
                OnProductSelected(value);
            }
        }

        private async void OnProductSelected(Product product) {
            if (product == null)
                return;

            await App.Navigation.PushAsync(new ProductDetailPage(product));
        }

        public bool IsRefreshing {
            set{ 
                _IsRefreshing = value;
                OnPropertyChanged();
            }
            get => _IsRefreshing;
        }

        public Command AddItemCommand { protected set; get; }
        public Command LoadItemsCommand { protected set; get; }
        public Command<Product> ProductTapped { protected set; get; }
    }
}

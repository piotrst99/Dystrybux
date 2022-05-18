using Dystrybux.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Dystrybux.ViewModel {
    public class ProductDetailViewModel: BaseViewModel {
        Product _product = null;
        string _name = "";
        string _description = "";
        int _cost = 0;
        int _count = 0;

        bool _isWatching = true;
        bool _isEdit = false;
        bool _IsClient = false;

        public ProductDetailViewModel(Product product) {
            _product = product;
            DeleteItemCommand = new Command(async () => await DeleteItem());
            EditItemCommand = new Command(() => EditItem());
            SaveDataCommand = new Command(async () => await SaveData());

            AddProductToOrderCommand = new Command(() => AddProductToOrder());

            Name = product.Name;
            Description = product.Description;
            Cost = product.Cost;
            Count = product.Count;

            _IsClient = App.User.Role == "Client" ? true : false;

        }

        async Task DeleteItem() {
            bool choice = await App.Current.MainPage.DisplayAlert("Uwaga", "Czy usunąć produkt?", "Tak", "Nie");
            if (choice) {
                await App.Database.DeleteProductAsync(_product);
                await App.Navigation.PopAsync();
            }
        }

        void AddProductToOrder() {

            Device.BeginInvokeOnMainThread(async () => {
                bool choice = await App.Current.MainPage.DisplayAlert("", "Czy dodać produkt do zamówienia?", "Tak", "Nie");
                if (choice) {

                    var order = await App.Database.GetUndoneOrderAsync("Nie złożono");
                    
                    if (order == null) {
                        // create new order
                        var newOrder = new Order {
                            Name = "",
                            OrderedDate = null,
                            FirstDate = null,
                            SecondDate = null,
                            Products = new List<Product>() { },
                            Status = "Nie złożono"
                        };

                        _product.Count -= 1;
                        await App.Database.UpdateProductAsync(_product);

                        await App.Database.SaveOrderAsync(newOrder);
                        await App.Database.SaveProductOrderAsync(newOrder, _product);
                    }
                    else{
                        // add to undone order

                        var productFromOrder = await App.Database.GetProductFromOrderAsync(order.ID, _product.ID);

                        if(productFromOrder == null) {
                            _product.Count -= 1;
                            await App.Database.UpdateProductAsync(_product);
                            await App.Database.SaveProductOrderAsync(order, _product);
                        }
                        else {
                            Device.BeginInvokeOnMainThread(async () => {
                                await App.Current.MainPage.DisplayAlert("Result", "Produkt został dodany do zamówienia", "OK");
                            });
                        }

                        


                        /*_product.Count -= 1;
                        await App.Database.UpdateProductAsync(_product);
                        await App.Database.SaveProductOrderAsync(order, _product);*/

                    }
                    

                    //var order = await OrderStore.GetItemAsync(3);
                    //var order = await OrderStore.GetItemAsync(3);

                    //var order = await App.Database.GetOrderAsync(1);

                    //order.Products = new List<Product>() { product };

                    //await App.Database.SaveProductOrderAsync(_selectedOrder, _product);

                    //order.Products = new List<Product>(){ product };

                    //await App.Navigation.PopAsync();
                }

                //await App.Navigation.PopAsync();
            });

        }

        void EditItem() {
            IsEdit = true;
            IsWatching = false;
        }

        async Task SaveData() {
            IsEdit = false;
            IsWatching = true;

            _product.Name = Name;
            _product.Description = Description;
            _product.Cost = Cost;
            _product.Count = Count;

            await App.Database.UpdateProductAsync(_product);
        }

        public string Name {
            get => _name;
            set {
                _name = value;
                OnPropertyChanged();
            }
        }

        public string Description {
            get => _description;
            set {
                _description = value;
                OnPropertyChanged();
            }
        }

        public int Cost {
            get => _cost;
            set {
                _cost = value;
                OnPropertyChanged();
            }
        }

        public int Count {
            get => _count;
            set {
                _count = value;
                OnPropertyChanged();
            }
        }

        public bool IsWatching {
            get => _isWatching;
            set {
                _isWatching = value;
                OnPropertyChanged();
            }
        }

        public bool IsEdit {
            get => _isEdit;
            set {
                _isEdit = value;
                OnPropertyChanged();
            }
        }

        public bool IsClient {
            get => _IsClient;
            set => _IsClient = value;
        }

        public Command DeleteItemCommand { protected set; get; }
        public Command EditItemCommand { protected set; get; }
        public Command SaveDataCommand { protected set; get; }
        public Command AddProductToOrderCommand { protected set; get; }

    }
}

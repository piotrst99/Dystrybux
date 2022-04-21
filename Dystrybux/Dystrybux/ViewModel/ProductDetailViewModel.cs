using Dystrybux.Model;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Dystrybux.ViewModel {
    public class ProductDetailViewModel: BaseViewModel {
        string _name = "";
        string _description = "";
        int _cost = 0;
        int _count = 0;

        bool _isWatching = true;
        bool _isEdit = false;

        public ProductDetailViewModel(Product product) {

            DeleteItemCommand = new Command(async () => {
                bool choice = await App.Current.MainPage.DisplayAlert("Uwaga", "Czy usunąć produkt?", "Tak", "Nie");
                if (choice) {
                    await App.Database.DeleteProductAsync(product);
                    await App.Navigation.PopAsync();
                }
            });
            //EditItemCommand = new Command(async () => await App.Current.MainPage.DisplayAlert("Result", "Edytuj dane produktu", "OK"));
            EditItemCommand = new Command(() => {
                IsEdit = true;
                IsWatching = false;
            });

            SaveDataCommand = new Command(async () => {
                IsEdit = false;
                IsWatching = true;

                product.Name = Name;
                product.Description = Description;
                product.Cost = Cost;
                product.Count = Count;

                await App.Database.UpdateProductAsync(product);

            });

            Name = product.Name;
            Description = product.Description;
            Cost = product.Cost;
            Count = product.Count;
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


        public Command DeleteItemCommand { protected set; get; }
        public Command EditItemCommand { protected set; get; }
        public Command SaveDataCommand { protected set; get; }

    }
}

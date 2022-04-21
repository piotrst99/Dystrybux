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

        public ProductDetailViewModel(Product product) {

            DeleteItemCommand = new Command(async () => await App.Current.MainPage.DisplayAlert("Result", "Usuń produkt", "OK"));
            EditItemCommand = new Command(async () => await App.Current.MainPage.DisplayAlert("Result", "Edytuj dane produktu", "OK"));
            
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

        public Command DeleteItemCommand { protected set; get; }
        public Command EditItemCommand { protected set; get; }

    }
}

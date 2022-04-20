using Dystrybux.Model;
using System;
using System.Collections.Generic;
using System.Text;

namespace Dystrybux.ViewModel {
    public class ProductDetailViewModel: BaseViewModel {
        string _name = "";
        string _description = "";
        int _cost = 0;
        int _count = 0;

        public ProductDetailViewModel(Product product) {
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

    }
}

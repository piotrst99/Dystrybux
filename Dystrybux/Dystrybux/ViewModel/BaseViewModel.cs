using Dystrybux.Model;
using Dystrybux.Service;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Dystrybux.ViewModel {
    public class BaseViewModel : INotifyPropertyChanged {
        public IDataStore<Product> ProductStore => DependencyService.Get<IDataStore<Product>>();
        public IDataStore<Order> OrderStore => DependencyService.Get<IDataStore<Order>>();
        public event PropertyChangedEventHandler PropertyChanged;


        /*public readonly ObservableCollection<Product> ProductsList = new ObservableCollection<Product>() {
            new Product{ ID=1, Name="Kremówka", Count=100, Cost=20, Description="Oryginalna z Wadowic" },
            new Product{ ID=2, Name="Ciasto", Count=10, Cost=50, Description="Słodka chwila" }
        };*/

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null) {
            PropertyChangedEventHandler handler = PropertyChanged;

            if (handler != null) {
                handler(this, new PropertyChangedEventArgs(propertyName));
            }

        }

    }
}

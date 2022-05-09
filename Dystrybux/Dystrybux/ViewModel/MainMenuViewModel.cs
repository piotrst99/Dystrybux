using Dystrybux.Model;
using Dystrybux.View;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Dystrybux.ViewModel {
    public class MainMenuViewModel : BaseViewModel {
        string _userName = "";

        public MainMenuViewModel(User user) {
            UserName = string.Format("{0} {1}", user.Name, user.Surname);

            ProductListCommand = new Command(async () =>  await App.Navigation.PushAsync(new ProductPage()));
            OrderListCommand = new Command(async () => await App.Navigation.PushAsync(new OrderPage()));
            SettingsCommand = new Command(async () => await App.Navigation.PushAsync(new SettingsPage()));
            TestCommand = new Command(async () => await App.Navigation.PushAsync(new TestPage()));
            DetailsOrderCommand = new Command(async () => {
                var order = await App.Database.GetUndoneOrderAsync("Nie złożono");
                if (order != null) {
                    await App.Navigation.PushAsync(new NewOrderPage(order));
                }
                else {
                    Device.BeginInvokeOnMainThread(async () => {
                        await App.Current.MainPage.DisplayAlert("Result", "Zamówienei nie istnieje, dodaj produkt, aby utworzyć", "OK");
                    });
                }
            });
        }

        public string UserName {
            set {
                if (_userName != value) {
                    _userName = value;
                    OnPropertyChanged();
                }
            }
            get {
                return _userName;
            }
        }

        public Command ProductListCommand { protected set; get; }
        public Command OrderListCommand { protected set; get; }
        public Command SettingsCommand { protected set; get; }
        public Command TestCommand { protected set; get; }
        public Command DetailsOrderCommand { protected set; get; }

    }
}

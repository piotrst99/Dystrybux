using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Dystrybux.ViewModel {
    public class TestViewModel : BaseViewModel {
        
        public TestViewModel() {
            CreateOrderCommand = new Command(() => {
                Device.BeginInvokeOnMainThread(async () => {
                    await App.Current.MainPage.DisplayAlert("Result", "zamówienie", "OK");
                });
            });
        }

        public Command CreateOrderCommand { protected set; get; }

    }
}

using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Dystrybux.ViewModel {
    public class SettingsViewModel : BaseViewModel {
        public SettingsViewModel() {
            LogOut = new Command(async () => {
                bool choice = await App.Current.MainPage.DisplayAlert("Uwaga", "Czy wylogować?", "Tak", "Nie");
                if (choice) {
                    await App.Navigation.PopToRootAsync();
                    App.User = null;
                }
            });
        }


        public Command LogOut { protected set; get; }
    }
}

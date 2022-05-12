using Dystrybux.View;
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

            UserDataCommand = new Command(async () => await App.Navigation.PushAsync(new UserDataPage()));

        }


        public Command LogOut { protected set; get; }
        public Command UserDataCommand { protected set; get; }
    }
}

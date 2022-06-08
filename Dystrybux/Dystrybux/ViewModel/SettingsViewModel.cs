using Dystrybux.View;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Dystrybux.ViewModel {
    public class SettingsViewModel : BaseViewModel {

        bool _IsClient = false;

        public SettingsViewModel() {
            LogOut = new Command(async () => {
                bool choice = await App.Current.MainPage.DisplayAlert("Uwaga", "Czy wylogować?", "Tak", "Nie");
                if (choice) {
                    await App.Navigation.PopToRootAsync();
                    App.User = null;
                }
            });

            UserDataCommand = new Command(async () => await App.Navigation.PushAsync(new UserDataPage()));
            PasswordCommand = new Command(async () => await App.Navigation.PushAsync(new PasswordPage()));
            CompanyDataCommand = new Command(async () => await App.Navigation.PushAsync(new CompanyDataPage()));

            IsClient = App.User.Role == "Client";

        }

        public bool IsClient {
            get => _IsClient;
            set => _IsClient = value;
        }

        public Command LogOut { protected set; get; }
        public Command UserDataCommand { protected set; get; }
        public Command PasswordCommand { protected set; get; }
        public Command CompanyDataCommand { protected set; get; }
    }
}

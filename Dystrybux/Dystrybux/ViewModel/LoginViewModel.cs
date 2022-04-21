using Dystrybux.Model;
using Dystrybux.View;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Dystrybux.ViewModel {
    public class LoginViewModel: BaseViewModel {
        string _login = "jan";
        string _password = "1234";
        string _loginResult = "";

        string _selectedRole = "Client";

        /*List<User> users = new List<User>(){
            new User{ID=1, Login ="jan", Password="1234", Name="Jan", Surname="Kowalski"},
            new User{ID=2, Login ="karol", Password="1234", Name="Karol", Surname="Kola"},
            new User{ID=3, Login ="maria", Password="1234", Name="Maria", Surname="Mała"}
        };*/

        public LoginViewModel() {

            this.LoginToAccount = new Command(async () => {
                //var user = users.Where(q => q.Login == this.Login && q.Password == this.Password).FirstOrDefault();

                var user = await App.Database.GetUserAsync(_login, _password, _selectedRole);

                if (user !=null) {
                    LoginResult = "";
                    App.User = user;
                    await App.Navigation.PushAsync(new MainMenu(user));
                }
                else {
                    LoginResult = "Nieprawidłowy login lub hasło!";
                }
            });

            CreateAccount = new Command(async () => { await App.Navigation.PushAsync(new RegisterPage()); });

        }

        /*void SelectedRole(object sender, CheckedChangedEventArgs e) {
            Device.BeginInvokeOnMainThread(async () => {
                await App.Current.MainPage.DisplayAlert("Result", e.Value.ToString(), "OK");
            });
        }*/

        public string SelectedRole {
            get => _selectedRole;
            set {
                _selectedRole = value;
                /*Device.BeginInvokeOnMainThread(async () => {
                    await App.Current.MainPage.DisplayAlert("Result", value.ToString(), "OK");
                });*/
            }
        }

        public string Login{
            set {
                if(_login != value) {
                    _login = value;
                }
            }
            get {
                return _login;
            }
        }

        public string Password {
            set {
                if(_password != value) {
                    _password = value;
                }
            }
            get {
                return _password;
            }
        }

        public string LoginResult{
            set {
                if(_loginResult != value) {
                    _loginResult = value;
                    OnPropertyChanged();
                }
            }
            get {
                return _loginResult;
            }
        }

        public ICommand LoginToAccount { protected set; get; }
        public ICommand CreateAccount { protected set; get; }

    }
}

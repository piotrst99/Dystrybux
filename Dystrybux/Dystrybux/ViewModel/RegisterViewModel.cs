using Dystrybux.Model;
using System;
using System.Collections.Generic;
using System.Text;
using System.Windows.Input;
using Xamarin.Forms;

namespace Dystrybux.ViewModel {
    public class RegisterViewModel : BaseViewModel{
        string _email = "";
        string _login = "";
        string _password = "";
        string _name = "";
        string _surname = "";
        string _selectedRole = "Client";

        public RegisterViewModel() {
            CreateAccount = new Command(async () => {
                if(_password.Length < 4 || _login.Length < 3 || string.IsNullOrEmpty(_email) || _name.Length < 3 || _surname.Length < 3) {
                    Device.BeginInvokeOnMainThread(async () => {
                        await App.Current.MainPage.DisplayAlert("Ostrzeżenie", "Niepoprawne dane!", "OK");
                    });
                }
                else {

                    var user = await App.Database.GetUserAsync(Login);

                    if (user == null) {
                        /*Device.BeginInvokeOnMainThread(async () => {
                            await App.Current.MainPage.DisplayAlert("Result", App.Database.GetCountOfUserAsync().ToString(), "OK");
                        });*/

                        await App.Database.SaveUserAsync(new User {
                            Login = _login,
                            Password = _password,
                            Email = _email,
                            Name = _name,
                            Surname = _surname,
                            Role = _selectedRole
                        });

                        Device.BeginInvokeOnMainThread(async () => {
                            await App.Current.MainPage.DisplayAlert("Result", "Konto zostało założone", "OK");
                            await App.Navigation.PopToRootAsync();
                        });
                    }
                    else {
                        Device.BeginInvokeOnMainThread(async () => {
                            await App.Current.MainPage.DisplayAlert("Result", "Użytkownik o tej nazwie istnieje", "OK");
                        });
                    }

                }
            });
        }

        public string Email {
            set {
                if (_email != value) {
                    _email = value;
                }
            }
            get {
                return _email;
            }
        }

        public string Login {
            set {
                if (_login != value) {
                    _login = value;
                }
            }
            get {
                return _login;
            }
        }

        public string Password {
            set {
                if (_password != value) {
                    _password = value;
                }
            }
            get {
                return _password;
            }
        }

        public string Name {
            set {
                if (_name != value) {
                    _name = value;
                }
            }
            get {
                return _name;
            }
        }

        public string Surname {
            set {
                if (_surname != value) {
                    _surname = value;
                }
            }
            get {
                return _surname;
            }
        }

        public string SelectedRole {
            get => _selectedRole;
            set {
                _selectedRole = value;
            }
        }

        public ICommand CreateAccount { protected set; get; }

    }
}

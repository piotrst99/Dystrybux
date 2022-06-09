using Dystrybux.Model;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Dystrybux.ViewModel
{
    public class UserDataViewModel : BaseViewModel{

        User _user = null;

        bool _UserDataAreShowing = true;
        bool _UserDataAreEditing = false;

        public UserDataViewModel(){
            User = App.User;
        }

        public void EditUderData() {
            UserDataAreShowing = false;
            UserDataAreEditing = true;
        }

        public void SaveUserData() {


            App.User = User;
            User = App.User;
            App.Database.UpdateUserDataAsync(App.User);

            UserDataAreShowing = true;
            UserDataAreEditing = false;
        }

        public User User{
            get => _user;
            set { 
                _user = value;
                //OnPropertyChanged();
            }
        }

        public bool UserDataAreShowing {
            get => _UserDataAreShowing;
            set {
                _UserDataAreShowing= value;
                OnPropertyChanged();
            }
        }

        public bool UserDataAreEditing {
            get => _UserDataAreEditing;
            set {
                _UserDataAreEditing = value;
                OnPropertyChanged();
            }
        }

    }
}

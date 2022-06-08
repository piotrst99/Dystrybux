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

        //bool _UserNameIsShow = true;
        //bool _UserNameIsEdit = false;

        //bool _UserSurnameIsShow = true;
        //bool _UserSurnameIsEdit = false;

        public UserDataViewModel(){
            User = App.User;
            
            EditUserDataCommand = new Command(() => {
                UserDataAreShowing = false;
                UserDataAreEditing = true;
            });

            SaveUserDataCommand = new Command(() => {
                UserDataAreShowing = true;
                UserDataAreEditing = false;
            });

            /*EditUserNameCommand = new Command(() => {
                UserNameIsShow = false;
                UserNameIsEdit = true;
            });

            SaveUserNameCommand = new Command(() => {
                UserNameIsShow = true;
                UserNameIsEdit = false;
            });

            EditUserSurnameCommand = new Command(() => {
                UserSurnameIsShow = false;
                UserSurnameIsEdit = true;
            });

            SaveUserSurnameCommand = new Command(() => {
                UserSurnameIsShow = true;
                UserSurnameIsEdit = false;
            });*/

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

        /*public bool UserNameIsShow {
            get => _UserNameIsShow;
            set {
                _UserNameIsShow = value;
                OnPropertyChanged();
            }
        }

        public bool UserNameIsEdit {
            get => _UserNameIsEdit;
            set {
                _UserNameIsEdit = value;
                OnPropertyChanged();
            }
        }

        public bool UserSurnameIsShow {
            get => _UserSurnameIsShow;
            set {
                _UserSurnameIsShow = value;
                OnPropertyChanged();
            }
        }

        public bool UserSurnameIsEdit {
            get => _UserSurnameIsEdit;
            set {
                _UserSurnameIsEdit = value;
                OnPropertyChanged();
            }
        }*/

        public Command EditUserDataCommand { protected set; get; }
        public Command SaveUserDataCommand { protected set; get; }

        public Command EditUserNameCommand { protected set; get; }
        public Command SaveUserNameCommand { protected set; get; }
        public Command EditUserSurnameCommand { protected set; get; }
        public Command SaveUserSurnameCommand { protected set; get; }

    }
}

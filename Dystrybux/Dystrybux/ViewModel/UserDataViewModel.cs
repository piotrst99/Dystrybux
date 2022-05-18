using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Dystrybux.ViewModel
{
    public class UserDataViewModel : BaseViewModel{

        string _name = "";
        string _surname = "";


        bool _UserDataAreShowing = true;
        bool _UserDataAreEditing = false;

        bool _UserNameIsShow = true;
        bool _UserNameIsEdit = false;

        bool _UserSurnameIsShow = true;
        bool _UserSurnameIsEdit = false;

        public UserDataViewModel(){
            Name = App.User.Name;
            Surname = App.User.Surname;

            EditUserDataCommand = new Command(() => {
                UserDataAreShowing = false;
                UserDataAreEditing = true;
            });

            SaveUserDataCommand = new Command(() => {
                UserDataAreShowing = true;
                UserDataAreEditing = false;
            });

            EditUserNameCommand = new Command(() => {
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
            });

        }

        public string Name{
            get => _name;
            set => _name = value;
        }

        public string Surname{
            get => _surname;
            set => _surname = value;
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

        public bool UserNameIsShow {
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
        }

        public Command EditUserDataCommand { protected set; get; }
        public Command SaveUserDataCommand { protected set; get; }

        public Command EditUserNameCommand { protected set; get; }
        public Command SaveUserNameCommand { protected set; get; }
        public Command EditUserSurnameCommand { protected set; get; }
        public Command SaveUserSurnameCommand { protected set; get; }

    }
}

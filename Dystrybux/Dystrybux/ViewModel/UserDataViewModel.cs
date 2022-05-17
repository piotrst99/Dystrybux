using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Dystrybux.ViewModel
{
    public class UserDataViewModel : BaseViewModel{

        string _name = "";
        string _surname = "";

        //bool _UserNameIsVisible = false;
        bool _UserDataIsEdit = false;

        public UserDataViewModel(){
            Name = App.User.Name;
            Surname = App.User.Surname;

            EditUserNameCommand = new Command(() => _UserDataIsEdit = true);

        }

        public string Name{
            get => _name;
            set => _name = value;
        }

        public string Surname{
            get => _surname;
            set => _surname = value;
        }

        public bool UserDataIsEdit {
            get => _UserDataIsEdit;
            set => _UserDataIsEdit = value;
        }

        public Command EditUserNameCommand { protected set; get; }

    }
}

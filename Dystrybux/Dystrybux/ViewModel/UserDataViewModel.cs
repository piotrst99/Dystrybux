using System;
using System.Collections.Generic;
using System.Text;

namespace Dystrybux.ViewModel
{
    public class UserDataViewModel:BaseViewModel{

        string _name = "";
        string _surname = "";


        public UserDataViewModel(){
            Name = App.User.Name;
            Surname = App.User.Surname;
        }

        public string Name{
            get => _name;
            set => _name = value;
        }

        public string Surname{
            get => _surname;
            set => _surname = value;
        }

    }
}

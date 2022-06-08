using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Dystrybux.ViewModel {
    public class PasswordViewModel :BaseViewModel {

        string _oldPassword = "";
        string _newPassword = "";
        string _newPassword2 = "";

        bool _oldPasswordIsNotCorrect = false;
        bool _newPasswordIsNotCorrect = false;

        public PasswordViewModel() {
            ChangePasswordCommand = new Command(async () => await ChangePassword());
        }

        async Task ChangePassword() {
            OldPasswordIsNotCorrect = !CheckOldPassword();
            NewPasswordIsNotCorrect = !CheckNewPassword();
            
            if (CheckOldPassword() && CheckNewPassword()) {
                App.User.Password = NewPassword;
                await App.Database.UpdateUserDataAsync(App.User);
            }
        }

        bool CheckOldPassword() {
            return OldPassword == App.User.Password;
        }

        bool CheckNewPassword() {
            return NewPassword == NewPassword2;
        }

        public string OldPassword{
            get => _oldPassword;
            set => _oldPassword = value;
        }

        public string NewPassword {
            get => _newPassword;
            set => _newPassword = value;
        }

        public string NewPassword2 {
            get => _newPassword2;
            set => _newPassword2 = value;
        }

        public bool OldPasswordIsNotCorrect {
            get => _oldPasswordIsNotCorrect;
            set => _oldPasswordIsNotCorrect = value;
        }

        public bool NewPasswordIsNotCorrect {
            get => _newPasswordIsNotCorrect;
            set => _newPasswordIsNotCorrect = value;
        }

        public Command ChangePasswordCommand { protected set; get; }

    }
}

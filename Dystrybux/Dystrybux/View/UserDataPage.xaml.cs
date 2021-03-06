using Dystrybux.ViewModel;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Dystrybux.View
{
    public partial class UserDataPage : ContentPage{

        UserDataViewModel _userDataViewModel;
        ToolbarItem editBtn = null;
        ToolbarItem saveBtn = null;

        public UserDataPage(){
            InitializeComponent();
            BindingContext = _userDataViewModel = new UserDataViewModel();

            editBtn = new ToolbarItem {
                IconImageSource = "editIcon.jpg"
            };

            saveBtn = new ToolbarItem {
                IconImageSource = "saveIcon.jpg"
            };

            editBtn.Clicked += EditDataClick;
            saveBtn.Clicked += SaveDataClick;

            this.ToolbarItems.Add(editBtn);

        }

        void EditDataClick(object sender, EventArgs e) {
            this.ToolbarItems.Remove(editBtn);
            //_ = _userDataViewModel.EditUserDataCommand;
            _userDataViewModel.EditUderData();
            this.ToolbarItems.Add(saveBtn);
        }

        void SaveDataClick(object sender, EventArgs e) {
            this.ToolbarItems.Remove(saveBtn);
            //_ = _userDataViewModel.SaveUserDataCommand;
            _userDataViewModel.SaveUserData();
            this.ToolbarItems.Add(editBtn);
        }

    }
}

using Dystrybux.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Dystrybux.View {
    public partial class CompanyDataPage : ContentPage {
        CompanyDataViewModel _companyDataViewModel;
        ToolbarItem editBtn = null;
        ToolbarItem saveBtn = null;

        public CompanyDataPage() {
            InitializeComponent();
            BindingContext = _companyDataViewModel = new CompanyDataViewModel();
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
            _companyDataViewModel.EditCompanyData();
            this.ToolbarItems.Add(saveBtn);
        }

        void SaveDataClick(object sender, EventArgs e) {
            this.ToolbarItems.Remove(saveBtn);
            //_ = _userDataViewModel.SaveUserDataCommand;
            _companyDataViewModel.SaveCompanyData();
            this.ToolbarItems.Add(editBtn);
        }

    }
}
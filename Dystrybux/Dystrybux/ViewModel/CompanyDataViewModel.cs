using Dystrybux.Model;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Dystrybux.ViewModel {
    public class CompanyDataViewModel : BaseViewModel{

        Company _company = null;
        bool _CompanyDataAreShowing = true;
        bool _CompanyDataAreEditing = false;

        public CompanyDataViewModel() {
            CheckCompanyIsExists();
        }

        void CheckCompanyIsExists() {
            Company = App.Database.GetCompanyAsync(App.User.ID).Result;
            if (Company == null) {
                /*Device.BeginInvokeOnMainThread(async () => {
                    await App.Current.MainPage.DisplayAlert("Result", "puste", "OK");
                });*/
                Company = new Company {
                    CompanyName ="",
                    Street = "",
                    Number = "",
                    LocalNumber = "",
                    UserID = App.User.ID
                };

                App.Database.SaveCompanyAsync(Company);
                Company = App.Database.GetCompanyAsync(App.User.ID).Result;
            }
        }

        public void EditCompanyData() {
            CompanyDataAreShowing = false;
            CompanyDataAreEditing = true;
        }

        public void SaveCompanyData() {
            App.Database.UpdateCompanyAsync(Company);

            CompanyDataAreShowing = true;
            CompanyDataAreEditing = false;
            //OnPropertyChanged();
        }

        public Company Company {
            get => _company;
            set => _company = value;
        }

        public bool CompanyDataAreShowing {
            get => _CompanyDataAreShowing;
            set { _CompanyDataAreShowing = value; OnPropertyChanged(); }
        }

        public bool CompanyDataAreEditing {
            get => _CompanyDataAreEditing;
            set { _CompanyDataAreEditing = value; OnPropertyChanged(); }
        }

    }
}

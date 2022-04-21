using Dystrybux.Model;
using System;
using System.Collections.Generic;
using System.Text;
using Xamarin.Forms;

namespace Dystrybux.ViewModel {
    public class NewItemViewModel : BaseViewModel {
        string _name = "";
        string _description = "";
        int _cost = 0;
        int _count = 0;

        public NewItemViewModel() {
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            this.PropertyChanged += (_, __) => SaveCommand.ChangeCanExecute();
        }

        private bool ValidateSave() {
            return !String.IsNullOrWhiteSpace(_name)
                && !String.IsNullOrWhiteSpace(_description)
                && _cost >= 0 && _count >= 0;
        }

        private async void OnCancel() {
            await App.Navigation.PopAsync();
        }

        private async void OnSave() {
            Product newProduct = new Product() {
                ID = 1,
                Name = Name,
                Cost = Cost,
                Count = Count,
                Description = Description
            };

            await App.Database.SaveProductAsync(newProduct);

            //await ProductStore.AddItemAsync(newProduct);
            /*Device.BeginInvokeOnMainThread(async () => {
                await page.DisplayAlert("Result", ProductsList.Count.ToString(), "OK");
            });*/
            //await App.Current.MainPage.DisplayAlert("Result", ProductsList.Count.ToString(), "OK");
            await App.Navigation.PopAsync();
            //await App.Current.MainPage.DisplayAlert("Result", ProductsList.Count.ToString(), "OK");
            //await App.Navigation.PopModalAsync();
        }

        

        public string Name {
            get => _name;
            set {
                _name = value;
                OnPropertyChanged();
            }
        }

        public string Description {
            get => _description;
            set {
                _description = value;
                OnPropertyChanged();
            }
        }

        public int Cost {
            get => _cost;
            set {
                _cost = value;
                OnPropertyChanged();
            }
        }

        public int Count {
            get => _count;
            set { 
                _count = value;
                OnPropertyChanged();
            }
        }



        public Command SaveCommand { protected set;  get; }
        public Command CancelCommand { protected set; get; }
    }
}


// https://docs.microsoft.com/pl-pl/xamarin/essentials/media-picker?tabs=android


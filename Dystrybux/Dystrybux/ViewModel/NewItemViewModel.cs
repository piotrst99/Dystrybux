using Dystrybux.Model;
using NativeMedia;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Dystrybux.ViewModel {
    public class NewItemViewModel : BaseViewModel {
        string _name = "";
        string _description = "";
        string _fileName = "";
        int _cost = 0;
        int _count = 0;
        ImageSource _image;
        string _imagePath = "";

        public NewItemViewModel() {
            SaveCommand = new Command(OnSave, ValidateSave);
            CancelCommand = new Command(OnCancel);
            TakePhotoCommand = new Command(TakePhoto);
            PickPhotoCommand = new Command(PickPhoto);
            this.PropertyChanged += (_, __) => SaveCommand.ChangeCanExecute();
        }

        private bool ValidateSave() {
            return !String.IsNullOrWhiteSpace(_name)
                && !String.IsNullOrWhiteSpace(_description)
                && _cost >= 0 && _count >= 0 && ImageCamera != null;
        }

        private async void OnCancel() {
            await App.Navigation.PopAsync();
        }

        private async void OnSave() {

            /*string file = _name + new Random().Next(1, 10000).ToString() + ".png";
            string fileName = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), file);

            using (var fileStream = new FileStream(fileName, FileMode.Create, FileAccess.Write)) {
                //ima.CopyTo(fileStream);
                
            }*/


            Product newProduct = new Product() {
                ID = 1,
                Name = Name,
                Cost = Cost,
                Count = Count,
                Description = Description,
                ImagePath = _fileName
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

        private async void TakePhoto() {
            var result = await MediaPicker.CapturePhotoAsync();
            if (result != null) {
                var stream = await result.OpenReadAsync();
                //ImageCamera.Source = ImageSource.FromStream(() => stream);
                ImageCamera = ImageSource.FromStream(() => stream);
            }

            if (result != null) {
                Device.BeginInvokeOnMainThread(async () => {

                    bool choice = await App.Current.MainPage.DisplayAlert("", "Czy dodać zdjęcie?", "Tak", "Nie");
                    if (choice) {

                        //var photoName = "productImage_" + new Random().Next(1, 10000) + "_" + new Random().Next(1, 10000) + ".png";
                        var photoName = "productImage_" + new Random().Next(1, 10000) + "_" + new Random().Next(1, 10000);// + ".png";
                        _fileName = photoName;

                        var stream2 = await result.OpenReadAsync();
                        await MediaGallery.SaveAsync(MediaFileType.Image, stream2, photoName + ".png");
                    }
                });

            }
            else {
                Device.BeginInvokeOnMainThread(async () => {
                    await App.Current.MainPage.DisplayAlert("Result", "nie wybrano zdj", "OK");
                });
            }

            /*var photo = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions() { });

            if (photo != null)
                ImageCamera.Source = ImageSource.FromStream(() => { return photo.GetStream(); });*/

            /*var result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions {
                Title = "Wybierz zdjęcie"
            });

            if(result != null) {
                var stream = await result.OpenReadAsync();
                ImageCamera = ImageSource.FromStream(() => stream);
            }*/

            /*var photo = await Plugin.Media.CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions() { });
            if (photo != null) {
                ImageCamera = ImageSource.FromStream(() => photo.GetStream());
            }*/

        }

        private async void PickPhoto() {
            var result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions {
                Title = "Wybierz zdjęcie"
            });


            /*Device.BeginInvokeOnMainThread(async () => {
                await App.Current.MainPage.DisplayAlert("Result", result.FullPath, "OK");
            });*/

            var stream = await result.OpenReadAsync();
            ImageCamera = ImageSource.FromStream(() => stream);

            if (result != null) {
                Device.BeginInvokeOnMainThread(async () => {

                    bool choice = await App.Current.MainPage.DisplayAlert("", "Czy dodać zdjęcie?", "Tak", "Nie");
                    if (choice) {

                        //var photoName = "productImage_" + new Random().Next(1, 10000) + "_" + new Random().Next(1, 10000) + ".png";
                        var photoName = "productImage_" + new Random().Next(1, 10000) + "_" + new Random().Next(1, 10000);// + ".png";
                        _fileName = photoName;

                        var stream2 = await result.OpenReadAsync();
                        await MediaGallery.SaveAsync(MediaFileType.Image, stream2, photoName + ".png");
                    }
                });

            }
            else {
                Device.BeginInvokeOnMainThread(async () => {
                    await App.Current.MainPage.DisplayAlert("Result", "nie wybrano zdj", "OK");
                });
            }


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

        public ImageSource ImageCamera {
            get => _image;
            set {
                /*if(_image == null)*/ _image = value;
                OnPropertyChanged();
            }
        }

        public Command SaveCommand { protected set;  get; }
        public Command CancelCommand { protected set; get; }
        public Command TakePhotoCommand { protected set; get; }
        public Command PickPhotoCommand { protected set; get; }
    }
}


// https://docs.microsoft.com/pl-pl/xamarin/essentials/media-picker?tabs=android


using Dystrybux.Model;
using Dystrybux.Service;
using NativeMedia;
using Plugin.Media;
using Plugin.Media.Abstractions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace Dystrybux.ViewModel {
    public class ProductDetailViewModel: BaseViewModel {
        Product _product = null;
        string _name = "";
        string _description = "";
        int _cost = 0;
        int _count = 0;
        ImageSource _image = null;
        bool _countIsMoreThanZero = true;

        bool _isWatching = true;
        bool _isEdit = false;
        bool _IsClient = false;
        bool _IsBusiness = false;

        List<string> sciezki = new List<string>();

        public ProductDetailViewModel(Product product) {
            _product = product;
            DeleteItemCommand = new Command(async () => await DeleteItem());
            EditItemCommand = new Command(() => EditItem());
            SaveDataCommand = new Command(async () => await SaveData());
            TakePhotoCommand = new Command(TakePhoto);
            PickPhotoCommand = new Command(PickPhoto);

            AddProductToOrderCommand = new Command(() => AddProductToOrder());

            Name = product.Name;
            Description = product.Description;
            Cost = product.Cost;
            Count = product.Count;

            IsClient = App.User.Role == "Client" ? true : false;
            IsBusiness = App.User.Role == "Client" ? false : true;

            CountIsMoreThanZero = product.Count > 0;

            foreach (var fileName in System.IO.Directory.GetFiles("/storage/emulated/0/Pictures/Dystrybux.Android")) {
                sciezki.Add(fileName);
            }

            string imagePath = sciezki.Where(q => q.Contains(_product.ImagePath)).FirstOrDefault();
            if (!string.IsNullOrEmpty(imagePath)) {
                ImageCamera = (Device.RuntimePlatform == Device.Android) ?
                    ImageSource.FromFile(imagePath) :
                    ImageSource.FromFile("/storage/emulated/0/Pictures/Dystrybux.Android/productImage_1704_495_20220606_105829.png");
            }

            /*ImageCamera = (Device.RuntimePlatform == Device.Android) ?
                ImageSource.FromFile("/storage/emulated/0/Pictures/Dystrybux.Android/productImage_6309_9742_20220606_101827.png") : null;
            */

        }

        async Task DeleteItem() {
            bool choice = await App.Current.MainPage.DisplayAlert("Uwaga", "Czy usunąć produkt?", "Tak", "Nie");
            if (choice) {
                await App.Database.DeleteProductAsync(_product);
                await App.Navigation.PopAsync();
            }
        }

        void AddProductToOrder() {

            Device.BeginInvokeOnMainThread(async () => {
                bool choice = await App.Current.MainPage.DisplayAlert("", "Czy dodać produkt do zamówienia?", "Tak", "Nie");
                if (choice) {

                    var order = await App.Database.GetUndoneOrderAsync("Nie złożono");
                    
                    if (order == null) {
                        // create new order
                        var newOrder = new Order {
                            Name = "",
                            OrderedDate = null,
                            FirstDate = null,
                            SecondDate = null,
                            Products = new List<Product>() { },
                            Status = "Nie złożono",
                            UserID = App.User.ID
                        };

                        _product.Count -= 1;
                        await App.Database.UpdateProductAsync(_product);

                        await App.Database.SaveOrderAsync(newOrder);
                        await App.Database.SaveProductOrderAsync(newOrder, _product);
                    }
                    else{
                        // add to undone order

                        var productFromOrder = await App.Database.GetProductFromOrderAsync(order.ID, _product.ID);

                        if(productFromOrder == null) {
                            _product.Count -= 1;
                            await App.Database.UpdateProductAsync(_product);
                            await App.Database.SaveProductOrderAsync(order, _product);
                        }
                        else {
                            Device.BeginInvokeOnMainThread(async () => {
                                await App.Current.MainPage.DisplayAlert("Result", "Produkt został dodany do zamówienia", "OK");
                            });
                        }

                        


                        /*_product.Count -= 1;
                        await App.Database.UpdateProductAsync(_product);
                        await App.Database.SaveProductOrderAsync(order, _product);*/

                    }
                    

                    //var order = await OrderStore.GetItemAsync(3);
                    //var order = await OrderStore.GetItemAsync(3);

                    //var order = await App.Database.GetOrderAsync(1);

                    //order.Products = new List<Product>() { product };

                    //await App.Database.SaveProductOrderAsync(_selectedOrder, _product);

                    //order.Products = new List<Product>(){ product };

                    //await App.Navigation.PopAsync();
                }

                //await App.Navigation.PopAsync();
            });

        }

        void EditItem() {
            IsEdit = true;
            IsWatching = false;
            Device.BeginInvokeOnMainThread(async () => {
                //await App.Current.MainPage.DisplayAlert("Result", Path.GetFileName("/storage/emulated/0/Pictures/Dystrybux.Android/"), "OK");
            });
        }

        async Task SaveData() {
            IsEdit = false;
            IsWatching = true;

            _product.Name = Name;
            _product.Description = Description;
            _product.Cost = Cost;
            _product.Count = Count;

            await App.Database.UpdateProductAsync(_product);
            //await MediaGallery.SaveAsync(MediaFileType.Image, ImageCamera, "image666.png");
        }

        private async void TakePhoto() {

            // https://mahmudx.com/how-to-use-camera-to-take-photo-in-xamarin-forms

            // !!!!!
            // https://www.youtube.com/watch?v=8JvgnlHVyrI // Potrzebne do zapisywania zdjecia
            // https://www.youtube.com/watch?v=sUxzoSl4SUw // Share

            var result = await MediaPicker.CapturePhotoAsync();
            //List<int> result = null;
            if (result != null) {
                var stream = await result.OpenReadAsync();
                ImageCamera = ImageSource.FromStream(() => stream);

                // !!!!!

                Device.BeginInvokeOnMainThread(async () => {
                    bool choice = await App.Current.MainPage.DisplayAlert("", "Czy dodać zdjęcie?", "Tak", "Nie");
                    if (choice) {
                        //var photoName = "productImage_" + new Random().Next(1, 10000) + "_" + new Random().Next(1, 10000) + ".png";
                        var photoName = "productImage_" + new Random().Next(1, 10000) + "_" + new Random().Next(1, 10000);// + ".png";
                        _product.ImagePath = photoName;

                        await MediaGallery.SaveAsync(MediaFileType.Image, stream, photoName + ".png");
                    }
                });


                /*await Share.RequestAsync(new ShareFileRequest {
                    Title = "Save",
                    //File = new ShareFile(ImageCamera.)
                });*/

                //Image image = new Image();
                //image = ImageSource.FromStream(() => stream);

                //int totalPixel = ImageCamera.hei
                //ImageSource.FromStream(() => new MemoryStream(imageAsBytes));
                //Image image = new Image(); image.Source = ImageSource.FromStream(() => new MemoryStream(new byte[] { }))

                // https://docs.microsoft.com/pl-pl/xamarin/xamarin-forms/user-interface/images?tabs=windows
                // https://www.androidbugfix.com/2021/09/xamarin-forms-save-image-from-url-into.html
                // https://medium.com/@hakimgulamali88/a-helper-for-converting-a-xamarin-forms-imagesource-to-the-respective-native-images-56a5aaf98156

                ////
                ///
                // https://docs.microsoft.com/en-us/answers/questions/531801/save-a-picture-from-camera-to-device-gallery-using.html
                // https://stackoverflow.com/questions/26814426/how-to-convert-imagesource-to-byte-array

                //MemoryStream ms = new MemoryStream();
                //stream.CopyTo(ms);
                //var bytes = ms.ToArray();

                //using (var ms = new MemoryStream()) {
                //imageIn.Save(ms, imageIn.RawFormat);
                //return ms.ToArray();

                //}

                //DependencyService.Get<IPicture>().SavePictureToDisk("imageTest", )
                //DependencyService.Get<IMediaService>().SaveImageFromByte(new byte[2], "imageTest");

                // https://www.androidbugfix.com/2021/09/xamarin-forms-save-image-from-url-into.html
                /*byte[] bytes2 = new byte[6];
                bytes2[0] = 1;
                bytes2[1] = 1;
                bytes2[2] = 1;
                bytes2[3] = 1;
                bytes2[4] = 1;
                bytes2[5] = 1;
                Stream stream11 = new MemoryStream(bytes2);
                DependencyService.Get<IMediaService>().SaveImageFromByte(bytes2, "test123666.jpg");*/

                //MemoryStream ms = new MemoryStream();
                //stream.CopyTo(ms);
                //var bytes = ms.ToArray();

                //await App.Current.MainPage.DisplayAlert("Alert", stream.ToString(), "OK");

                //Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),"plik.jpg");

                //DependencyService.Get<ITestInterface>().StorePhotoToGallery(bytes, "test.png");

                /*var folderPath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                var filePath = Path.Combine(folderPath, "image.jpg");
                using (var fileStream = File.Create(filePath)) {
                    await this.ImageCamera.SaveAsync(fileStream, ImageFormat.Jpeg, 1);
                }

                await Application.Current.MainPage.DisplayAlert("", "The Image is saved with original size", "OK");*/

            }
            
            /*try {

                var photo = await CrossMedia.Current.TakePhotoAsync(new StoreCameraMediaOptions() {
                    DefaultCamera = Plugin.Media.Abstractions.CameraDevice.Rear,
                    Directory = "Dystrybux",
                    SaveToAlbum = true
                });

                if (photo != null)
                    ImageCamera = ImageSource.FromStream(() => { return photo.GetStream(); });
                    //imgCam.Source = ImageSource.FromStream(() => { return photo.GetStream(); });

            }
            catch (Exception ex) {
                await App.Current.MainPage.DisplayAlert("Error", ex.Message.ToString(), "Ok");
            }*/

            /*var cameraMediaOptions = new StoreCameraMediaOptions {
                DefaultCamera = CameraDevice.Rear,

                // Set the value to true if you want to save the photo to your public storage.
                SaveToAlbum = true,

                // Give the name of the folder you want to save to
                Directory = "MyAppName",

                // Give a photo name of your choice,
                // or set it to null if you want to use the default naming convention
                Name = null,

                // Set the compression quality
                // 0 = Maximum compression but worse quality
                // 100 = Minimum compression but best quality
                CompressionQuality = 100
            };
            MediaFile photo = await CrossMedia.Current.TakePhotoAsync(cameraMediaOptions);
            if (photo == null) return;
            ImageCamera = ImageSource.FromStream(() => photo.GetStream());
            */

            /*var photo = await CrossMedia.Current.TakePhotoAsync(new Plugin.Media.Abstractions.StoreCameraMediaOptions() { });

            if (photo != null) {
                byte[] imageArray = null;

                using (MemoryStream memory = new MemoryStream()) {

                    Stream stream = photo.GetStream();
                    ImageCamera = ImageSource.FromStream(() => stream);
                    stream.CopyTo(memory);
                    imageArray = memory.ToArray();

                    //string documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
                    //string localFilename = "downloaded.jpg";
                    //string localPath = Path.Combine(documentsPath, localFilename);

                    //await App.Current.MainPage.DisplayAlert("Alert", localPath, "OK");

                }

            }*/


            /*var photo = CrossMedia.Current.TakePhotoAsync(
                new StoreCameraMediaOptions {
                    SaveToAlbum = true
            });*/

            /*var photo = await CrossMedia.Current.TakePhotoAsync(
            new StoreCameraMediaOptions {
                SaveToAlbum = true
            });*/

            // https://stackoverflow.com/questions/51038251/how-to-download-image-and-save-it-in-local-storage-using-xamarin-forms
            // https://stackoverflow.com/questions/57192117/how-to-save-an-image-in-sqlite-in-xamarin-forms

            // https://docs.devexpress.com/MobileControls/403498/xamarin-forms/data-form/data-validation

            // SavePicture("test123", ImageCamera, "imagesFolder");
            // DependencyService.Get<IFileService>().SavePicture("ImageName.jpg", imageData, "imagesFolder")
        }

        /*private void SavePicture(string name, Stream data, string location = "temp") {
            var documentsPath = Environment.GetFolderPath(Environment.SpecialFolder.Personal);
            documentsPath = Path.Combine(documentsPath, "Orders", location);
            Directory.CreateDirectory(documentsPath);

            string filePath = Path.Combine(documentsPath, name);

            byte[] bArray = new byte[data.Length];
            using (FileStream fs = new FileStream(filePath, FileMode.OpenOrCreate)) {
                using (data) {
                    data.Read(bArray, 0, (int)data.Length);
                }
                int length = bArray.Length;
                fs.Write(bArray, 0, length);
            }
        }*/

        private async void PickPhoto() {
            var result = await MediaPicker.PickPhotoAsync(new MediaPickerOptions {
                Title = "Wybierz zdjęcie"
            });


            /*Device.BeginInvokeOnMainThread(async () => {
                await App.Current.MainPage.DisplayAlert("Result", result.FullPath, "OK");
            });*/

            var stream = await result.OpenReadAsync();
            ImageCamera = ImageSource.FromStream(() => stream);
            
            if(result != null) {
                Device.BeginInvokeOnMainThread(async () => {

                    bool choice = await App.Current.MainPage.DisplayAlert("", "Czy dodać zdjęcie?", "Tak", "Nie");
                    if (choice) {

                        //var photoName = "productImage_" + new Random().Next(1, 10000) + "_" + new Random().Next(1, 10000) + ".png";
                        var photoName = "productImage_" + new Random().Next(1, 10000) + "_" + new Random().Next(1, 10000);// + ".png";
                        _product.ImagePath = photoName;

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

        public bool IsWatching {
            get => _isWatching;
            set {
                _isWatching = value;
                OnPropertyChanged();
            }
        }

        public bool IsEdit {
            get => _isEdit;
            set {
                _isEdit = value;
                OnPropertyChanged();
            }
        }

        public bool IsClient {
            get => _IsClient;
            set => _IsClient = value;
        }

        public bool IsBusiness {
            get => _IsBusiness;
            set => _IsBusiness = value;
        }

        public ImageSource ImageCamera {
            get => _image;
            set {
                /*if (_image == null)*/ _image = value;
                OnPropertyChanged();
            }
        }

        public bool CountIsMoreThanZero {
            get => _countIsMoreThanZero;
            set => _countIsMoreThanZero = value;
        }

        public Command DeleteItemCommand { protected set; get; }
        public Command EditItemCommand { protected set; get; }
        public Command SaveDataCommand { protected set; get; }
        public Command AddProductToOrderCommand { protected set; get; }
        public Command TakePhotoCommand { protected set; get; }
        public Command PickPhotoCommand { protected set; get; }

    }
}

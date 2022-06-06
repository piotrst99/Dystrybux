using Android.App;
using Android.Content;
using Android.OS;
using Android.Runtime;
using Android.Views;
using Android.Widget;
using Dystrybux.Droid;
using Dystrybux.Service;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

//[assembly: Xamarin.Forms.Dependency(typeof(TestImplementation))]
namespace Dystrybux.Droid {
    /*public class TestImplementation : ITestInterface{
        [Obsolete]
        public async void StorePhotoToGallery(byte[] bytes, string fileName) {
            Context context = MainActivity.Instance;
            try {
                Java.IO.File storagePath = Android.OS.Environment.GetExternalStoragePublicDirectory(Android.OS.Environment.DirectoryPictures);
                string path = System.IO.Path.Combine(storagePath.ToString(), fileName);
                System.IO.File.WriteAllBytes(path, bytes);
                var mediaScanIntent = new Intent(Intent.ActionMediaScannerScanFile);
                mediaScanIntent.SetData(Android.Net.Uri.FromFile(new Java.IO.File(path)));
                context.SendBroadcast(mediaScanIntent);
            }
            catch (Exception ex) {

            }
        }
    }*/
}
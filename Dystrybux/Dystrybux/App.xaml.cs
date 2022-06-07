using Dystrybux.DataBase;
using Dystrybux.Model;
using Dystrybux.Service;
using Dystrybux.View;
using System;
using System.IO;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Dystrybux {
    public partial class App : Application {

        public static INavigation Navigation = null;

        static Database database;
        static User user;

        public static Database Database {
            get {
                if(database == null) {
                    database = new Database(Path.Combine(Environment.GetFolderPath(
                        Environment.SpecialFolder.LocalApplicationData), "Dystrybux.db3"));
                }
                return database;
            }
        }

        public static User User {
            get => user;
            set => user = value;
        }

        public App() {
            InitializeComponent();

            Plugin.Media.CrossMedia.Current.Initialize();

            DependencyService.Register<ProductDataStore>();
            DependencyService.Register<OrderDataStore>();
            
            NavigationPage page = new NavigationPage(new LoginPage());
            App.Navigation = page.Navigation;
            MainPage = page;

            var o = App.Database.GetOrderAsync(5).Result;
            //o.Status = "W realizacji";
            o.Status = "Złożono";
            App.Database.UpdateOrderAsync(o);

        }

        protected override void OnStart() { }

        protected override void OnSleep() { }

        protected override void OnResume() { /*App.Navigation.PopToRootAsync();*/ }
    }
}

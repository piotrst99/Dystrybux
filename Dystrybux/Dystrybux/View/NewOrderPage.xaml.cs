using Dystrybux.Model;
using Dystrybux.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Dystrybux.View {
    public partial class NewOrderPage : ContentPage {

        NewOrderViewModel _newOrderViewModel;

        public NewOrderPage(string orderName) {
            InitializeComponent();
            //this.BindingContext = new NewOrderViewModel();
            BindingContext = _newOrderViewModel = new NewOrderViewModel(orderName);

            if (App.User.Role == "Client") {
                var searchProductToolBarItem = new ToolbarItem {
                    Text = "Przeglądaj",
                    Command = _newOrderViewModel.SearchProductCommand,
                };
                this.ToolbarItems.Add(searchProductToolBarItem);
            }
        }

        public NewOrderPage(Order order) {
            InitializeComponent();
            //this.BindingContext = new NewOrderViewModel(order);
            BindingContext = _newOrderViewModel = new NewOrderViewModel(order);

            if (App.User.Role == "Client") {
                var searchProductToolBarItem = new ToolbarItem {
                    Text = "Przeglądaj",
                    Command = _newOrderViewModel.SearchProductCommand,
                };
                this.ToolbarItems.Add(searchProductToolBarItem);
            }
        }

        protected override void OnAppearing() {
            base.OnAppearing();
            _newOrderViewModel.OnAppearing();
        }

        private void CountChanged(object sender, TextChangedEventArgs e) {
            _newOrderViewModel.SetCountCommand(int.Parse(e.NewTextValue));

        }

        private void IncrementCount(object sender, EventArgs e) {
            Button button = (Button)sender;
            //StackLayout listViewItem = (StackLayout)button.Parent;
            //StackLayout test = (StackLayout)listViewItem.Parent;
            //StackLayout test2 = (StackLayout)test.Parent;
            //CollectionView test3 = (CollectionView)button.Parent;
            //ListView test3 = (ListView)button.Parent;
            //StackLayout listViewItem2 = (StackLayout)listViewItem.Parent;
            //var index = ItemsListView.
            var i = ItemsListView.TabIndex;
            
            

            Device.BeginInvokeOnMainThread(async () => {
                await App.Current.MainPage.DisplayAlert("Result", button.TabIndex.ToString(), "OK");
            });
        }

    }
}

// https://social.msdn.microsoft.com/Forums/en-US/4eb771e5-966a-4fde-96bf-94fc52157dd8/load-more-items-without-button-click?forum=xamarinforms

// https://techsolutions2017.blogspot.com/2017/01/listview-in-xamarin-forms-in-mvvm.html#comment-form

// https://stackoverflow.com/questions/61749959/xamarin-observablecollection-doest-automatically-update-listview


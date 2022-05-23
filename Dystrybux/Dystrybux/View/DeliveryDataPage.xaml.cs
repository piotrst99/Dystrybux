using Dystrybux.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Dystrybux.View {
    public partial class DeliveryDataPage : ContentPage {
        DeliveryDataViewModel _deliveryDataViewModel;

        public DeliveryDataPage() {
            InitializeComponent();
            BindingContext = _deliveryDataViewModel = new DeliveryDataViewModel();
        }
    }
}
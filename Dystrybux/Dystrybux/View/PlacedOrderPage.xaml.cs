﻿using Dystrybux.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Dystrybux.View {
    public partial class PlacedOrderPage : ContentPage {
        PlacedOrderViewModel _PlacedOrderViewModel;

        public PlacedOrderPage() {
            InitializeComponent();
            BindingContext = _PlacedOrderViewModel = new PlacedOrderViewModel();
        }
    }
}
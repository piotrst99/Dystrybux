using Dystrybux.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace Dystrybux.View {
    public partial class TestPage : TabbedPage {
        TestViewModel _testViewModel;

        public TestPage() {
            InitializeComponent();
            BindingContext = _testViewModel = new TestViewModel();
            
            //test123
            
        }

        
    }
}
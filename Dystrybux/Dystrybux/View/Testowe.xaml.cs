using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Xamarin.Forms;
using Xamarin.Forms.PlatformConfiguration;
using Xamarin.Forms.PlatformConfiguration.AndroidSpecific;
using Xamarin.Forms.Xaml;

namespace Dystrybux.View {
    public partial class Testowe : Xamarin.Forms.TabbedPage {
        public Testowe() {
            InitializeComponent();
            //On<Android>().SetToolbarPlacement(ToolbarPlacement.Bottom);
        }
    }
}
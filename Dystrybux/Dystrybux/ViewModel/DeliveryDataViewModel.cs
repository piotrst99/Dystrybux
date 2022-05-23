using Dystrybux.Model;
using Dystrybux.View;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Dystrybux.ViewModel {
    public class DeliveryDataViewModel : BaseViewModel {

        Delivery _delivery;

        public DeliveryDataViewModel() {
            SetData();
            
            OrderSummaryCommand = new Command(async () => await OrderSummary());
        }

        private void SetData() {
            var order = App.Database.GetUndoneOrderAsync("Nie złożono").Result;
        
            if(order.DeliveryID == 0) {
                Delivery delivery = new Delivery {
                    //CompanyName = "Nika",
                    //Street = "Wawrzyńca",
                    //Number = "20",
                    //LocalNumber = "",
                    EarliestDate = DateTime.Now,
                    LatestDate = DateTime.Now
                };
                Delivery = delivery;
            }
            else {
                Delivery = App.Database.GetDeliveryFromOrderAsync(order.DeliveryID).Result;
            }
        }

        async Task OrderSummary() {
            if(Delivery.ID == 0) {
                await App.Database.SaveDeliveryAsync(Delivery);
            }
            else {
                await App.Database.UpdateDeliveryAsync(Delivery);
            }
            await App.Navigation.PushAsync(new OrderSummaryPage());
        }

        public Delivery Delivery {
            get => _delivery;
            set => _delivery = value;
        }

        public Command OrderSummaryCommand { protected set; get; }
    }
}

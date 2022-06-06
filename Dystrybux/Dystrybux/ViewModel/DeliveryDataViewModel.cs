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
        bool _earliestDateIsNotCorrect = false;
        bool _latestDateIsNotCorrect = false;

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

            EarliestDateIsNotCorrect = false;
            LatestDateIsNotCorrect = false;
        }

        async Task OrderSummary() {
            CheckDates();
            if (!EarliestDateIsNotCorrect && !LatestDateIsNotCorrect) {
                if(Delivery.ID == 0) {
                    await App.Database.SaveDeliveryAsync(Delivery);
                }
                else {
                    await App.Database.UpdateDeliveryAsync(Delivery);
                }
                EarliestDateIsNotCorrect = false;
                LatestDateIsNotCorrect = false;
                await App.Navigation.PushAsync(new OrderSummaryPage());
            }
        }

        void CheckDates() {
            if (Delivery.EarliestDate.Date <= DateTime.Today.Date || (Delivery.EarliestDate.Date - DateTime.Now.Date).TotalDays < 2
                || Delivery.EarliestDate.DayOfWeek == DayOfWeek.Sunday)
                EarliestDateIsNotCorrect = true;
            else
                EarliestDateIsNotCorrect = false;
            if (Delivery.LatestDate.Date <= DateTime.Today.Date || Delivery.LatestDate.Date <= Delivery.EarliestDate.Date ||
                (Delivery.LatestDate.Date - Delivery.EarliestDate.Date).TotalDays < 2 || Delivery.LatestDate.DayOfWeek == DayOfWeek.Sunday)
                LatestDateIsNotCorrect = true;
            else
                LatestDateIsNotCorrect = false;
        }

        public Delivery Delivery {
            get => _delivery;
            set => _delivery = value;
        }

        public bool EarliestDateIsNotCorrect {
            get => _earliestDateIsNotCorrect;
            set {
                _earliestDateIsNotCorrect = value;
                OnPropertyChanged();
            }
        }
        
        public bool LatestDateIsNotCorrect {
            get => _latestDateIsNotCorrect;
            set {
                _latestDateIsNotCorrect = value;
                OnPropertyChanged();
            }
        }

        public Command OrderSummaryCommand { protected set; get; }
    }
}

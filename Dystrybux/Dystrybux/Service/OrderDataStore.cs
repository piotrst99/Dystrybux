using Dystrybux.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dystrybux.Service {
    public class OrderDataStore: IDataStore<Order> {
        readonly List<Order> orders;

        public OrderDataStore() {
            orders = new List<Order>() {
                new Order{ID = 1, Name = "220406_00001", OrderedDate="06.04.2022", Products = new List<Product>(){ } },
                new Order{ID = 2, Name = "220406_00002", OrderedDate="06.04.2022", Products = new List<Product>(){ } }
            };
        }

        public async Task<bool> AddItemAsync(Order order) {
            orders.Add(order);
            return await Task.FromResult(true);
        }

        public async Task<Order> GetItemAsync(int id) {
            return await Task.FromResult(orders.FirstOrDefault(q => q.ID == id));
        }

        public async Task<IEnumerable<Order>> GetItemsAsync(bool forceRefresh = false) {
            return await Task.FromResult(orders);
        }

    }
}

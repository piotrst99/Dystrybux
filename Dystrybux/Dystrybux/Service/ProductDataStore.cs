using Dystrybux.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Dystrybux.Service {
    public class ProductDataStore :IDataStore<Product> {
        readonly List<Product> products;

        /*public ProductDataStore() {
            products= new List<Product>() {
                new Product{ ID=1, Name="Kremówka", Count=100, Cost=20, Description="Oryginalna z Wadowic" },
                new Product{ ID=2, Name="Ciasto Czekoladowe", Count=50, Cost=80, Description="Słodka chwila" },
                new Product{ ID=3, Name="Babka wielkanocna", Count=100, Cost=50, Description="Słodka chwila" },
                new Product{ ID=4, Name="Kiełbasa", Count=200, Cost=60, Description="Słodka chwila" },
                new Product{ ID=5, Name="Babka mała", Count=200, Cost=20, Description="Słodka chwila" },
                new Product{ ID=6, Name="Babka średnia", Count=150, Cost=35, Description="Słodka chwila" },
                new Product{ ID=7, Name="Mazurek", Count=35, Cost=75, Description="Słodka chwila" },
                new Product{ ID=8, Name="Mazurek cytrynowy", Count=20, Cost=80, Description="Słodka chwila" },
            };
        }*/

        public async Task<bool> AddItemAsync(Product product) {
            products.Add(product);
            return await Task.FromResult(true);
        }

        public async Task<Product> GetItemAsync(int id) {
            return await Task.FromResult(products.FirstOrDefault(q => q.ID == id));
        }

        public async Task<IEnumerable<Product>> GetItemsAsync(bool forceRefresh = false) {
            return await Task.FromResult(products);
        }

    }
}

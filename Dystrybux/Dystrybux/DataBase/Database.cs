using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Dystrybux.Model;
using SQLite;

namespace Dystrybux.DataBase {
    public class Database {
        readonly SQLiteAsyncConnection _database;

        public Database(string dbPath) {
            _database = new SQLiteAsyncConnection(dbPath);
            //_database.DropTableAsync<User>().Wait();
            //_database.DropTableAsync<Product>().Wait();
            _database.DropTableAsync<Order>().Wait();
            _database.DropTableAsync<OrderProduct>().Wait();

            _database.CreateTableAsync<User>().Wait();
            _database.CreateTableAsync<Product>().Wait();
            _database.CreateTableAsync<Order>().Wait();
            _database.CreateTableAsync<OrderProduct>().Wait();
        }

        //////
        /// UZYTKOWNIK
        /// USER
        //////

        #region USER

        public Task<List<User>> GetUsersAsync() {
            return _database.Table<User>().ToListAsync();
        }

        public Task<User> GetUserAsync(string login, string password, string role) {
            return _database.Table<User>().Where(q => q.Login == login && q.Password == password && q.Role == role).FirstOrDefaultAsync();
        }

        public Task<User> GetUserAsync(string login) {
            return _database.Table<User>().Where(q => q.Login == login).FirstOrDefaultAsync();
        }

        public Task<int> SaveUserAsync(User user) {
            return _database.InsertAsync(user);
        }

        public Task<int> GetCountOfUserAsync() {
            return _database.Table<User>().CountAsync();
        }

        #endregion


        //////
        /// PRODUKT
        /// PRODUCT
        //////

        #region PRODUCT

        public Task<Product> GetProductAsync(int id) {
            return _database.Table<Product>().Where(q => q.ID == id).FirstOrDefaultAsync();
        }

        public Task<List<Product>> GetProductsAsync() {
            return _database.Table<Product>().ToListAsync();
        }

        public Task<List<Product>> GetProductsAsync(string productName) {
            return _database.Table<Product>().Where(q => q.Name.ToUpper().Contains(productName.ToUpper())).ToListAsync();
            //return _database.Table<Product>().Where(q => q.Name.ToLower().Contains(productName.ToLower())).ToListAsync();
            //return _database.Table<Product>().Where(q => q.Name.Contains(productName)).ToListAsync();
        }

        public Task<int> SaveProductAsync(Product product) {
            return _database.InsertAsync(product);
        }

        public Task<int> UpdateProductAsync(Product product) {
            return _database.UpdateAsync(product);
        }

        public Task<int> DeleteProductAsync(Product product) {
            return _database.DeleteAsync(product);
        }

        #endregion

        //////
        /// ZAMOWIENIE
        /// ORDER
        //////

        #region ORDER

        public Task<Order> GetOrderAsync(int id) {
            return _database.Table<Order>().Where(q => q.ID == id).FirstOrDefaultAsync();
        }

        public Task<List<Order>> GetOrdersAsync() {
            return _database.Table<Order>().ToListAsync();
        }

        public Task<List<Order>> GetOrdersAsync(string status) {
            return _database.Table<Order>().Where(q => q.Status != status).ToListAsync();
        }

        public Task<Order> GetUndoneOrderAsync(string status) {
            return _database.Table<Order>().Where(q => q.Status == status).FirstOrDefaultAsync();
        }

        public Task<int> SaveOrderAsync(Order order) {
            return _database.InsertAsync(order);
        }

        public Task<int> UpdateOrderAsync(Order order) {
            return _database.UpdateAsync(order);
        }

        public Task<int> DeleteOrderAsync(Order order) {
            return _database.DeleteAsync(order);
        }

        #endregion

        //////
        /// ZAMOWIENIE_PRODUKT
        /// ORDER_PRODUCT
        //////

        #region ORDER_PRODUCT

        public Task<List<OrderProduct>> GetOrderProductsAsync(int orderID) {
            Task<List<OrderProduct>> orderProducts = _database.Table<OrderProduct>().Where(q => q.OrderID == orderID).ToListAsync();
            
            foreach (var item in orderProducts.Result) {
                item.Order = App.Database.GetOrderAsync(item.OrderID).Result;
                item.Product = App.Database.GetProductAsync(item.ProductID).Result;
            }

            return orderProducts;

            //return _database.Table<OrderProduct>().Where(q => q.OrderID == orderID).ToListAsync();

        }

        public Task<int> SaveProductOrderAsync(Order order, Product product) {
            return _database.InsertAsync(new OrderProduct() { 
                OrderID = order.ID,
                ProductID = product.ID,
                Order = order,
                Product = product
            });
        }

        #endregion

    }
}

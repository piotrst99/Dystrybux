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
            //_database.DropTableAsync<Order>().Wait();
            //_database.DropTableAsync<OrderProduct>().Wait();
            //_database.DropTableAsync<Delivery>().Wait();
            //_database.DropTableAsync<Company>().Wait();

            _database.CreateTableAsync<User>().Wait();
            _database.CreateTableAsync<Product>().Wait();
            _database.CreateTableAsync<Order>().Wait();
            _database.CreateTableAsync<OrderProduct>().Wait();
            _database.CreateTableAsync<Delivery>().Wait();
            _database.CreateTableAsync<Company>().Wait();
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

        public Task<User> GetUserAsync(int userID) {
            return _database.Table<User>().Where(q => q.ID == userID).FirstOrDefaultAsync();
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

        public Task<int> UpdateUserDataAsync(User user) {
            return _database.UpdateAsync(user);
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

        public Task<List<Order>> GetOrdersForUserAsync() {
            return _database.Table<Order>().Where(q => q.UserID == App.User.ID).ToListAsync();
        }

        public Task<List<Order>> GetOrdersForUserAsync(string status) {
            return _database.Table<Order>().Where(q => q.Status == status && q.UserID == App.User.ID).ToListAsync();
        }

        public Task<List<Order>> GetOrdersForEmployeeAsync() {
            return _database.Table<Order>().ToListAsync();
        }

        public Task<List<Order>> GetOrdersForEmployeeAsync(string status){
            return _database.Table<Order>().Where(q => q.Status == status).ToListAsync();
        }

        public Task<Order> GetUndoneOrderAsync(string status) {
            return _database.Table<Order>().Where(q => q.Status == status && q.UserID == App.User.ID).FirstOrDefaultAsync();
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

        public Task<OrderProduct> GetProductFromOrderAsync(int orderID, int productID) {
            return _database.Table<OrderProduct>()
                .Where(q => q.OrderID == orderID && q.ProductID == productID).FirstOrDefaultAsync();
        }

        public Task<int> SaveProductOrderAsync(Order order, Product product) {
            return _database.InsertAsync(new OrderProduct() {
                OrderID = order.ID,
                ProductID = product.ID,
                CountOfProducts = 1,
                Order = order,
                Product = product,
                TotalCostForProduct = product.Cost * 1.0
            });
        }

        public Task<int> UpdateProductOrderAsync(OrderProduct orderProduct) {
            return _database.UpdateAsync(orderProduct);
        }

        public Task<int> RemoveProductFromOrderAsync(OrderProduct orderProduct) {
            return _database.DeleteAsync(orderProduct);
        }

        #endregion

        //////
        /// DOSTAWA
        /// DELIVERY
        //////

        #region DELIVERY

        public Task<Delivery> GetDeliveryFromOrderAsync(int deliveryID) {
            return _database.Table<Delivery>().Where(q => q.ID == deliveryID).FirstOrDefaultAsync();
        }

        public Task<int> SaveDeliveryAsync(Delivery delivery) {
            _database.InsertAsync(delivery);
            var order = GetUndoneOrderAsync("Nie złożono").Result;
            order.DeliveryID = _database.Table<Delivery>().OrderByDescending(q => q.ID).FirstAsync().Result.ID;
            return UpdateOrderAsync(order);
        }

        public Task<int> UpdateDeliveryAsync(Delivery delivery) {
            return _database.UpdateAsync(delivery);
        }

        #endregion

        //////
        /// FIRMA
        /// COMPANY
        //////

        #region COMPANY

        public Task<Company> GetCompanyAsync(int userID) {
            return _database.Table<Company>().Where(q => q.UserID == userID).FirstOrDefaultAsync();
        }

        public Task<int> SaveCompanyAsync(Company company) {
            return _database.InsertAsync(company);
        }

        public Task<int> UpdateCompanyAsync(Company company) {
            return _database.UpdateAsync(company);
        }

        #endregion

    }
}

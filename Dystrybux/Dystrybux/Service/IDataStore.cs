using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Dystrybux.Service {
    public interface IDataStore<T> {
        Task<bool> AddItemAsync(T item);
        Task<T> GetItemAsync(int id);
        Task<IEnumerable<T>> GetItemsAsync(bool forceRefresh = false);
    }
}

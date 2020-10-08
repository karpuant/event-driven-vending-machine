using System.Collections.Generic;
using System.Threading.Tasks;

namespace VendingMachine.Domain.Abstractions
{
    public interface IRepository<T> where T : IAggregateRoot
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> FindByIdAsync(string id);
        Task<bool> UpdateAsync(T entity);
        List<T> ChangedEntities { get; }
        void OnTransactionStarted();
        void OnTransactionCommited();
    }
}

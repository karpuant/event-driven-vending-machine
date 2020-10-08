using VendingMachine.Domain.Aggregates.Product;
using VendingMachine.Domain.Aggregates.Wallet;

namespace VendingMachine.Domain.Abstractions
{
    public interface IUnitOfWork
    {
        void BeginTransaction();
        bool CommitTransaction();
        IRepository<Wallet> Wallet { get; }
        IRepository<Product> Products { get; }
    }
}

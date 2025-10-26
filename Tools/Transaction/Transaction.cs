using Microsoft.EntityFrameworkCore.Storage;
using Microsoft.EntityFrameworkCore; 

namespace coer91.Tools
{
    public class Transaction<T> : ITransaction<T> where T : DbContext
    {
        private readonly T _context;
        private IDbContextTransaction _transaction; 


        public Transaction(T context)
            => _context = context;


        public bool HasTransaction => _transaction is not null;


        public void ClearTracker()
            => _context.ChangeTracker.Clear();


        public async Task BeginTransaction()
            => _transaction ??= await _context.Database.BeginTransactionAsync(); 


        public async Task CommitTransaction()
        {
            if (_transaction is not null) 
            { 
                await _transaction.CommitAsync();
                _transaction = null;
            }
        }


        public async Task RollbackTransaction()
        {
            if(_transaction is not null)
            {
                await _transaction.RollbackAsync();
                _transaction = null;
            }
        }
    } 
}
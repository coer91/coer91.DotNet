using Microsoft.EntityFrameworkCore; 

namespace coer91.Tools
{
    public interface ITransaction<T> where T : DbContext
    {
        void ClearTracker();
        Task BeginTransaction();
        Task CommitTransaction();
        Task RollbackTransaction();
    }
} 
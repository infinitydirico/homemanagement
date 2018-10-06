using Microsoft.EntityFrameworkCore.Storage;

namespace HomeManagement.Data
{
    public interface ITransactonalRepository
    {
        IDbContextTransaction BeginTransaction();

        void Commit();
    }
}

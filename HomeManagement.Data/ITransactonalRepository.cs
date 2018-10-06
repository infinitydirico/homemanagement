using System.Data.Common;

namespace HomeManagement.Data
{
    public interface ITransactonalRepository
    {
        DbTransaction BeginTransaction();

        DbTransaction GetCurrentTransaction();

        void CommitData();
    }
}

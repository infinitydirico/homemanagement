using HomeManagement.Contracts;
using HomeManagement.Domain;

namespace HomeManagement.Business.Contracts
{
    public interface IExportableTransaction : IExportable<Transaction>
    {
    }
}

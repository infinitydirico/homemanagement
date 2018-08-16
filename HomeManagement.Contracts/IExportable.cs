using System.Collections.Generic;

namespace HomeManagement.Contracts
{
    public interface IExportable
    {
        IList<string> GetProperties();
    }

    public interface IExportable<TEntity> : IExportable
        where TEntity : IExportable
    {
        byte[] ToCsv(List<TEntity> collection);

        List<TEntity> ToEntities(byte[] rawData);
    }
}

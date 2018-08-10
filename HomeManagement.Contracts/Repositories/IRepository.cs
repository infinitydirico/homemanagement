namespace HomeManagement.Contracts.Repositories
{
    public interface IRepository
    {
        void Add(object value);

        object GetById(int id);
    }
}

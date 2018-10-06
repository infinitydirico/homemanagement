namespace HomeManagement.Data
{
    public interface IWrittableRepository<T>
    {
        void Add(T entity);

        void Remove(T entity);

        void Update(T entity);
    }
}

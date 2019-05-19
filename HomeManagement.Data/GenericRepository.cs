namespace HomeManagement.Data
{
    public class GenericRepository<T> : BaseRepository<T>
        where T : class
    {
        public GenericRepository(IPlatformContext platformContext) : base(platformContext)
        {
        }

        public override bool Exists(T entity) => false;

        public override T GetById(int id) => null;
    }
}

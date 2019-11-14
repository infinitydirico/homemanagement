using Microsoft.EntityFrameworkCore;

namespace HomeManagement.Data
{
    public class GenericRepository<T> : BaseRepository<T>
        where T : class
    {
        public GenericRepository(DbContext context) : base(context)
        {
        }

        public override bool Exists(T entity) => false;

        public override T GetById(int id) => null;
    }
}

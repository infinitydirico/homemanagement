using Microsoft.EntityFrameworkCore;

namespace HomeManagement.Data
{
    public interface IPlatformContext
    {
        DbContext GetDbContext();

        void Commit();
    }
}

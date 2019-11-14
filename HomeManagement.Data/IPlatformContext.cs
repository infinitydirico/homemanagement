using Microsoft.EntityFrameworkCore;

namespace HomeManagement.Data
{
    public interface IPlatformContext
    {
        DbContext CreateDbContext();

        void Commit();
    }
}

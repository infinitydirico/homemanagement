using Microsoft.EntityFrameworkCore;

namespace HomeManagement.Data
{
    public interface IPlatformContext
    {
        DbContext GetDbContext();

        DbContext CreateContext();
    }
}

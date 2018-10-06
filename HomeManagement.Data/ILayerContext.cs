using Microsoft.EntityFrameworkCore;

namespace HomeManagement.Data
{
    public interface IPlatformContext : ITransactonalRepository
    {
        DbContext GetDbContext();
    }
}

using Microsoft.EntityFrameworkCore;

namespace Sender.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }

        public DbSet<MoneyTransfer> MoneyTransfer { get; set; }
    }
}
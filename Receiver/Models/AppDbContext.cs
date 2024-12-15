using Microsoft.EntityFrameworkCore;

namespace Receiver.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext()
        {
        }
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            if (!options.IsConfigured)
            {
                options.UseSqlServer("Data Source = (localdb)\\v11.0; Initial Catalog = SagaPattern; Integrated Security = True; Connect Timeout = 30; Encrypt = False; Trust Server Certificate = False; Application Intent = ReadWrite; Multi Subnet Failover = False");
            }
        }
        public DbSet<MoneyTransfer> MoneyTransfer { get; set; }
        public DbSet<Account> Account { get; set; }

    }
}
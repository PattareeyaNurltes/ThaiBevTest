using ClassLib.Data.Entities;
using ClassLib.Entities;
using Microsoft.EntityFrameworkCore;

public class PattareeyaDbContext : DbContext
{
    public PattareeyaDbContext(DbContextOptions<PattareeyaDbContext> options) : base(options) { }

    public DbSet<Accounts> Accounts { get; set; }
    public DbSet<AccountProfile> AccountProfile { get; set; }
    public DbSet<LoginInfomations> LoginInfomations { get; set; }
}

using Microsoft.EntityFrameworkCore;
using Ong.Infra.Tables;

namespace Ong.Infra
{
    public class OngDbContext : DbContext
    {
        public OngDbContext(DbContextOptions<OngDbContext> options) : base(options) { }

        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<Campaign> Campaigns { get; set; }
    }
}

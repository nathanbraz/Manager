using Manager.Domain.Entities;
using Manager.Infra.Mappings;
using Microsoft.EntityFrameworkCore;

namespace Manager.Infra.Context
{
    public class ManagerContext : DbContext
    {
        public ManagerContext() { }

        public ManagerContext(DbContextOptions<ManagerContext> options) : base(options) { }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            string connection = "server=localhost; port=3306; database=manager; user=root; password=root;";
            optionsBuilder.UseMySql(connection, ServerVersion.AutoDetect(connection));
        }

        public virtual DbSet<User> Users { get; set;}

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder.ApplyConfiguration(new UserMap());
        }

        
    }
}
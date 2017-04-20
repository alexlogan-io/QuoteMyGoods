using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace QuoteMyGoods.Models
{
    public class QMGContext:IdentityDbContext<QMGUser>
    {
        public QMGContext(DbContextOptions<QMGContext> options)
            : base(options)
        {
            Database.EnsureCreated();
        }

        public DbSet<Product> Products { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            foreach (var entity in builder.Model.GetEntityTypes())
            {
                entity.Relational().TableName = entity.ClrType.Name;
            }
            base.OnModelCreating(builder);
        }
    }
}

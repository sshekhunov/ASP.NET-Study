using Microsoft.EntityFrameworkCore;

namespace PromoCodeFactory.DataAccess.Contexts
{
    public class PromoDbContext: DbContext
    {
        public PromoDbContext(DbContextOptions<PromoDbContext> options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

        }
    }
}

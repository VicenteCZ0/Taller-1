using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using APITaller1.src.data;

namespace APITaller1.src.data
{
    public class DesignTimeDbContextFactory : IDesignTimeDbContextFactory<StoreContext>
    {
        public StoreContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<StoreContext>();
            optionsBuilder.UseSqlite("Data Source=store.db"); // o tu conexión real

            return new StoreContext(optionsBuilder.Options);
        }
    }
}
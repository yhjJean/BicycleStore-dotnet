using Microsoft.EntityFrameworkCore;
using BicycleStore.Models;

namespace BicycleStore.DataLayer
{
    public class DbcontextSales : DbContext
    {
        public DbcontextSales(DbContextOptions options) : base(options)
        {
        }

        public DbSet<rental> rental{ get; set; }
    }
}

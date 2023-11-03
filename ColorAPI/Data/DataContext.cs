using Microsoft.EntityFrameworkCore;

namespace ColorAPI.Data
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options) { }
        public DbSet<Color> Colors { get; set; }
        public DbSet<Palette> Palettes { get; set; }
    }
}

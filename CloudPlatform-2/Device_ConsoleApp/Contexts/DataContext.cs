using Device_ConsoleApp.Models;
using Microsoft.EntityFrameworkCore;

namespace Device_ConsoleApp.Contexts
{
    internal class DataContext : DbContext
    {
        #region constructors 
        public DataContext()
        {
        }

        public DataContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        #endregion

        #region overrides

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if(!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlServer(@"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Users\HansMattin-Lassei\Documents\Utbildning\NETFUND22-2\cloud\CloudPlatform-2\Device_ConsoleApp\Contexts\device_db.mdf;Integrated Security=True;Connect Timeout=30");
            }
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
        }

        #endregion


        public DbSet<DeviceEntity> Device { get; set; }

    }
}

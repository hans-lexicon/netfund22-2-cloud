using AzureFunctions.Models;
using Microsoft.EntityFrameworkCore;

namespace AzureFunctions.Contexts
{
    public class CosmosDbContext : DbContext
    {
        #region constructors

        public CosmosDbContext()
        {
        }

        public CosmosDbContext(DbContextOptions options) : base(options)
        {
        }

        #endregion

        #region overrides 

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<TelemetryEntity>().ToContainer("Telemetry").HasPartitionKey(x => x.PartitionKey);
        }

        #endregion

        public DbSet<TelemetryEntity> Telemetry { get; set; }

    }
}

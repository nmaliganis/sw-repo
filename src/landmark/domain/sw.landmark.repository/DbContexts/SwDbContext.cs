using sw.landmark.model;
using Microsoft.EntityFrameworkCore;

namespace sw.landmark.repository.DbContexts
{
    public partial class swDbContext : DbContext
    {
        public swDbContext(DbContextOptions<swDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<EventHistory> EventHistories { get; set; }

        public virtual DbSet<EventPosition> EventPositions { get; set; }

        public virtual DbSet<GeocodedPosition> GeocodedPositions { get; set; }

        public virtual DbSet<GeocoderProfile> GeocoderProfiles { get; set; }

        public virtual DbSet<Landmark> Landmarks { get; set; }

        public virtual DbSet<LandmarkCategory> LandmarkCategories { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder
                .ApplyConfigurationsFromAssembly(typeof(swDbContext).Assembly)
                .HasPostgresExtension("postgis");

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

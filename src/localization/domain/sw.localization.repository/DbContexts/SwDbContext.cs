using sw.localization.model.Localizations;
using Microsoft.EntityFrameworkCore;
using System;

namespace sw.localization.repository
{
    public partial class swDbContext : DbContext
    {
        public swDbContext(DbContextOptions<swDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<LocalizationDomain> LocalizationDomains { get; set; }
        public virtual DbSet<LocalizationLanguage> LocalizationLanguages { get; set; }
        public virtual DbSet<LocalizationValue> LocalizationValues { get; set; }

        //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        //{
        //    if (!optionsBuilder.IsConfigured)
        //    {
        //        optionsBuilder.UseNpgsql("Server=dbserver.sw.gr;Database=swDb;User Id=sa;Password=!Empha1#;",
        //            options => options.EnableRetryOnFailure(
        //            maxRetryCount: 4,
        //            maxRetryDelay: TimeSpan.FromSeconds(1),
        //            errorCodesToAdd: new string[] { }
        //        ));
        //    }
        //}

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //Database.SetInitializer<swDbContext>(null);

            //modelBuilder.HasAnnotation("Relational:Collation", "Greek_CI_AS");

            modelBuilder.ApplyConfigurationsFromAssembly(typeof(swDbContext).Assembly);

            OnModelCreatingPartial(modelBuilder);
        }

        partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
    }
}

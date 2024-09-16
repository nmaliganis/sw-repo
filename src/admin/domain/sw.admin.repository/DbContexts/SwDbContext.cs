using sw.admin.model;
using Microsoft.EntityFrameworkCore;

namespace sw.admin.repository.DbContexts
{
    public partial class swDbContext : DbContext
    {
        public swDbContext(DbContextOptions<swDbContext> options)
            : base(options)
        {
        }

        public virtual DbSet<Company> Companies { get; set; }

        public virtual DbSet<Department> Departments { get; set; }

        public virtual DbSet<DepartmentPersonRole> DepartmentPersonRoles { get; set; }

        public virtual DbSet<Person> Persons { get; set; }

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

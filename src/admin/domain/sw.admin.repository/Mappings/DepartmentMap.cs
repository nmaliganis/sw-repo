using sw.admin.model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace sw.admin.repository.Mappings
{
    public class DepartmentMap : IEntityTypeConfiguration<Department>
    {
        public void Configure(EntityTypeBuilder<Department> builder)
        {
            builder.ToTable("Department");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                .HasColumnName("id");

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(250)
                .HasColumnName("name");

            builder.Property(e => e.Notes)
                .HasColumnName("notes");

            builder.Property(e => e.CodeErp)
                .IsRequired()
                .HasMaxLength(150)
                .HasColumnName("codeErp");

            builder.Property(e => e.CompanyId)
                .IsRequired()
                .HasColumnName("companyId");

            // Constraints
            builder.HasOne(e => e.Company)
                .WithMany(p => p.Departments)
                .HasForeignKey(e => e.CompanyId)
                .OnDelete(DeleteBehavior.ClientSetNull);


            // TODO: When EntityBase is enriched with the new props
            // create a base generic extension to do the same binding in all mappings
            builder.Property(e => e.Active)
                .IsRequired()
                .HasColumnName("active");

            builder.Property(e => e.CreatedDate)
                .IsRequired()
                .HasColumnName("created_date");

            builder.Property(e => e.CreatedBy)
                .IsRequired()
                .HasColumnName("created_by");

            builder.Property(e => e.ModifiedDate)
                .IsRequired()
                .HasColumnName("modified_date");

            builder.Property(e => e.ModifiedBy)
                .IsRequired()
                .HasColumnName("modified_by");

            builder.Property(e => e.DeletedDate)
                .IsRequired()
                .HasColumnName("deleted_date");

            builder.Property(e => e.DeletedBy)
                .IsRequired()
                .HasColumnName("deleted_by");

            builder.Property(e => e.DeletedReason)
                .HasColumnName("deleted_reason");
        }
    }
}

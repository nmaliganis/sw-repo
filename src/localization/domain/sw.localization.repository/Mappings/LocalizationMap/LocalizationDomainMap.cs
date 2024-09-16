using sw.localization.model.Localizations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace sw.localization.repository.Mappings.LocalizationMap
{
    public class LocalizationDomainMap : IEntityTypeConfiguration<LocalizationDomain>
    {
        public void Configure(EntityTypeBuilder<LocalizationDomain> builder)
        {
            builder.HasKey(e => e.Id);

            builder.ToTable("LocalizationDomain");

            builder.Property(e => e.Id)
                .HasColumnName("id");

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(500)
                .HasColumnName("name");

            //builder.Property(e => e.Revision)
            //    .IsRequired()
            //    .HasColumnName("ldorevision");

            builder.HasMany(e => e.LocalizationValues)
                .WithOne(e => e.LocalizationDomain);



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
                .IsRequired()
                .HasColumnName("deleted_reason");
        }
    }
}
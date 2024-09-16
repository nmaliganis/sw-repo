using sw.localization.model.Localizations;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace sw.repository.Mappings.Roles
{
    public class LocalizationValueMap : IEntityTypeConfiguration<LocalizationValue>
    {
        public void Configure(EntityTypeBuilder<LocalizationValue> builder)
        {
            builder.ToTable("LocalizationValue");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                .HasColumnName("id");

            builder.Property(e => e.LanguageId)
                .IsRequired()
                .HasColumnName("languageId");

            builder.Property(e => e.DomainId)
                .IsRequired()
                .HasColumnName("domainId");

            builder.Property(e => e.Key)
                .IsRequired()
                .HasMaxLength(500)
                .HasColumnName("key");

            builder.Property(e => e.Value)
                .IsRequired()
                .HasMaxLength(500)
                .HasColumnName("value");

            //builder.Property(e => e.Revision)
            //    .IsRequired()
            //    .HasColumnName("lvarevision");

            builder.HasOne(d => d.LocalizationDomain)
                .WithMany(p => p.LocalizationValues)
                .HasForeignKey(d => d.DomainId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasOne(d => d.LocalizationLanguage)
                .WithMany(p => p.LocalizationValues)
                .HasForeignKey(d => d.LanguageId)
                .OnDelete(DeleteBehavior.ClientSetNull);

            builder.HasIndex(p => new { p.LanguageId, p.DomainId, p.Key })
                .IsUnique(true);




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
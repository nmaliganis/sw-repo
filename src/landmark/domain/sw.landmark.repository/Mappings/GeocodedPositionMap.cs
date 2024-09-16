using sw.landmark.model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace sw.landmark.repository.Mappings
{
    public class CompanyMap : IEntityTypeConfiguration<GeocodedPosition>
    {
        public void Configure(EntityTypeBuilder<GeocodedPosition> builder)
        {
            builder.ToTable("Company");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                .HasColumnName("id");

            builder.Property(e => e.GeocoderProfileId)
                .IsRequired()
                .HasColumnName("geocoderProfileId");

            builder.Property(e => e.Position)
                .IsRequired()
                .HasColumnType("geometry (point)")
                .HasColumnName("point");

            builder.Property(e => e.Street)
                .HasMaxLength(300)
                .HasColumnName("street");

            builder.Property(e => e.Number)
                .HasMaxLength(100)
                .HasColumnName("number");

            builder.Property(e => e.CrossStreet)
                .HasMaxLength(100)
                .HasColumnName("crossstreet");

            builder.Property(e => e.City)
                .HasMaxLength(100)
                .HasColumnName("city");

            builder.Property(e => e.Prefecture)
                .HasMaxLength(100)
                .HasColumnName("prefecture");

            builder.Property(e => e.Country)
                .HasMaxLength(100)
                .HasColumnName("country");

            builder.Property(e => e.Zipcode)
                .HasMaxLength(100)
                .HasColumnName("zipcode");

            builder.Property(e => e.LastGeocoded)
                .IsRequired()
                .HasColumnName("lastGeocoded");

            // Constraints
            builder.HasOne(e => e.GeocoderProfile)
                .WithMany(p => p.GeocodedPositions)
                .HasForeignKey(e => e.GeocoderProfileId)
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

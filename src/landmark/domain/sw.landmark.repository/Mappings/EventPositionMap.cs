using sw.landmark.model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace sw.landmark.repository.Mappings
{
    internal class AssetMap : IEntityTypeConfiguration<EventPosition>
    {
        public void Configure(EntityTypeBuilder<EventPosition> builder)
        {
            builder.ToTable("EventPosition");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                .HasColumnName("id");

            builder.Property(e => e.GeocodedPositionId)
                .IsRequired()
                .HasColumnName("geocodedPositionId");

            builder.Property(e => e.Position)
                .IsRequired()
                .HasColumnType("geometry (point)")
                .HasColumnName("position");

            // Constraints
            builder.HasOne(e => e.GeocodedPosition)
                .WithMany(p => p.EventPositions)
                .HasForeignKey(e => e.GeocodedPositionId)
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

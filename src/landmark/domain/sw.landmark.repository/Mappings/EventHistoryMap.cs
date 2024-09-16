using sw.landmark.model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace sw.landmark.repository.Mappings
{
    internal class AssetCategoryMap : IEntityTypeConfiguration<EventHistory>
    {
        public void Configure(EntityTypeBuilder<EventHistory> builder)
        {
            builder.ToTable("EventHistory");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                .HasColumnName("id");

            builder.Property(e => e.EventPositionId)
                .IsRequired()
                .HasColumnName("eventPositionId");

            builder.Property(e => e.SensorId)
                .IsRequired()
                .HasColumnName("sensorId");

            builder.Property(e => e.Recorded)
                .IsRequired()
                .HasColumnName("recorded");

            builder.Property(e => e.Received)
                .IsRequired()
                .HasColumnName("received");

            builder.Property(e => e.EventValue)
                .HasColumnName("eventValue");

            builder.Property(e => e.EventValueJson)
                .HasColumnType("jsonb")
                .HasColumnName("eventvaluejson");

            // Constraints
            builder.HasOne(e => e.EventPosition)
                .WithMany(p => p.EventHistories)
                .HasForeignKey(e => e.EventPositionId)
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

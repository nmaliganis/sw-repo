using sw.landmark.model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace sw.landmark.repository.Mappings
{
    internal class LandmarkMap : IEntityTypeConfiguration<Landmark>
    {
        public void Configure(EntityTypeBuilder<Landmark> builder)
        {
            builder.ToTable("Landmark");

            builder.HasKey(e => e.Id);

            builder.Property(e => e.Id)
                .HasColumnName("id");

            builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(100)
                .HasColumnName("name");

            builder.Property(e => e.CodeErp)
                .IsRequired()
                .HasMaxLength(150)
                .HasColumnName("codeerp");

            builder.Property(e => e.Description)
                .HasColumnType("text")
                .HasColumnName("description");

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

            builder.Property(e => e.PhoneNumber)
                .HasMaxLength(100)
                .HasColumnName("phonenumber");

            builder.Property(e => e.PhoneNumber2)
                .HasMaxLength(100)
                .HasColumnName("phonenumber2");

            builder.Property(e => e.Email)
                .HasMaxLength(300)
                .HasColumnName("email");

            builder.Property(e => e.Fax)
                .HasMaxLength(100)
                .HasColumnName("fax");

            builder.Property(e => e.Url)
                .HasMaxLength(100)
                .HasColumnName("url");

            builder.Property(e => e.PersonInCharge)
                .HasMaxLength(100)
                .HasColumnName("personincharge");

            builder.Property(e => e.Vat)
                .HasMaxLength(100)
                .HasColumnName("vat");

            builder.Property(e => e.Image)
                .HasMaxLength(500)
                .HasColumnName("image");

            builder.Property(e => e.IsBase)
                .IsRequired()
                .HasColumnName("isbase");

            builder.Property(e => e.ExcludeFromSpace)
                .IsRequired()
                .HasColumnName("excludefromspace");

            builder.Property(e => e.HasSpacePriority)
                .IsRequired()
                .HasColumnName("hasspacepriority");

            builder.Property(e => e.SpeedLimit)
                .IsRequired()
                .HasColumnName("speedlimit");

            builder.Property(e => e.Expired)
                .IsRequired()
                .HasColumnName("expired");

            builder.Property(e => e.RootId)
                .IsRequired()
                .HasColumnName("rootId");

            builder.Property(e => e.ParentId)
                .IsRequired()
                .HasColumnName("parentId");

            builder.Property(e => e.LandmarkCategoryId)
                .IsRequired()
                .HasColumnName("landmarkCategoryId");

            // Constraints
            builder.HasOne(e => e.LandmarkCategory)
                .WithMany(p => p.Landmarks)
                .HasForeignKey(e => e.LandmarkCategoryId)
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

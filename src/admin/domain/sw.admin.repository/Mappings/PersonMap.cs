using sw.admin.model;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace sw.admin.repository.Mappings;
public class PersonMap : IEntityTypeConfiguration<Person>
{
  public void Configure(EntityTypeBuilder<Person> builder)
  {
    builder.ToTable("Person");

    builder.HasKey(e => e.Id);

    builder.Property(e => e.Id)
      .HasColumnName("id");

    builder.Property(e => e.FirstName)
      .IsRequired()
      .HasMaxLength(250)
      .HasColumnName("firstname");

    builder.Property(e => e.LastName)
      .IsRequired()
      .HasMaxLength(250)
      .HasColumnName("lastname");

    builder.Property(e => e.Gender)
      .IsRequired()
      .HasColumnName("gender");

    builder.Property(e => e.Phone)
      .HasMaxLength(10)
      .HasColumnName("phone");

    builder.Property(e => e.ExtPhone)
      .HasMaxLength(5)
      .HasColumnName("extphone");

    builder.Property(e => e.Notes)
      .HasColumnName("notes");

    builder.Property(e => e.Email)
      .IsRequired()
      .HasMaxLength(128)
      .HasColumnName("email");

    builder.Property(e => e.AddressStreet1)
      .HasMaxLength(128)
      .HasColumnName("addressstreet1");

    builder.Property(e => e.AddressStreet2)
      .HasMaxLength(128)
      .HasColumnName("addressstreet2");

    builder.Property(e => e.AddressPostCode)
      .HasMaxLength(8)
      .HasColumnName("addresspostcode");

    builder.Property(e => e.AddressCity)
      .HasMaxLength(64)
      .HasColumnName("addresscity");

    builder.Property(e => e.AddressRegion)
      .HasMaxLength(64)
      .HasColumnName("addressregion");

    builder.Property(e => e.Mobile)
      .HasMaxLength(10)
      .HasColumnName("mobile");

    builder.Property(e => e.ExtMobile)
      .HasMaxLength(5)
      .HasColumnName("extmobile");

    builder.Property(e => e.Status)
      .IsRequired()
      .HasColumnName("status");

    builder.Property(e => e.PersonRoleId)
      .IsRequired()
      .HasColumnName("personRoleId");

    // Constraints
    builder.HasOne(e => e.DepartmentPersonRole)
      .WithMany(p => p.Persons)
      .HasForeignKey(e => e.PersonRoleId)
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
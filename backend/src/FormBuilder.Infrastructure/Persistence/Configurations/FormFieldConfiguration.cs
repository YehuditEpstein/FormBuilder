using FormBuilder.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FormBuilder.Infrastructure.Persistence.Configurations;

public class FormFieldConfiguration : IEntityTypeConfiguration<FormField>
{
    public void Configure(EntityTypeBuilder<FormField> builder)
    {
        builder.ToTable("FormFields");

        builder.HasKey(f => f.Id);

        builder.Property(f => f.Label)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(f => f.Type)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(30);

        builder.Property(f => f.OrderIndex)
            .IsRequired();

        builder.Property(f => f.IsRequired)
            .IsRequired();
    }
}

using FormBuilder.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FormBuilder.Infrastructure.Persistence.Configurations;

public class FormTemplateConfiguration : IEntityTypeConfiguration<FormTemplate>
{
    public void Configure(EntityTypeBuilder<FormTemplate> builder)
    {
        builder.ToTable("FormTemplates");

        builder.HasKey(t => t.Id);

        builder.Property(t => t.Name)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(t => t.CreatedBy)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(t => t.CreatedAt)
            .IsRequired();

        // Fields/ApprovalSteps are exposed as read-only collections backed by private
        // lists (_fields / _approvalSteps) — tell EF Core to materialize through those fields.
        builder.HasMany(t => t.Fields)
            .WithOne(f => f.FormTemplate)
            .HasForeignKey(f => f.FormTemplateId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(t => t.Fields)
            .HasField("_fields")
            .UsePropertyAccessMode(PropertyAccessMode.Field);

        builder.HasMany(t => t.ApprovalSteps)
            .WithOne(s => s.FormTemplate)
            .HasForeignKey(s => s.FormTemplateId)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Navigation(t => t.ApprovalSteps)
            .HasField("_approvalSteps")
            .UsePropertyAccessMode(PropertyAccessMode.Field);
    }
}

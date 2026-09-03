using FormBuilder.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace FormBuilder.Infrastructure.Persistence.Configurations;

public class ApprovalStepConfiguration : IEntityTypeConfiguration<ApprovalStep>
{
    public void Configure(EntityTypeBuilder<ApprovalStep> builder)
    {
        builder.ToTable("ApprovalSteps");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.StepOrder)
            .IsRequired();

        builder.Property(s => s.StepName)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(s => s.ApproverIdentity)
            .IsRequired()
            .HasMaxLength(200);

        builder.Property(s => s.ActionType)
            .IsRequired()
            .HasConversion<string>()
            .HasMaxLength(30);
    }
}

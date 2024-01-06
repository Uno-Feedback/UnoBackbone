using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Uno.Domain.Entities;

namespace Uno.Infrastructer.EntitesConfigs;

public class IssueAttachmentConfigurations : IEntityTypeConfiguration<IssueAttachment>
{
    public void Configure(EntityTypeBuilder<IssueAttachment> builder)
    {
        builder.ToTable(t =>
                        t.HasComment("Attachments with each Issue"));


    

        builder.HasKey(t => t.Id)
               .HasName("PK_Base_IssueAttachment");

        builder.HasIndex(x => x.IssueId)
               .HasDatabaseName("IX_Base_IssueAttachment(IssueId)");

        builder.HasOne(a => a.Issue)
               .WithMany(i => i.Attachments)
               .HasForeignKey(a => a.IssueId)
               .HasConstraintName("FK_Base_IssueAttachment_Base_Issue");

        builder.Property(x => x.Type)
           .HasComment("Space-efficient representation for C# enum values");

        builder.Property(x => x.Content)
                .HasMaxLength(5000);
    }
}

using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Uno.Domain.Entities;

namespace Uno.Infrastructure.EntitesConfigs;

public class IssueConfigurations : IEntityTypeConfiguration<Issue>
{
    public void Configure(EntityTypeBuilder<Issue> builder)
    {
        builder.ToTable(t =>
                        t.HasComment("User recordings to be sent to the project management platform"));

        builder.HasKey(t => t.Id)
               .HasName("PK_Base_Issue");

        builder.HasIndex(x => x.ProjectId)
               .HasDatabaseName("IX_Base_Issue(ProjectId)");

        builder.HasOne(i => i.Project)
               .WithMany(p => p.Issues)
               .HasForeignKey(i => i.ProjectId)
               .HasConstraintName("FK_Base_Issue_Base_Project");

        builder.HasMany(x => x.Attachments)
               .WithOne(x => x.Issue)
               .HasForeignKey(x => x.IssueId);

        builder.HasMany(x => x.ConnectorInIssues)
               .WithOne(x => x.Issue)
               .HasForeignKey(x => x.IssueId);

        builder.Property(x => x.Subject)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(x => x.Description)
               .IsRequired()
               .HasMaxLength(600);
    }
}

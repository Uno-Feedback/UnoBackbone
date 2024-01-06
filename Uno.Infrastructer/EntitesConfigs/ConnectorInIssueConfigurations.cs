using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Uno.Domain.Entities;

namespace Uno.Infrastructer.EntitesConfigs;

public class ConnectorInIssueConfigurations : IEntityTypeConfiguration<ConnectorInIssue>
{
    public void Configure(EntityTypeBuilder<ConnectorInIssue> builder)
    {
        builder.ToTable(t =>
                        t.HasComment("Stores many-to-many relationships between isuues and connectors"));

        builder.HasKey(t => t.Id)
               .HasName("PK_Base_ConnectorInIssue");

        builder.HasIndex(x => x.ConnectorId)
                  .HasDatabaseName("IX_Base_ConnectorInIssue(ConnectorId)");

        builder.HasIndex(x => x.IssueId)
                   .HasDatabaseName("IX_Base_ConnectorInIssue(IssueId)");

        builder.HasOne(cis => cis.Connector)
               .WithMany(c => c.ConnectorInIssues)
               .HasForeignKey(cis => cis.ConnectorId)
               .HasConstraintName("FK_Base_ConnectorInIssue_Base_Connector");

        builder.HasOne(cis => cis.Issue)
               .WithMany(i => i.ConnectorInIssues)
               .HasForeignKey(cis => cis.IssueId)
               .HasConstraintName("FK_Base_ConnectorInIssue_Base_Issue");

        builder.Property(x => x.ConnectorMetaData)
               .IsRequired()
               .HasMaxLength(150);   
        
        builder.Property(x => x.ClientMetaData)
               .IsRequired()
               .HasMaxLength(750);

        builder.Property(x => x.IssueMetaData)
               .IsRequired(false)
               .HasMaxLength(100);
    }
}

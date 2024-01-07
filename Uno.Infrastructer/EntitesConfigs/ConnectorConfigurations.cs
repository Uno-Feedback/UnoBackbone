using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Uno.Domain.Entities;

namespace Uno.Infrastructure.EntitesConfigs;

public class ConnectorConfigurations : IEntityTypeConfiguration<Connector>
{
    public void Configure(EntityTypeBuilder<Connector> builder)
    {
        builder.ToTable(t =>
                   t.HasComment("Information on project control platforms for submitting issues"));

        builder.Property(x => x.Type)
               .HasComment("Space-efficient representation for C# enum values");

        builder.HasKey(t => t.Id)
               .HasName("PK_Base_Connector");

        builder.HasIndex(x => x.ProjectId)
            .HasDatabaseName("IX_Base_Connector(ProjectId)");

        builder.HasOne(c => c.Project)
            .WithMany(p => p.Connectors)
            .HasForeignKey(c => c.ProjectId)
            .HasConstraintName("FK_Base_Connector_Base_Project");

        builder.HasMany(x => x.ConnectorInIssues)
               .WithOne(x => x.Connector)
               .HasForeignKey(x => x.ConnectorId);

        builder.Property(x => x.UserName)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(x => x.Password)
               .IsRequired()
               .HasMaxLength(100);

        builder.Property(x => x.Url)
               .IsRequired()
               .HasMaxLength(100);

        builder.Ignore(x => x.TypeDescription);

    }
}

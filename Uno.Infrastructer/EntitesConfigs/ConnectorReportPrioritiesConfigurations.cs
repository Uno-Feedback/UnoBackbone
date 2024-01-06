using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Uno.Domain.Entities;

namespace Uno.Infrastructer.EntitesConfigs;

public class ConnectorReportPrioritiesConfigurations : IEntityTypeConfiguration<ConnectorReportPriorities>
{
    public void Configure(EntityTypeBuilder<ConnectorReportPriorities> builder)
    {
        builder.ToTable(t =>
                        t.HasComment("Stores connectors report priorities like bug or sub-task"));

        builder.HasKey(t => t.Id)
               .HasName("PK_Base_ConnectorReportPriorities");

        builder.HasIndex(x => x.ConnectorId)
               .HasDatabaseName("IX_Base_ConnectorReprtPriorities(ConnectorId)");

        builder.HasOne(cis => cis.Connector)
               .WithMany(c => c.ConnectorReportPriorities)
               .HasForeignKey(cis => cis.ConnectorId)
               .HasConstraintName("FK_Base_ConnetorReportPriorities_Base_Connector");
    }
}

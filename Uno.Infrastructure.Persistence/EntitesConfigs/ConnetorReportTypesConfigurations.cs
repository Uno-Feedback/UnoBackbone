using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Uno.Domain.Entities;

namespace Uno.Infrastructure.EntitesConfigs;

public class ConnetorReportTypesConfigurations : IEntityTypeConfiguration<ConnectorReportTypes>
{
    public void Configure(EntityTypeBuilder<ConnectorReportTypes> builder)
    {
        builder.ToTable(t =>
                        t.HasComment("Stores connectors report types like bug or sub-task"));

        builder.HasKey(t => t.Id)
               .HasName("PK_Base_ConnectorReportType");

        builder.HasIndex(x => x.ConnectorId)
               .HasDatabaseName("IX_Base_ConnectorReprtTypes(ConnectorId)");

        builder.HasOne(cis => cis.Connector)
               .WithMany(c => c.ConnectorReportTypes)
               .HasForeignKey(cis => cis.ConnectorId)
               .HasConstraintName("FK_Base_ConnetorReportTypes_Base_Connector");
    }
}

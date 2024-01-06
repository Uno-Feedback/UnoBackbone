using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Uno.Domain.Entities;

namespace Uno.Infrastructer.EntitesConfigs;

public class ProjectConfigurations : IEntityTypeConfiguration<Project>
{
    public void Configure(EntityTypeBuilder<Project> builder)
    {
        builder.ToTable(t =>
                        t.HasComment("Project information in project management software"));

        builder.HasKey(t => t.Id)
               .HasName("PK_Base_Project");

        builder.HasIndex(x => x.UserId)
               .HasDatabaseName("IX_Base_Project(UserId)");

        builder.HasOne(p => p.User)
               .WithMany(u => u.Projects)
               .HasForeignKey(p => p.UserId)
               .HasConstraintName("FK_Base_Project_Base_User");

        builder.HasMany(x => x.Issues)
               .WithOne(x => x.Project)
               .HasForeignKey(x => x.ProjectId);

        builder.HasMany(x => x.Connectors)
               .WithOne(x => x.Project)
               .HasForeignKey(x => x.ProjectId);

        builder.Property(x => x.IP)
               .HasMaxLength(20);
    }
}

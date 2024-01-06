using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Uno.Domain.Entities;

namespace Uno.Infrastructer.EntitesConfigs;

public class UserConfigurations : IEntityTypeConfiguration<User>
{
    public void Configure(EntityTypeBuilder<User> builder)
    {
        builder.ToTable(t =>
                        t.HasComment("stores information about registered users in our application"));

        builder.HasKey(t => t.Id)
               .HasName("PK_Base_User");

        builder.Property(x => x.FirstName)
               .IsRequired()
               .HasMaxLength(50);

        builder.Property(x => x.LastName)
               .IsRequired()
               .HasMaxLength(50);

        builder.Property(x => x.Email)
               .IsRequired()
               .HasMaxLength(50);

        builder.HasMany(x => x.Projects)
               .WithOne(x => x.User)
               .HasForeignKey(x => x.UserId);
    }
}

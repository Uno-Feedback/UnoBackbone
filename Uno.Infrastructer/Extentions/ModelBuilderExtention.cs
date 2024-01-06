using System.Reflection;
using Uno.Domain.Common;

namespace Uno.Infrastructer.Extentions;

/// <summary>
/// This extention is programmed for registering Entities that are defined the EntityAttribute .
/// </summary>
public static class ModelBuilderExtention
{
    public static void RegisterEntities(this ModelBuilder modelBuilder, params Assembly[] assemblies)
    {
        IEnumerable<Type> types = assemblies.SelectMany(x => x.GetExportedTypes())
                                            .Where(x => x.IsClass && !x.IsAbstract && x.IsPublic && Attribute.IsDefined(x, typeof(EntityAttribute)));

        foreach (Type type in types)
            modelBuilder.Entity(type);
    }
}

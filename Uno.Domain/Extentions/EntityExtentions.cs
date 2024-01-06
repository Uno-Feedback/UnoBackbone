using Uno.Domain.Common;

namespace Uno.Domain.Extentions;

public static class EntityExtentions
{
    /// <summary>
    /// An Extention for mapping property values of source to destination object that have same property name.
    /// </summary>
    /// <typeparam name="TDestination"></typeparam>
    /// <typeparam name="TSource"></typeparam>
    /// <param name="destinationObject"></param>
    /// <param name="sourceObject"></param>
    /// <returns></returns>
    public static TDestination MapFrom<TDestination, TSource>(this TDestination destinationObject, TSource sourceObject) where TDestination : IEntity where TSource : class
    {
        var sourceObjectProperties = sourceObject.GetType().GetProperties();
        var destinationObjectProperties = destinationObject.GetType().GetProperties();

        foreach (var property in destinationObjectProperties)
        {
            try
            {
                var propertyInfo = sourceObjectProperties.FirstOrDefault((x) => x.Name == property.Name && x.CanWrite);
                if (propertyInfo != null)
                    property.SetValue(destinationObject, propertyInfo.GetValue(sourceObject));
            }
            catch
            {

            }
        }

        return destinationObject;
    }
}

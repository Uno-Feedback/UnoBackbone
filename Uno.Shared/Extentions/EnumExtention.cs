using System.ComponentModel;
using System.Reflection;

namespace Uno.Shared.Extentions;


public static class EnumExtention
{
    /// <summary>
    /// This Extention is programmed for valiating given value for an enum.
    /// If given value is not in enum the validation is failed.
    /// </summary>
    /// <typeparam name="TEnum"></typeparam>
    /// <param name="enumObj"></param>
    /// <returns></returns>
    public static bool IsValidEnumValue<TEnum>(this Enum enumObj) where TEnum : Enum
        => Enum.GetNames(typeof(TEnum)).Contains(enumObj.ToString());

    /// <summary>
    /// This Exteniton is programmed for recieving description of Enum field that implemented DescriptionAttribute.
    /// </summary>
    /// <param name="enumObj"></param>
    /// <returns></returns>
    public static string GetDescription(this Enum enumObj)
    {
        MemberInfo[] member = enumObj.GetType().GetMember(enumObj.ToString());
        if (member != null && member.Length != 0)
        {
            object[] customAttributes = member[0].GetCustomAttributes(typeof(DescriptionAttribute), inherit: false);
            if (customAttributes != null && customAttributes.Length != 0)
                return ((DescriptionAttribute)customAttributes[0]).Description;
        }

        return enumObj.ToString();
    }
}

using System.ComponentModel;
using System.Reflection;

namespace DL.GameOfLife.Domain;

public static class EnumExtensions
{
    public static string GetDescription<T>(this T enumValue, string defaultDescription = "No description") where T : Enum
    {
        var enumFieldName = enumValue.ToString();
        var field = typeof(T).GetField(enumFieldName);

        if (field is null)
        {
            return defaultDescription;
        }
        
        var attribute = field.GetCustomAttribute<DescriptionAttribute>();

        return attribute?.Description ?? defaultDescription;
    }
}

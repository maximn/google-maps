using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;

namespace GoogleMapsApi.Engine
{
    /// <summary>
    /// Resolves the <see cref="EnumMemberAttribute"/> wire value of an enum member, for requests
    /// that build query strings by hand (where the JSON converters don't apply).
    /// </summary>
    internal static class EnumMemberHelper
    {
        public static string GetValue<TEnum>(TEnum value) where TEnum : struct, Enum
        {
            var member = typeof(TEnum).GetMember(value.ToString()).FirstOrDefault();
            var attribute = member?.GetCustomAttribute<EnumMemberAttribute>();
            return attribute?.Value ?? value.ToString();
        }
    }
}

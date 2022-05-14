using System;

namespace StrEnum
{
    public static class TypeExtensions
    {
        /// <summary>
        /// Gets a value indicating whether the type is a StrEnum string enum.
        /// </summary>
        /// <param name="type">The type to check</param>
        /// <returns></returns>
        public static bool IsStringEnum(this Type type)
        {
            if (!type.IsClass) return false;

            var baseType = type.BaseType;

            if (baseType == null) return false;

            var baseClassIsStringEnumGeneric = baseType.IsGenericType && baseType.GetGenericTypeDefinition() == typeof(StringEnum<>);

            return baseClassIsStringEnumGeneric;
        }
    }
}
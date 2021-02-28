using System;

namespace MaterialFader
{
    public static class EnumExtensions
    {
        public static bool AsEnum<T>(this string name, out T val) where T : struct
            => Enum.TryParse(name, true, out val) && Enum.IsDefined(typeof(T), val);
    }
}

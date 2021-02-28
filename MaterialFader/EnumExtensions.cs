using System;
using System.Collections.Generic;
using System.Linq;

namespace MaterialFader
{
    public static class EnumExtensions
    {
        public static bool TryAsEnum<T>(this string name, out T val) where T : struct
            => Enum.TryParse(name, true, out val) && Enum.IsDefined(typeof(T), val);

        public static T? AsEnum<T>(this string name) where T : struct
            => Enum.TryParse<T>(name, true, out var val) && Enum.IsDefined(typeof(T), val)
                ? val
                : default;

        public static IEnumerable<T> AsValidEnums<T>(this IEnumerable<string> names) where T : struct
            => names.Select(b => b.AsEnum<T>())
                .Where(b => b.HasValue)
                .Select(b => b.Value);
    }
}

using System.Reflection;

namespace Extended.System
{
    /// <summary>
    /// The assembly extensions class.
    /// </summary>
    public static class AssemblyExtensions
    {
        /// <summary>
        /// Gets the inherited types using the specified assemblies.
        /// </summary>
        /// <param name="assemblies">The assemblies.</param>
        /// <param name="inheritedType">The inherited type.</param>
        /// <returns>The types from assemblies.</returns>
        public static IEnumerable<Type> GetInheritedTypes(this Assembly[] assemblies, Type inheritedType)
        {
            var typesFromAssemblies = assemblies.SelectMany(a => a.DefinedTypes.Where(t => !t.IsAbstract && t.IsClass && t.IsInherited(inheritedType)));
            return typesFromAssemblies;
        }

        /// <summary>
        /// Gets the inherited types using the specified assemblies.
        /// </summary>
        /// <typeparam name="T">The .</typeparam>
        /// <param name="assemblies">The assemblies.</param>
        /// <returns>An enumerable of type.</returns>
        public static IEnumerable<Type> GetInheritedTypes<T>(this Assembly[] assemblies)
        {
            var inheritedType = typeof(T);
            return assemblies.GetInheritedTypes(inheritedType);
        }

        /// <summary>
        /// Gets the assembly directory using the specified assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <exception cref="ArgumentException">Assembly is null.</exception>
        /// <returns>The string.</returns>
        public static string GetAssemblyDirectory(this Assembly? assembly)
        {
            if (assembly == null)
                throw new ArgumentException("Assembly is null", nameof(assembly));

            return assembly.Location;
        }

        /// <summary>
        /// Gets the linker time using the specified assembly.
        /// </summary>
        /// <param name="assembly">The assembly.</param>
        /// <param name="target">The target.</param>
        /// <returns>The local time.</returns>
        public static DateTime GetLinkerTime(this Assembly assembly, TimeZoneInfo? target = null)
        {
            assembly.CheckIsNull();

            var filePath = assembly.Location;
            const int c_PeHeaderOffset = 60;
            const int c_LinkerTimestampOffset = 8;

            var buffer = new byte[2048];

            using (var stream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                stream.Read(buffer, 0, 2048);

            var offset = BitConverter.ToInt32(buffer, c_PeHeaderOffset);
            var secondsSince1970 = BitConverter.ToInt32(buffer, offset + c_LinkerTimestampOffset);
            var epoch = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);

            var linkTimeUtc = epoch.AddSeconds(secondsSince1970);

            var tz = target ?? TimeZoneInfo.Local;
            var localTime = TimeZoneInfo.ConvertTimeFromUtc(linkTimeUtc, tz);

            return localTime;
        }
    }
}

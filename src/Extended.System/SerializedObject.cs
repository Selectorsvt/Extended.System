using Newtonsoft.Json;

namespace Extended.System
{
    /// <summary>
    /// The serialized object class.
    /// </summary>
    public class SerializedObject
    {
        /// <summary>
        /// Gets or sets the value of the source.
        /// </summary>
        public string? Source { get; set; }

        /// <summary>
        /// Gets or sets the value of the assembly qualified name.
        /// </summary>
        public string? AssemblyQualifiedName { get; set; }

        /// <summary>
        /// Creates the obj.
        /// </summary>
        /// <param name="obj">The obj.</param>
        /// <returns>The serialized object.</returns>
        public static SerializedObject Create(object obj)
        {
            return new SerializedObject()
            {
                AssemblyQualifiedName = obj.GetType().AssemblyQualifiedName,
                Source = JsonConvert.SerializeObject(obj)
            };
        }
    }
}

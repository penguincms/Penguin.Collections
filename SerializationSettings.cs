using System;

namespace Penguin.Collections
{
    public class SerializationSettings<T>
    {
        public Func<string, T> Deserialize { get; set; } = DefaultDeserialize;

        public Func<T, string> Serialize { get; set; } = DefaultSerialize;

        private static T DefaultDeserialize(string s)
        {
            return (T)Convert.ChangeType(s, typeof(T));
        }

        private static string DefaultSerialize(T t)
        {
            return $"{t}";
        }
    }
}
using System.Collections.Generic;
using System.Linq;

namespace Penguin.Collections
{
    public class DictionaryFile : DictionaryFile<string>
    {
        public DictionaryFile(string path, bool autoflush = true) : base(path, new SerializationSettings<string>()
        {
            Serialize = v => v,
            Deserialize = v => v
        }, autoflush)
        {
        }
    }

    public class DictionaryFile<T> : DictionaryFile<T, T>
    {
        public DictionaryFile(string path, SerializationSettings<T> serialization, bool autoflush = true) : base(path, serialization, serialization, autoflush)
        {
        }
    }
}
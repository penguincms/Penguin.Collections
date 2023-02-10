using System;

namespace Penguin.Collections.SerializationSettings
{
    public class DeserializationSettings<T> : SerializationSettings<T>
    {
        public DeserializationSettings(Func<string, T> deserializationFunc)
        {
            Serialize = k => $"{k}";
            Deserialize = deserializationFunc;
        }
    }
}
using System;
using System.Collections.Generic;
using System.Text;

namespace Penguin.Collections.SerializationSettings
{
    public class DeserializationSettings<T> : SerializationSettings<T>
    {
        public DeserializationSettings(Func<string, T> deserializationFunc)
        {
            this.Serialize = k => $"{k}";
            this.Deserialize = deserializationFunc;
        }
    }
}

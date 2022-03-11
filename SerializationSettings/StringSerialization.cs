using System;
using System.Collections.Generic;
using System.Text;

namespace Penguin.Collections.SerializationSettings
{
    public class StringSerialization : SerializationSettings<string>
    {
        public StringSerialization()
        {
            this.Serialize = k => k;
            this.Deserialize = v => v;
        }
    }
}

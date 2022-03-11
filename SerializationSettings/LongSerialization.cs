using System;
using System.Collections.Generic;
using System.Text;

namespace Penguin.Collections.SerializationSettings
{
    public class LongSerialization : DeserializationSettings<long>
    {
        public LongSerialization() : base(long.Parse)
        {
        }
    }
}

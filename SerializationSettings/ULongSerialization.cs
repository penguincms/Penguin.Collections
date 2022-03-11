using System;
using System.Collections.Generic;
using System.Text;

namespace Penguin.Collections.SerializationSettings
{
    public class ULongSerialization : DeserializationSettings<ulong>
    {
        public ULongSerialization() : base(ulong.Parse)
        {
        }
    }
}

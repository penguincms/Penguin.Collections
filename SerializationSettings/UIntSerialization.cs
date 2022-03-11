using System;
using System.Collections.Generic;
using System.Text;

namespace Penguin.Collections.SerializationSettings
{
    public class UIntSerialization : DeserializationSettings<uint>
    {
        public UIntSerialization() : base(uint.Parse)
        {
        }
    }
}

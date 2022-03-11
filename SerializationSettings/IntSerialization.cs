using System;
using System.Collections.Generic;
using System.Text;

namespace Penguin.Collections.SerializationSettings
{
    public class IntSerialization : DeserializationSettings<int>
    {
        public IntSerialization() : base(int.Parse)
        {
        }
    }
}

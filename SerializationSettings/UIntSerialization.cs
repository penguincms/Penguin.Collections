namespace Penguin.Collections.SerializationSettings
{
    public class UIntSerialization : DeserializationSettings<uint>
    {
        public UIntSerialization() : base(uint.Parse)
        {
        }
    }
}
namespace Penguin.Collections.SerializationSettings
{
    public class LongSerialization : DeserializationSettings<long>
    {
        public LongSerialization() : base(long.Parse)
        {
        }
    }
}
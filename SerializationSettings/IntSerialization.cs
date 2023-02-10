namespace Penguin.Collections.SerializationSettings
{
    public class IntSerialization : DeserializationSettings<int>
    {
        public IntSerialization() : base(int.Parse)
        {
        }
    }
}
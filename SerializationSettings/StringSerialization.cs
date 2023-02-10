namespace Penguin.Collections.SerializationSettings
{
    public class StringSerialization : SerializationSettings<string>
    {
        public StringSerialization()
        {
            Serialize = k => k;
            Deserialize = v => v;
        }
    }
}
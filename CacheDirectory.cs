using Penguin.Extensions.Strings;
using System.IO;

namespace Penguin.Collections
{
    public class CacheDirectory
    {
        private ListFile _backingList;

        public string CachePath { get; private set; }

        private ListFile BackingList
        {
            get
            {
                if (this._backingList is null)
                {
                    if (!Directory.Exists(this.CachePath))
                    {
                        Directory.CreateDirectory(this.CachePath);
                    }

                    this._backingList = new ListFile(Path.Combine(this.CachePath, "Index.cache"));
                }

                return this._backingList;
            }
        }

        public bool EncodeKey { get; private set; }

        public CacheDirectory(string path, bool encodeKey = true)
        {
            this.CachePath = path;
            this.EncodeKey = encodeKey;
        }

        public void Add(object okey, string value)
        {
            string key = $"{okey}";

            if (this.EncodeKey)
            {
                key = key.ToBase64();
            }

            int index = this.FindIndex(key);

            if (index == -1)
            {
                index = this.FindIndex(string.Empty);
            }

            if (index == -1)
            {
                index = this.BackingList.Count;
            }

            this.BackingList.SetElement(index, key);

            File.WriteAllText(Path.Combine(this.CachePath, $"{index}.cache"), value);
        }

        public bool Remove(object okey)
        {
            int index = this.FindIndex(okey);

            if (index == -1)
            {
                return false;
            }
            else
            {
                this.BackingList.SetElement(index, string.Empty);

                return true;
            }
        }

        public int FindIndex(object okey)
        {
            string key = $"{okey}";

            if (this.EncodeKey)
            {
                key = key.ToBase64();
            }

            return this.BackingList.IndexOf(key);
        }

        public bool TryGet(object okey, out string value)
        {
            int index = this.FindIndex(okey);

            if (index == -1)
            {
                value = null;
                return false;
            }

            string fName = Path.Combine(this.CachePath, $"{index}.cache");

            if (File.Exists(fName))
            {
                value = File.ReadAllText(fName);
                return true;
            }
            else
            {
                value = null;
                return false;
            }
        }
    }
}
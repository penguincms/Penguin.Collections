using Penguin.Extensions.String;
using System;
using System.IO;

namespace Penguin.Collections
{
    public class CacheDirectory : IDisposable
    {
        private ListFile _backingList;

        public string CachePath { get; private set; }

        private ListFile BackingList
        {
            get
            {
                if (_backingList is null)
                {
                    if (!Directory.Exists(CachePath))
                    {
                        _ = Directory.CreateDirectory(CachePath);
                    }

                    _backingList = new ListFile(Path.Combine(CachePath, "Index.cache"));
                }

                return _backingList;
            }
        }

        public bool EncodeKey { get; private set; }

        public CacheDirectory(string path, bool encodeKey = true)
        {
            CachePath = path;
            EncodeKey = encodeKey;
        }

        public void Add(object okey, string value)
        {
            string key = $"{okey}";

            if (EncodeKey)
            {
                key = key.ToBase64();
            }

            int index = FindIndex(key);

            if (index == -1)
            {
                index = FindIndex(string.Empty);
            }

            if (index == -1)
            {
                index = BackingList.Count;
            }

            BackingList.SetElement(index, key);

            File.WriteAllText(Path.Combine(CachePath, $"{index}.cache"), value);
        }

        public bool Remove(object okey)
        {
            int index = FindIndex(okey);

            if (index == -1)
            {
                return false;
            }
            else
            {
                BackingList.SetElement(index, string.Empty);

                return true;
            }
        }

        public int FindIndex(object okey)
        {
            string key = $"{okey}";

            if (EncodeKey)
            {
                key = key.ToBase64();
            }

            return BackingList.IndexOf(key);
        }

        public bool TryGet(object okey, out string value)
        {
            int index = FindIndex(okey);

            if (index == -1)
            {
                value = null;
                return false;
            }

            string fName = Path.Combine(CachePath, $"{index}.cache");

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

        public void Dispose()
        {
            ((IDisposable)_backingList).Dispose();
        }
    }
}
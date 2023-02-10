using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Penguin.Collections
{
    public class ListFile : IList<string>, IDisposable
    {
        public string this[int index]
        {
            get => ((IList<string>)backing)[index];
            set => ((IList<string>)backing)[index] = value;
        }

        private readonly bool AutoFlush = true;

        private readonly List<string> backing = new();

        private readonly string Path;

        private bool disposedValue;

        public int Count => ((ICollection<string>)backing).Count;

        public bool Dirty { get; private set; }

        public bool IsReadOnly => ((ICollection<string>)backing).IsReadOnly;

        public ListFile(string path)
        {
            Path = path;

            if (System.IO.File.Exists(path))
            {
                backing = System.IO.File.ReadAllLines(path).ToList();
            }
        }

        public ListFile(string path, bool autoFlush) : this(path)
        {
            AutoFlush = autoFlush;
        }

        public void Add(string item)
        {
            ((ICollection<string>)backing).Add(item);

            if (AutoFlush)
            {
                System.IO.File.WriteAllLines(Path, backing);
            }
            else
            {
                Dirty = true;
            }
        }

        public void Clear()
        {
            ((ICollection<string>)backing).Clear();

            if (AutoFlush)
            {
                System.IO.File.Delete(Path);
            }
            else
            {
                Dirty = true;
            }
        }

        public bool Contains(string item)
        {
            return ((ICollection<string>)backing).Contains(item);
        }

        public void CopyTo(string[] array, int arrayIndex)
        {
            ((ICollection<string>)backing).CopyTo(array, arrayIndex);
        }

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public void Flush()
        {
            System.IO.File.WriteAllLines(Path, backing);
            Dirty = false;
        }

        public IEnumerator<string> GetEnumerator()
        {
            return ((IEnumerable<string>)backing).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)backing).GetEnumerator();
        }

        public int IndexOf(string item)
        {
            return ((IList<string>)backing).IndexOf(item);
        }

        public void Insert(int index, string item)
        {
            ((IList<string>)backing).Insert(index, item);

            if (AutoFlush)
            {
                System.IO.File.WriteAllLines(Path, backing);
            }
            else
            {
                Dirty = true;
            }
        }

        public bool Remove(string item)
        {
            bool v = ((ICollection<string>)backing).Remove(item);

            if (AutoFlush)
            {
                System.IO.File.WriteAllLines(Path, backing);
            }
            else
            {
                Dirty = true;
            }

            return v;
        }

        public void RemoveAt(int index)
        {
            ((IList<string>)backing).RemoveAt(index);

            if (AutoFlush)
            {
                System.IO.File.WriteAllLines(Path, backing);
            }
            else
            {
                Dirty = true;
            }
        }

        public void SetElement(int index, string value)
        {
            if (backing.Count <= index)
            {
                while (backing.Count < index)
                {
                    Add(string.Empty);

                    if (AutoFlush)
                    {
                        System.IO.File.AppendAllText(Path, System.Environment.NewLine);
                    }
                }

                Add(value);

                if (AutoFlush)
                {
                    System.IO.File.AppendAllText(Path, value);
                }
                else
                {
                    Dirty = true;
                }
            }
            else
            {
                string eVal = backing[index];

                if (eVal == value)
                {
                    return;
                }

                backing[index] = value;

                if (AutoFlush)
                {
                    System.IO.File.WriteAllLines(Path, backing);
                }
                else
                {
                    Dirty = true;
                }
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    if (!AutoFlush && Dirty)
                    {
                        Flush();
                    }
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                disposedValue = true;
            }
        }

        // // TODO: override finalizer only if 'Dispose(bool disposing)' has code to free unmanaged resources
        // ~ListFile()
        // {
        //     // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
        //     Dispose(disposing: false);
        // }
    }
}
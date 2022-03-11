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
            get => ((IList<string>)this.backing)[index];
            set => ((IList<string>)this.backing)[index] = value;
        }

        private readonly bool AutoFlush = true;

        private readonly List<string> backing = new List<string>();

        private readonly string Path;

        private bool disposedValue;
        public int Count => ((ICollection<string>)this.backing).Count;
        public bool Dirty { get; private set; } = false;
        public bool IsReadOnly => ((ICollection<string>)this.backing).IsReadOnly;

        public ListFile(string path)
        {
            this.Path = path;

            if (System.IO.File.Exists(path))
            {
                this.backing = System.IO.File.ReadAllLines(path).ToList();
            }
        }

        public ListFile(string path, bool autoFlush) : this(path)
        {
            this.AutoFlush = autoFlush;
        }

        public void Add(string item)
        {
            ((ICollection<string>)this.backing).Add(item);

            if (this.AutoFlush)
            {
                System.IO.File.WriteAllLines(this.Path, this.backing);
            }
            else
            {
                this.Dirty = true;
            }
        }

        public void Clear()
        {
            ((ICollection<string>)this.backing).Clear();

            if (this.AutoFlush)
            {
                System.IO.File.Delete(this.Path);
            }
            else
            {
                this.Dirty = true;
            }
        }

        public bool Contains(string item) => ((ICollection<string>)this.backing).Contains(item);

        public void CopyTo(string[] array, int arrayIndex) => ((ICollection<string>)this.backing).CopyTo(array, arrayIndex);

        public void Dispose()
        {
            // Do not change this code. Put cleanup code in 'Dispose(bool disposing)' method
            this.Dispose(disposing: true);
            GC.SuppressFinalize(this);
        }

        public void Flush()
        {
            System.IO.File.WriteAllLines(this.Path, this.backing);
            this.Dirty = false;
        }

        public IEnumerator<string> GetEnumerator() => ((IEnumerable<string>)this.backing).GetEnumerator();

        IEnumerator IEnumerable.GetEnumerator() => ((IEnumerable)this.backing).GetEnumerator();

        public int IndexOf(string item) => ((IList<string>)this.backing).IndexOf(item);

        public void Insert(int index, string item)
        {
            ((IList<string>)this.backing).Insert(index, item);

            if (this.AutoFlush)
            {
                System.IO.File.WriteAllLines(this.Path, this.backing);
            }
            else
            {
                this.Dirty = true;
            }
        }

        public bool Remove(string item)
        {
            bool v = ((ICollection<string>)this.backing).Remove(item);

            if (this.AutoFlush)
            {
                System.IO.File.WriteAllLines(this.Path, this.backing);
            }
            else
            {
                this.Dirty = true;
            }

            return v;
        }

        public void RemoveAt(int index)
        {
            ((IList<string>)this.backing).RemoveAt(index);

            if (this.AutoFlush)
            {
                System.IO.File.WriteAllLines(this.Path, this.backing);
            }
            else
            {
                this.Dirty = true;
            }
        }

        public void SetElement(int index, string value)
        {
            if (this.backing.Count <= index)
            {
                while (this.backing.Count < index)
                {
                    this.Add(string.Empty);

                    if (this.AutoFlush)
                    {
                        System.IO.File.AppendAllText(this.Path, System.Environment.NewLine);
                    }
                }

                this.Add(value);

                if (this.AutoFlush)
                {
                    System.IO.File.AppendAllText(this.Path, value);
                }
                else
                {
                    this.Dirty = true;
                }
            }
            else
            {
                string eVal = this.backing[index];

                if (eVal == value)
                {
                    return;
                }

                this.backing[index] = value;

                if (this.AutoFlush)
                {
                    System.IO.File.WriteAllLines(this.Path, this.backing);
                }
                else
                {
                    this.Dirty = true;
                }
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!this.disposedValue)
            {
                if (disposing)
                {
                    if (!this.AutoFlush && this.Dirty)
                    {
                        this.Flush();
                    }
                }

                // TODO: free unmanaged resources (unmanaged objects) and override finalizer
                // TODO: set large fields to null
                this.disposedValue = true;
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
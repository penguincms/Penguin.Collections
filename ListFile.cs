using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Penguin.Collections
{
    public class ListFile : IList<string>
    {
        private readonly List<string> backing = new List<string>();

        private readonly string Path;
        public int Count => ((ICollection<string>)this.backing).Count;

        public bool IsReadOnly => ((ICollection<string>)this.backing).IsReadOnly;

        public ListFile(string path)
        {
            this.Path = path;

            if (System.IO.File.Exists(path))
            {
                this.backing = System.IO.File.ReadAllLines(path).ToList();
            }
        }

        public string this[int index]
        {
            get => ((IList<string>)this.backing)[index];
            set => ((IList<string>)this.backing)[index] = value;
        }

        public void Add(string item)
        {
            ((ICollection<string>)this.backing).Add(item);
            System.IO.File.WriteAllLines(this.Path, this.backing);
        }

        public void SetElement(int index, string value)
        {
            if (this.backing.Count <= index)
            {
                while (this.backing.Count < index)
                {
                    this.Add(string.Empty);
                    System.IO.File.AppendAllText(this.Path, System.Environment.NewLine);
                }

                this.Add(value);
                System.IO.File.AppendAllText(this.Path, value);
            }
            else
            {
                string eVal = this.backing[index];

                if (eVal == value)
                {
                    return;
                }

                this.backing[index] = value;
                System.IO.File.WriteAllLines(this.Path, this.backing);
            }
        }

        public void Clear()
        {
            ((ICollection<string>)this.backing).Clear();
            System.IO.File.Delete(this.Path);
        }

        public bool Contains(string item)
        {
            return ((ICollection<string>)this.backing).Contains(item);
        }

        public void CopyTo(string[] array, int arrayIndex)
        {
            ((ICollection<string>)this.backing).CopyTo(array, arrayIndex);
        }

        public IEnumerator<string> GetEnumerator()
        {
            return ((IEnumerable<string>)this.backing).GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return ((IEnumerable)this.backing).GetEnumerator();
        }

        public int IndexOf(string item)
        {
            return ((IList<string>)this.backing).IndexOf(item);
        }

        public void Insert(int index, string item)
        {
            ((IList<string>)this.backing).Insert(index, item);
            System.IO.File.WriteAllLines(this.Path, this.backing);
        }

        public bool Remove(string item)
        {
            bool v = ((ICollection<string>)this.backing).Remove(item);
            System.IO.File.WriteAllLines(this.Path, this.backing);
            return v;
        }

        public void RemoveAt(int index)
        {
            ((IList<string>)this.backing).RemoveAt(index);
            System.IO.File.WriteAllLines(this.Path, this.backing);
        }
    }
}
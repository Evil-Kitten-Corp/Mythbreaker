// Decompiled with JetBrains decompiler
// Type: Ara.ElasticArray`1
// Assembly: Assembly-CSharp, Version=0.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: F4031B7C-11BF-4CB4-9E59-92795C2AF19E
// Assembly location: D:\Me Gameys\Build.ver4\Combo Asset_BackUpThisFolder_ButDontShipItWithYourGame\Managed\Assembly-CSharp.dll

using System;
using System.Collections;
using System.Collections.Generic;

#nullable disable
namespace Ara
{
  public class ElasticArray<T> : IList<T>, ICollection<T>, IEnumerable<T>, IEnumerable
  {
    private T[] data = new T[16];
    private int count;

    public IEnumerator<T> GetEnumerator()
    {
      for (int i = 0; i < this.count; ++i)
        yield return this.data[i];
    }

    IEnumerator IEnumerable.GetEnumerator() => (IEnumerator) this.GetEnumerator();

    public void Add(T item)
    {
      this.EnsureCapacity(this.count + 1);
      this.data[this.count++] = item;
    }

    public void Clear() => this.count = 0;

    public bool Contains(T item)
    {
      for (int index = 0; index < this.count; ++index)
      {
        if (this.data[index].Equals((object) item))
          return true;
      }
      return false;
    }

    public void CopyTo(T[] array, int arrayIndex)
    {
      if (array == null)
        throw new ArgumentNullException();
      if (array.Length - arrayIndex < this.count)
        throw new ArgumentException();
      Array.Copy((Array) this.data, 0, (Array) array, arrayIndex, this.count);
    }

    public bool Remove(T item)
    {
      bool flag = false;
      for (int index = 0; index < this.count; ++index)
      {
        if (!flag && this.data[index].Equals((object) item))
          flag = true;
        if (flag && index < this.count - 1)
          this.data[index] = this.data[index + 1];
      }
      if (flag)
        --this.count;
      return flag;
    }

    public int Count => this.count;

    public bool IsReadOnly => false;

    public int IndexOf(T item) => Array.IndexOf<T>(this.data, item);

    public void Insert(int index, T item)
    {
      if (index < 0 || index > this.count)
        throw new ArgumentOutOfRangeException();
      this.EnsureCapacity(++this.count);
      for (int index1 = this.count - 1; index1 > index; ++index1)
        this.data[index1] = this.data[index1 - 1];
      this.data[index] = item;
    }

    public void RemoveAt(int index)
    {
      for (int index1 = index; index1 < this.count; ++index1)
      {
        if (index1 < this.count - 1)
          this.data[index1] = this.data[index1 + 1];
      }
      --this.count;
    }

    public void RemoveRange(int index, int num)
    {
      if (index < 0 || num < 0)
        throw new ArgumentOutOfRangeException();
      if (index + num > this.count)
        throw new ArgumentException();
      for (int index1 = index + num - 1; index1 >= index; --index1)
        this.RemoveAt(index1);
    }

    public T this[int index]
    {
      get => this.data[index];
      set => this.data[index] = value;
    }

    public T[] Data => this.data;

    public void SetCount(int count)
    {
      this.EnsureCapacity(count);
      this.count = count;
    }

    public void EnsureCapacity(int capacity)
    {
      if (capacity < this.data.Length)
        return;
      Array.Resize<T>(ref this.data, capacity * 2);
    }

    public void Reverse()
    {
      int index1 = 0;
      T obj;
      for (int index2 = this.count - 1; index1 < index2; this.data[index2--] = obj)
      {
        obj = this.data[index1];
        this.data[index1++] = this.data[index2];
      }
    }
  }
}

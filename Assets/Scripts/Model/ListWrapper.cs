using System;
using System.Collections.Generic;

namespace TestLab.EventChannel.Model
{
    [Serializable]
    public class ListWrapper<T>
    {
        public List<T> Items = new List<T>();
    }
}
using System;
using System.Collections.Generic;

namespace TestLab.EventChannel.Model
{
    public class DataStructure<T> : IEventBasedData
    {
        public event Action OnDataReloaded;
        public event Action OnDataClear;

        private List<T> _data = new List<T>();

        public List<T> Data
        {
            get => _data;
            set
            {
                _data = value;
                OnDataReloaded?.Invoke();
            }
        }

        public void ResetData()
        {
            _data.Clear();
            OnDataClear?.Invoke();
        }
    }
}
using System;

namespace TestLab.EventChannel.Model
{
    public interface IEventBasedData
    {
        public event Action OnDataReloaded;
        public event Action OnDataClear;
    }
}
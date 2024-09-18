using TestLab.EventChannel.Model;
using UnityEngine;
using UnityEngine.Pool;

namespace TestLab.EventChannel.View
{
    public abstract class DataView : MonoBehaviour
    {
        public abstract void Bind(IDataModel data, IObjectPool<GameObject> pool, IEventBasedData model);
    }
}
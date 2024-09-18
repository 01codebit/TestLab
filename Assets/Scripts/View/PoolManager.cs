using System.Collections.Generic;
using Logging;
using TestLab.EventChannel.Model;
using UnityEngine;
using UnityEngine.Pool;

namespace TestLab.EventChannel.View
{
    public class PoolManager : MonoBehaviour
    {
        [SerializeField] private GameObject _itemPrefab;
        [SerializeField] private Transform _anchor;
        
        private ObjectPool<TodoDataView> _pool;

        // Collection checks will throw errors if we try to release an item that is already in the pool.
        public bool collectionChecks = true;
        public int defaultPoolSize = 10;
        public int maxPoolSize = 100;
        
        public void OnEnable()
        {
            _pool = new ObjectPool<TodoDataView>(CreatePooledItem, OnTakeFromPool, OnReturnedToPool, OnDestroyPoolObject, collectionChecks, defaultPoolSize, maxPoolSize);
            ConditionalLogger.Log($"[PoolManager.OnEnable] {_pool.CountAll} ({_pool.CountActive}/{_pool.CountInactive})");
        }
        
        public void SetItemList(List<Todo> list)
        {
            ConditionalLogger.Log($"[PoolManager.SetItemList] {_pool.CountAll} ({_pool.CountActive}/{_pool.CountInactive})");
            _pool.Clear();
            foreach (var data in list)
            {
                var item = _pool.Get();
                item.gameObject.name = data.GetField("userId") + "_" + data.GetField("id"); 
                var dataView = item.GetComponent<DataView>();
                //dataView.Bind(data);
            }
        }
        
        private TodoDataView CreatePooledItem()
        {
            var go = Instantiate(_itemPrefab);//, Vector3.zero, Quaternion.identity);
            go.transform.SetParent(_anchor);
            go.transform.localScale = Vector2.one;

            var dv = go.AddComponent<TodoDataView>();
            
            return dv;
        }
        
        // Called when an item is taken from the pool using Get
        void OnTakeFromPool(TodoDataView dv)
        {
            dv.gameObject.SetActive(true);
        }
        
        // Called when an item is returned to the pool using Release
        void OnReturnedToPool(TodoDataView dv)
        {
            _pool.Release(dv);
            dv.gameObject.SetActive(false);
        }
        
        // If the pool capacity is reached then any items returned will be destroyed.
        // We can control what the destroy behavior does, here we destroy the GameObject.
        void OnDestroyPoolObject(TodoDataView dv)
        {
            ConditionalLogger.Log($"[PoolManager.OnDestroyPoolObject] go name: {dv.gameObject.name}");
            Destroy(dv.gameObject);
        }

        public void Clear()
        {
            _pool.Clear();
        }
    }
}
using UnityEngine;
using UnityEngine.Pool;

namespace ObjectPool
{
    public class GameObjectsFromPrefabPool
    {
        private GameObject _prefab;
        private Transform _anchor;
        private static string _defaultName;
        private int _maxSize;
        
        private ObjectPool<GameObject> _pool;
        
        public GameObjectsFromPrefabPool(GameObject prefab, Transform transform, string defaultName, int maxSize = 20)
        {
            _prefab = prefab;
            _anchor = transform;
            _defaultName = defaultName;

            _pool ??= new ObjectPool<GameObject>(CreateFunc, ActionOnGet, ActionOnRelease,
                ActionOnDestroy, true, 10, maxSize);
        }
        
        public ObjectPool<GameObject> GetPool() => _pool;
        
        private GameObject CreateFunc()
        {
            var label = GameObject.Instantiate(_prefab, _anchor);
            label.name = _defaultName;
            return label;
        }

        private void ActionOnGet(GameObject obj)
        {
            Debug.Log("[GameObjectPool.ActionOnGet]");
            obj.SetActive(true);
        }

        private void ActionOnRelease(GameObject obj)
        {
            Debug.Log("[GameObjectPool.ActionOnRelease]");
            obj.SetActive(false);
            obj.name = _defaultName;
        }

        private void ActionOnDestroy(GameObject obj)
        {
            Debug.Log("[GameObjectPool.ActionOnDestroy]");
            GameObject.Destroy(obj);
        }
    }
}
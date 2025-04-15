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
        
        public ObjectPool<GameObject> Build(GameObject prefab, Transform transform, string defaultName)
        {
            _prefab = prefab;
            _anchor = transform;
            _defaultName = defaultName;

            return _pool ??= new ObjectPool<GameObject>(CreateFunc, ActionOnGet, ActionOnRelease,
                ActionOnDestroy, true, 10, 20);
        }

        public GameObjectsFromPrefabPool WithPrefab(GameObject prefab)
        {
            _prefab = prefab;
            return this;
        }

        public GameObjectsFromPrefabPool WithTransform(Transform transform)
        {
            _anchor = transform;
            return this;
        }

        public GameObjectsFromPrefabPool WithDefaultName(string defaultName)
        {
            _defaultName = defaultName;
            return this;
        }

        public GameObjectsFromPrefabPool Build(int maxSize = 20)
        {
            _maxSize = maxSize;
            _pool ??= new ObjectPool<GameObject>(CreateFunc, ActionOnGet, ActionOnRelease,
                ActionOnDestroy, true, 10, _maxSize);

            return this;
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
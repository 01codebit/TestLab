using UnityEngine;
using UnityEngine.Pool;

namespace ObjectPool
{
    public class GameObjectsFromPrefabPool
    {
        private GameObject _prefab;
        private Transform _anchor;
        private static string _defaultName;

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

        public GameObjectsFromPrefabPool Build()
        {
            _pool ??= new ObjectPool<GameObject>(CreateFunc, ActionOnGet, ActionOnRelease,
                ActionOnDestroy, true, 10, 20);

            return this;
        }

        public ObjectPool<GameObject> GetPool() => _pool;
        
        private GameObject CreateFunc()
        {
            var label = GameObject.Instantiate(_prefab, _anchor);
            label.name = _defaultName;
            return label;
        }

        private static void ActionOnGet(GameObject obj)
        {
            obj.SetActive(true);
        }

        private static void ActionOnRelease(GameObject obj)
        {
            obj.SetActive(false);
            obj.name = _defaultName;
        }

        private static void ActionOnDestroy(GameObject obj)
        {
            GameObject.Destroy(obj);
        }
    }
}
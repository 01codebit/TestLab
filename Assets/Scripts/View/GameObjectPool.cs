using UnityEngine;
using UnityEngine.Pool;

namespace TestLab.EventChannel.View
{
    // Note that this class is not a MonoBehavior. Just a pure C# class.
    public class GameObjectPool
    {
        GameObject prefab;
        ObjectPool<GameObject> pool;
        int defaultSize;
        int maxSize;

        // Our class's constructor. Takes the prefab to spawn as an argument.
        public GameObjectPool(GameObject prefab, int defaultSize = 10, int maxSize = 100)
        {
            this.prefab = prefab; // The prefab to spawn.
            this.defaultSize = defaultSize; // Pool's starting number of objects.
            this.maxSize = maxSize; // Max size for our pool.
            
            // Initializing our pool.
            pool = new ObjectPool<GameObject>(
                CreatePooledObject,
                OnGetFromPool,
                OnReturnToPool,
                OnDestroyPooledObject,
                true,
                defaultSize,
                maxSize
            );
        }

        public ObjectPool<GameObject> GetPool() => pool;
        
        // Wrapper function for pool.Get. Gets object and sets position.
        // Also resets rigidbody's velocity if it has one.
        public GameObject GetObject(Vector3 position)
        {
            GameObject obj = pool.Get();
            obj.transform.position = position;
            if (obj.TryGetComponent<Rigidbody>(out Rigidbody rb))
            {
                rb.linearVelocity = Vector3.zero;
                rb.angularVelocity = Vector3.zero;
            }

            return obj;
        }

        public GameObject GetAnchoredObject(Transform anchor)
        {
            GameObject obj = pool.Get();
            obj.transform.SetParent(anchor);

            return obj;
        }

        // Wrapper function for pool.Release
        public void ReleaseObject(GameObject obj)
        {
            pool.Release(obj);
        }

        // Return a brand new GameObject instance for our pool to use.
        // We have to specify GameObject.Instantiate because
        // this isn't a Monobehavior.
        private GameObject CreatePooledObject()
        {
            GameObject newObject = GameObject.Instantiate(prefab);
            // ConditionalLogger.Log("[GameObjectPool.CreatePooledObject] GameObject instantiated");
            return newObject;
        }

        // When an object is taken from the pool, activate it.
        private void OnGetFromPool(GameObject pooledObject)
        {
            pooledObject.SetActive(true);
            // ConditionalLogger.Log("[GameObjectPool.OnGetFromPool] GameObject reactivated");
        }

        // When an object is returned to the pool, deactivate it.
        private void OnReturnToPool(GameObject pooledObject)
        {
            pooledObject.SetActive(false);
            // ConditionalLogger.Log("[GameObjectPool.OnReturnToPool] GameObject activated");
        }

        // When the pool discards an object, destroy the GameObject.
        private void OnDestroyPooledObject(GameObject pooledObject)
        {
            GameObject.DestroyImmediate(pooledObject);
            // ConditionalLogger.Log("[GameObjectPool.OnDestroyPooledObject] GameObject destroyed");
        }

        public void Clear()
        {
            pool.Clear();
        }

        public string Stats()
        {
            return $"[GameObjectPool.Stats] pool counts: active: {pool.CountActive} - inactive: {pool.CountInactive} - total: {pool.CountAll}";
        }
    }
}
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Pool;

namespace BeastLinkBattle.Core
{
    public class PoolManager : MonoBehaviour
    {
        public static PoolManager Instance { get; private set; }

        private Dictionary<GameObject, IObjectPool<GameObject>> _pools = new Dictionary<GameObject, IObjectPool<GameObject>>();

        private Dictionary<GameObject, IObjectPool<GameObject>> _spawnedObjects = new Dictionary<GameObject, IObjectPool<GameObject>>();

        private void Awake()
        {
            if (Instance == null) Instance = this;
            else Destroy(gameObject);
        }

        public GameObject Get(GameObject prefab, Vector3 position, Quaternion rotation, Transform parent = null)
        {
            if (prefab == null) return null;

            if (!_pools.ContainsKey(prefab))
            {
                _pools[prefab] = new ObjectPool<GameObject>(
                    createFunc: () => Instantiate(prefab, parent),
                    actionOnGet: obj =>
                    {
                        obj.transform.position = position;
                        obj.transform.rotation = rotation;
                        if (parent != null) obj.transform.SetParent(parent);
                        obj.SetActive(true);
                    },
                    actionOnRelease: obj => obj.SetActive(false),
                    actionOnDestroy: obj => Destroy(obj),
                    collectionCheck: false,
                    defaultCapacity: 20,
                    maxSize: 100
                );
            }

            GameObject instance = _pools[prefab].Get();
            instance.transform.position = position;
            instance.transform.rotation = rotation;
            if (parent != null) instance.transform.SetParent(parent);

            _spawnedObjects[instance] = _pools[prefab];

            return instance;
        }

        public void Release(GameObject instance)
        {
            if (instance == null || !instance.activeSelf) return;

            if (_spawnedObjects.TryGetValue(instance, out var pool))
            {
                pool.Release(instance);
                _spawnedObjects.Remove(instance);
            }
            else
            {
                Destroy(instance);
            }
        }
    }
}
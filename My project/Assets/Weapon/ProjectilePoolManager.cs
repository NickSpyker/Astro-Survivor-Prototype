using System.Collections.Generic;
using UnityEngine;

public class ProjectilePoolManager : MonoBehaviour
{
    public static ProjectilePoolManager Instance { get; private set; }

    [System.Serializable]
    public class ProjectilePool
    {
        public string poolName;
        public GameObject projectilePrefab;
        public int poolSize = 150;
        public bool expandable = true;
    }

    [SerializeField] private List<ProjectilePool> pools = new();
    private Dictionary<string, Queue<GameObject>> poolDictionary;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        InitializePools();
    }

    private void InitializePools()
    {
        poolDictionary = new Dictionary<string, Queue<GameObject>>();

        foreach (var pool in pools)
        {
            Queue<GameObject> objectPool = new Queue<GameObject>();

            for (int i = 0; i < pool.poolSize; i++)
            {
                GameObject obj = Instantiate(pool.projectilePrefab);
                obj.SetActive(false);
                obj.transform.SetParent(transform);
                objectPool.Enqueue(obj);
            }

            poolDictionary.Add(pool.poolName, objectPool);
        }
    }

    public GameObject SpawnProjectile(string poolName, Vector3 position, Quaternion rotation)
    {
        if (!poolDictionary.ContainsKey(poolName))
        {
            Debug.LogWarning($"Pool {poolName} doesn't exist!");
            return null;
        }

        GameObject objectToSpawn;

        if (poolDictionary[poolName].Count > 0)
        {
            objectToSpawn = poolDictionary[poolName].Dequeue();
        }
        else
        {
            // Check if pool is expandable
            var pool = pools.Find(p => p.poolName == poolName);
            if (pool != null && pool.expandable)
            {
                objectToSpawn = Instantiate(pool.projectilePrefab);
                objectToSpawn.transform.SetParent(transform);
            }
            else
            {
                Debug.LogWarning($"Pool {poolName} is empty and not expandable!");
                return null;
            }
        }

        objectToSpawn.SetActive(true);
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;

        return objectToSpawn;
    }

    public void ReturnToPool(string poolName, GameObject obj)
    {
        obj.SetActive(false);
        
        if (poolDictionary.ContainsKey(poolName))
        {
            poolDictionary[poolName].Enqueue(obj);
        }
        else
        {
            Destroy(obj);
        }
    }
}

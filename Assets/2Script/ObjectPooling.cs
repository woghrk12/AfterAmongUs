using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct Pool
{
    public string tag;
    public GameObject prefab;
    public int size;
}

public class ObjectPooling : MonoBehaviour
{
    public static ObjectPooling inst;

    [SerializeField] private Pool[] pools;
    private Dictionary<string, Queue<GameObject>> poolDictionary;
    private Dictionary<string, GameObject> categories;

    private void Awake()
    {
        inst = this;

        poolDictionary = new Dictionary<string, Queue<GameObject>>();
        categories = new Dictionary<string, GameObject>();
    }

    private void Start()
    {
        foreach (Pool pool in pools)
        {
            var categroy = new GameObject(pool.tag);
            categroy.transform.SetParent(transform);
            categories.Add(pool.tag, categroy);

            poolDictionary.Add(pool.tag, new Queue<GameObject>());

            for (int i = 0; i < pool.size; i++)
            {
                var obj = CreateNewObject(pool.tag, pool.prefab);
            }
        }
    }

    public static GameObject SpawnObject(string tag, Vector3 position, Quaternion rotation)
        => inst.SpawnFromPool(tag, position, rotation);
    public static void ReturnObject(GameObject obj)
        => inst.ReturnToPool(obj);

    private GameObject SpawnFromPool(string tag, Vector3 position, Quaternion rotation)
    {
        Queue<GameObject> poolQueue = poolDictionary[tag];

        if (poolQueue.Count <= 0)
        {
            var pool = Array.Find(pools, x => x.tag == tag);
            var obj = CreateNewObject(pool.tag, pool.prefab);
        }

        var objectToSpawn = poolQueue.Dequeue();
        objectToSpawn.transform.position = position;
        objectToSpawn.transform.rotation = rotation;
        objectToSpawn.SetActive(true);

        return objectToSpawn;
    }

    private void ReturnToPool(GameObject obj)
    {
        obj.SetActive(false);
        poolDictionary[obj.name].Enqueue(obj);
    }

    private GameObject CreateNewObject(string tag, GameObject prefab)
    {
        var obj = Instantiate(prefab, transform);
        obj.name = tag;
        obj.transform.SetParent(categories[tag].transform);
        poolDictionary[tag].Enqueue(obj);
        obj.SetActive(false);
        return obj;
    }
}

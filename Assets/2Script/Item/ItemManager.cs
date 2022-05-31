using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager instance;

    [SerializeField] private Transform[] spawnPositions;
    private Vector3[] positions;
    private int length;

    private Queue<GameObject> items;

    private void Awake()
    {
        instance = this;

        length = spawnPositions.Length;
        positions = new Vector3[length];
        items = new Queue<GameObject>();

        for (int i = 0; i < length; i++)
            positions[i] = spawnPositions[i].position;
    }

    public static void SpawnItems(int num)
        => instance.SpawnFromPool(num);
    public static void ReturnItems()
        => instance.ReturnToPool();


    private void SpawnFromPool(int num)
    {
        for (int i = 0; i < num; i++)
        {
            if (length <= 0) return;

            var position = Random.Range(0, length);
            var obj = ObjectPooling.SpawnObject("Item Box", positions[position], Quaternion.identity);
            items.Enqueue(obj);
            obj.GetComponent<ItemBox>().miniMapObject = MiniMapManager.SpawnObject(obj.transform.position);
            positions[position] = positions[length - 1];
            length--;
        }
    }

    public void AddPosition(Vector3 position)
    {
        if (length >= spawnPositions.Length) return;

        length++;
        positions[length - 1] = position;
    }

    private void ReturnToPool()
    {
        while (items.Count > 0)
        {
            var obj = items.Dequeue();
            MiniMapManager.ReturnObject(obj.GetComponent<ItemBox>().miniMapObject);
            ObjectPooling.ReturnObject(obj);
        }
    }
}

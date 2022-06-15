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

    public static void SpawnItems(int p_num)
        => instance.SpawnFromPool(p_num);
    public static void ReturnItems()
        => instance.ReturnToPool();

    private void SpawnFromPool(int p_num)
    {
        for (int i = 0; i < p_num; i++)
        {
            if (length <= 0) return;

            var t_position = Random.Range(0, length);
            var t_obj = ObjectPooling.SpawnObject("Item Box", positions[t_position], Quaternion.identity);
            items.Enqueue(t_obj);
            t_obj.GetComponent<ItemBox>().miniMapObject = MiniMapManager.SpawnObject(t_obj.transform.position);
            positions[t_position] = positions[length - 1];
            length--;
        }
    }

    public void AddPosition(Vector3 p_position)
    {
        if (length >= spawnPositions.Length) return;

        length++;
        positions[length - 1] = p_position;
    }

    private void ReturnToPool()
    {
        while (items.Count > 0)
        {
            var t_obj = items.Dequeue();
            MiniMapManager.ReturnObject(t_obj.GetComponent<ItemBox>().miniMapObject);
            ObjectPooling.ReturnObject(t_obj);
        }
    }
}

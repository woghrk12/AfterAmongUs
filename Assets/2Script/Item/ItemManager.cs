using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager instance;

    [SerializeField] private Transform[] spawnPositions;
    private Vector3[] positions;
    private int length;

    private void Awake()
    {
        instance = this;

        length = spawnPositions.Length;

        positions = new Vector3[length];

        for (int i = 0; i < length; i++)
            positions[i] = spawnPositions[i].position;
    }

    public void SpawnItems()
    {
        if (length <= 0) return;

        var position = Random.Range(0, length);
        ObjectPooling.SpawnObject("Item Box", positions[position], Quaternion.identity);
        positions[position] = positions[length - 1];
        length--;
    }

    public void SpawnItems(int num)
    {
        for (int i = 0; i < num; i++)
        {
            if (length <= 0) return;

            var position = Random.Range(0, length);
            ObjectPooling.SpawnObject("Item Box", positions[position], Quaternion.identity);
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
}

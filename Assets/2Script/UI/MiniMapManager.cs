using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MiniMapManager : MonoBehaviour
{
    public static MiniMapManager instance;

    [SerializeField] private float minX, maxX;
    [SerializeField] private float minY, maxY;
    private float width, height;

    [SerializeField] private Image background;
    private float miniMapWidth, miniMapHeight;

    private float rateX, rateY;

    [SerializeField] private int numQueue;
    private Queue<GameObject> objectQueue;
    [SerializeField] private GameObject prefab;

    private void Awake()
    {
        instance = this;    

        width = maxX - minX;
        height = maxY - minY;
        miniMapWidth = background.transform.localScale.x * background.rectTransform.sizeDelta.x;
        miniMapHeight = background.transform.localScale.y * background.rectTransform.sizeDelta.y;

        rateX = miniMapWidth / width;
        rateY = miniMapHeight / height;

        objectQueue = new Queue<GameObject>();

        for (int i = 0; i < numQueue; i++)
        {
            var obj = Instantiate(prefab, transform);
            objectQueue.Enqueue(obj);
            obj.SetActive(false);
        }
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public MiniMapObject SpawnObject(Vector2 position)
    {
        var obj = objectQueue.Dequeue().GetComponent<MiniMapObject>();
        obj.gameObject.SetActive(true);
        obj.SetPosition(CalculatePosition(position));

        return obj;
    }

    public void ReturnObject(MiniMapObject obj)
    {
        objectQueue.Enqueue(obj.gameObject);
        obj.gameObject.SetActive(false);
    }

    private Vector2 CalculatePosition(Vector2 position)
    {
        var newPosition = position;

        newPosition.x *= rateX;
        newPosition.y *= rateY;

        return newPosition;
    }
}

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

    [SerializeField] private Sprite itemBoxSprite;
    [SerializeField] private Sprite enemyPortalSprite;

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
            var t_obj = Instantiate(prefab, transform);
            objectQueue.Enqueue(t_obj);
            t_obj.SetActive(false);
        }
    }

    private void Start()
    {
        gameObject.SetActive(false);
    }

    public static MiniMapObject SpawnObject(Vector2 p_position, EMiniMapObject p_objType)
        => instance.SpawnFromPool(p_position, p_objType);
    public static void ReturnObject(MiniMapObject p_obj)
        => instance.ReturnToPool(p_obj);

    public MiniMapObject SpawnFromPool(Vector2 p_position, EMiniMapObject p_objType)
    {
        var t_obj = objectQueue.Dequeue().GetComponent<MiniMapObject>();
        t_obj.gameObject.SetActive(true);

        switch (p_objType)
        {
            case EMiniMapObject.ITEMBOX:
                t_obj.SetObject(CalculatePosition(p_position), itemBoxSprite);
                break;
            case EMiniMapObject.ENEMYPORTAL:
                t_obj.SetObject(CalculatePosition(p_position), enemyPortalSprite);
                break;
            default:
                break;
        }
        

        return t_obj;
    }

    private void ReturnToPool(MiniMapObject p_obj)
    {
        objectQueue.Enqueue(p_obj.gameObject);
        p_obj.gameObject.SetActive(false);
    }

    private Vector2 CalculatePosition(Vector2 p_position)
    {
        var t_position = p_position;

        t_position.x *= rateX;
        t_position.y *= rateY;

        return t_position;
    }
}

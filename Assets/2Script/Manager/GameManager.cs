using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager instance = null;
    public static GameManager Instance
    {
        get
        {
            if (instance == null)
            {
                var t_obj = FindObjectOfType<GameManager>();

                if (t_obj != null)
                {
                    instance = t_obj;
                }
                else
                {
                    var t_newObj = new GameObject().AddComponent<GameManager>();
                    instance = t_newObj;
                }
            }
            return instance;
        }
    }

    public static EPlayerColor playerColor = EPlayerColor.RED;
    public static EPlayerWeapon[] playerWeapon = new EPlayerWeapon[2];
    public static bool isStart = false;

    private void Awake()
    {
        var t_objs = FindObjectsOfType<GameManager>();

        if (t_objs.Length != 1)
        {
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);

        playerColor = EPlayerColor.RED;
        for (int i = 0; i < playerWeapon.Length; i++)
            playerWeapon[i] = EPlayerWeapon.NONE;
    }
}

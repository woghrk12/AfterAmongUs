using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Chasing : MonoBehaviour
{
    [SerializeField] private Region curRegion = null;
    [SerializeField] private InGameManager inGameManager = null;

    [SerializeField] private float speed = 0f;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.P))
        {
            StartCoroutine(Chase());
        }
    }

    private IEnumerator Chase()
    {
        var t_regionList = inGameManager.FindRegion(curRegion);
        var t_curRegion = t_regionList[0];
        var t_curPos = new Vector2Int((int)(transform.position.x * 10f), (int)(transform.position.y * 10f));

        while (t_regionList.Count > 0)
        {
            var t_targetPos = t_regionList.Count > 1 ? t_curRegion.GetAdjPoint(t_regionList[1]) : t_curRegion.TargetPos;

            var t_nodeList = t_curRegion.FindPath(t_curPos, t_targetPos);

            while (t_nodeList.Count > 0)
            {
                Move(t_nodeList[0].Position);
                yield return null;
                if ((transform.position - t_nodeList[0].Position).sqrMagnitude <= 0.001f) t_nodeList.RemoveAt(0);
            }
            yield return null;

            t_curPos = t_targetPos;

            t_regionList.RemoveAt(0);
            if (t_regionList.Count > 0) t_curRegion = t_regionList[0];
        }
    }

    private void Move(Vector3 p_pos)
    {
        var t_moveDir = (p_pos - transform.position).normalized;

        transform.position += t_moveDir * Time.deltaTime * speed;
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public SpriteRenderer sprite;
    private Animator anim;

    private Coroutine runningCo = null;

    [SerializeField] private GamePlayer player;

    public Region region;

    private Point startPoint, targetPoint, curPoint;
    private List<Point> openList, closedList, finalList;

    private bool isChasing;
    private bool canAttack;
    private bool isDie;

    [SerializeField] private float moveSpeed;

    [SerializeField] private int health;
    [SerializeField] private int maxHealth;
    [SerializeField] private ControlSlider healthBar;

    [SerializeField] private Transform firePosition;
    [SerializeField] private float fireDelay;
    private Vector3 fireDirection;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();

        var t_inst = Instantiate(sprite.material);
        sprite.material = t_inst;

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<GamePlayer>();

        sprite.material.SetColor("_PlayerColor", Color.red);
    }

    private void OnEnable()
    {
        health = maxHealth;
        healthBar.SetMaxValue(maxHealth);
        sprite.enabled = false;

        canAttack = true;
        isChasing = false;
        isDie = false;
    }

    private void Update()
    {
        if (health <= 0 && !isDie) StartCoroutine(Die());

        if ((transform.position - player.transform.position).sqrMagnitude > 2f) return;

        if (canAttack)
        {
            runningCo = StartCoroutine(AttackCo());
        }
    }

    private void FixedUpdate()
    {
        if (isChasing)
            Chase();
    }

    public void SetEnemy(Region p_region)
    {
        region = p_region;
        transform.position = PointManager.GetPoint(p_region.dstPoint).transform.position;
        FindPath(player.playerRegion);
        isChasing = true;
    }

    private void Chase()
    {
        if (region == player.playerRegion)
        {
            Move(player.transform);
        }
        else
        {
            if (finalList.Count == 0) return;

            Move(finalList[0].transform);

            if ((transform.position - finalList[0].transform.position).sqrMagnitude <= 0.01f)
                finalList.RemoveAt(0);
        }
    }

    private void Move(Transform p_target)
    {
        var t_moveDir = (p_target.position - transform.position).normalized;

        var t_flipX = sprite.flipX;
        sprite.flipX = (t_moveDir.x != 0) ? (t_moveDir.x < 0) : t_flipX;

        anim.SetBool("isChasing", isChasing);

        transform.position += t_moveDir * Time.deltaTime * moveSpeed;
    }

    private void FindPath(Region p_target)
    {
        startPoint = region.FindStartPoint(transform.position);
        targetPoint = PointManager.GetPoint(p_target.dstPoint);

        openList = new List<Point>() { startPoint };
        closedList = new List<Point>();
        finalList = new List<Point>();

        while (openList.Count > 0)
        {
            // Find the point with smallest cost 
            curPoint = openList[0];
            for (int i = 1; i < openList.Count; i++)
                if (openList[i].F <= curPoint.F && openList[i].H < curPoint.H)
                    curPoint = openList[i];

            openList.Remove(curPoint);
            closedList.Add(curPoint);

            // reach the final destination
            if (curPoint == targetPoint)
            {
                var t_curPoint = targetPoint;

                while (t_curPoint != startPoint)
                {
                    finalList.Add(t_curPoint);
                    t_curPoint = t_curPoint.parentPoint;
                }

                finalList.Add(t_curPoint);
                finalList.Reverse();
            }

            // add adjacent points 
            var t_adjList = curPoint.adj_Point;
            var t_adjWeight = curPoint.adj_Weight;
            for (int i = 0; i < t_adjList.Count; i++)
                OpenListAdd(t_adjList[i], t_adjWeight[i]);
        }
    }

    private void OpenListAdd(Point p_point, float p_weight)
    {
        if (closedList.Contains(p_point)) return;

        if (openList.Contains(p_point) && curPoint.G + p_weight > p_point.G) return;

        p_point.SetParent(curPoint);
        p_point.SetG(p_weight);
        p_point.SetH(p_point.transform.position, targetPoint.transform.position);

        openList.Add(p_point);
    }

    private Vector3 Targeting()
    {
        var t_playerPosition = player.transform.position;
        var t_direction = (t_playerPosition - firePosition.position).normalized;

        float t_angle = Mathf.Atan2(t_direction.y, t_direction.x) * Mathf.Rad2Deg;

        var t_rotation = Quaternion.AngleAxis(t_angle, Vector3.forward);

        firePosition.rotation = t_rotation;

        return t_direction;
    }

    private IEnumerator AttackCo()
    {
        canAttack = false;
        isChasing = false;

        anim.SetBool("isChasing", isChasing);
        anim.SetTrigger("Attack");

        yield return new WaitForSeconds(0.1f);

        fireDirection = Targeting();
        var t_bullet = ObjectPooling.SpawnObject("Enemy Bullet", firePosition.position, firePosition.rotation).GetComponent<Bullet>();
        t_bullet.SetDirection(fireDirection);

        yield return new WaitForSeconds(fireDelay);

        canAttack = true;
        isChasing = true;
    }

    public void OnDamage(int p_damage)
    {
        health -= p_damage;
        
        if (!healthBar.gameObject.activeSelf) healthBar.gameObject.SetActive(true);

        healthBar.SetValue(health);

        if (health > 0)
        {
            StartCoroutine(OnDamageCo());
        }
    }

    private IEnumerator OnDamageCo()
    {
        sprite.color = Color.red;

        yield return new WaitForSeconds(0.05f);

        sprite.color = Color.white;
    }

    private IEnumerator Die()
    {
        canAttack = false;
        isChasing = false;
        isDie = true;
        gameObject.layer = 10;

        if(runningCo != null)
            StopCoroutine(runningCo);
        
        anim.SetTrigger("Die");

        yield return new WaitForSeconds(3f);

        gameObject.layer = 9;
        ObjectPooling.ReturnObject(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Region"))
        {
            region = collision.GetComponent<Region>();
        }

        if (collision.CompareTag("Camera Collider"))
        {
            sprite.enabled = true;
        }

        if (collision.CompareTag("Bullet"))
        {
            var t_damage = collision.GetComponent<Bullet>().damage;
            OnDamage(t_damage);
            ObjectPooling.ReturnObject(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Camera Collider"))
        {
            sprite.enabled = false;
        }
    }
}

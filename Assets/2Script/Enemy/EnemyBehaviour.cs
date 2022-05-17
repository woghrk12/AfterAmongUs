using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{
    public SpriteRenderer sprite;
    private Animator anim;

    private Coroutine runningCo = null;

    [SerializeField] private GameObject capsuleCollider;

    [SerializeField] private PointManager pointManager;

    [SerializeField] private PlayerBehavior player;

    public Region region;

    private Point startPoint, targetPoint, curPoint;
    private List<Point> openList, closedList, finalList;

    private bool isChasing;
    private bool canAttack;

    [SerializeField] private float moveSpeed;

    [SerializeField] private int health;
    [SerializeField] private int maxHealth;
    [SerializeField] private ControlSlider healthBar;

    [SerializeField] private GameObject bullet;
    [SerializeField] private Transform firePosition;
    [SerializeField] private float fireDelay;

    private void Awake()
    {
        anim = GetComponent<Animator>();
        sprite = GetComponent<SpriteRenderer>();

        var inst = Instantiate(sprite.material);
        sprite.material = inst;

        player = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerBehavior>();

        moveSpeed = 1f;

        canAttack = true;
        isChasing = false;
    }

    private void Start()
    {
        pointManager = GameManager.Instance.pointManager;

        sprite.material.SetColor("_PlayerColor", Color.red);

        health = maxHealth;
        healthBar.SetMaxValue(maxHealth);

        sprite.enabled = false;
        gameObject.SetActive(false);
    }

    private void Update()
    {
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

    public void SetEnemy(Region _region)
    {
        gameObject.SetActive(true);
        region = _region;
        transform.position = pointManager.GetPoint(_region.dstPoint).transform.position;
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

    private void Move(Transform _target)
    {
        var moveDir = (_target.position - transform.position).normalized;

        var curFlipX = sprite.flipX;
        sprite.flipX = (moveDir.x != 0) ? (moveDir.x < 0) : curFlipX;

        anim.SetBool("isChasing", isChasing);

        transform.position += moveDir * Time.deltaTime * moveSpeed;
    }

    private void FindPath(Region _target)
    {
        startPoint = region.FindStartPoint(transform.position);
        targetPoint = pointManager.GetPoint(_target.dstPoint);

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
                var targetCurPoint = targetPoint;

                while (targetCurPoint != startPoint)
                {
                    finalList.Add(targetCurPoint);
                    targetCurPoint = targetCurPoint.parentPoint;
                }

                finalList.Add(targetCurPoint);
                finalList.Reverse();
            }

            // add adjacent points 
            var adjList = curPoint.adj_Point;
            var adjWeight = curPoint.adj_Weight;
            for (int i = 0; i < adjList.Count; i++)
                OpenListAdd(adjList[i], adjWeight[i]);
        }
    }

    private void OpenListAdd(Point _point, float _weight)
    {
        if (closedList.Contains(_point)) return;

        if (openList.Contains(_point) && curPoint.G + _weight > _point.G) return;

        _point.SetParent(curPoint);
        _point.SetG(_weight);
        _point.SetH(_point.transform.position, targetPoint.transform.position);

        openList.Add(_point);
    }

    private void Targeting()
    {
        var playerPosition = player.transform.position;
        var direction = playerPosition - firePosition.position;

        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;

        var rotation = Quaternion.AngleAxis(angle, Vector3.forward);

        firePosition.rotation = rotation;
    }

    private IEnumerator AttackCo()
    {
        canAttack = false;
        isChasing = false;

        anim.SetBool("isChasing", isChasing);
        anim.SetTrigger("Attack");

        yield return new WaitForSeconds(0.1f);

        Targeting();
        Instantiate(bullet, firePosition.position, firePosition.rotation);

        yield return new WaitForSeconds(fireDelay);

        canAttack = true;
        isChasing = true;
    }

    public void OnDamage(int _damage)
    {
        health -= _damage;
        
        if (!healthBar.gameObject.activeSelf) healthBar.gameObject.SetActive(true);

        healthBar.SetValue(health);

        if (health > 0)
        {
            StartCoroutine(OnDamageCo());
        }
        else
        {
            Die();
        }
    }

    private IEnumerator OnDamageCo()
    {
        sprite.color = Color.red;

        yield return new WaitForSeconds(0.05f);

        sprite.color = Color.white;
    }

    private void Die()
    {
        canAttack = false;
        isChasing = false;
        gameObject.layer = 9;
        capsuleCollider.layer = 9;

        if(runningCo != null)
            StopCoroutine(runningCo);
        
        anim.SetTrigger("Die");

        GameManager.Instance.EnemyDie();
        Destroy(gameObject, 3f);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Region")
        {
            region = collision.GetComponent<Region>();
        }

        if (collision.tag == "Camera Collider")
        {
            sprite.enabled = true;
        }

        if (collision.tag == "Bullet")
        {
            var damage = collision.GetComponent<Bullet>().damage;
            OnDamage(damage);
            Destroy(collision.gameObject);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Camera Collider")
        {
            sprite.enabled = false;
        }
    }
}

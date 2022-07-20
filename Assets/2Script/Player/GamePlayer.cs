using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GamePlayer : PlayerBehaviour
{
    private Animator anim = null;

    protected CharacterTargeting targetingController;
    protected CharacterDamagable healthController;

    [SerializeField] private List<PlayerWeapon> weapons;
    private bool[] hasWeapons;
    private PlayerWeapon equipWeapon;
    private EBulletType bulletType;
    private int equipWeaponIndex;

    private bool sDown1;
    private bool sDown2;
    private bool sDown3;
    private bool mDown;

    private bool isFireReady;
    private bool isReloadReady;

    private float fireDelay;
    [SerializeField] private PlayerRader playerRader;

    private Coroutine reloadCo;

    private int ammo12Guage;
    private int ammo7MM;
    private int ammo5MM;
    private int ammo9MM;

    [SerializeField] private Text curAmmoText;
    [SerializeField] private Text totalAmmoText;
    [SerializeField] private Image ammoImage;

    [SerializeField] private Sprite ammo12GuageImage;
    [SerializeField] private Sprite ammo7MMImage;
    [SerializeField] private Sprite ammo5MMImage;
    [SerializeField] private Sprite ammo9MMImage;

    [SerializeField] private GameObject miniMap;

    [SerializeField] private GameObject itemAcqView;
    [SerializeField] private GameObject itemTextPrefab;

    protected override void Awake()
    {
        base.Awake();

        anim = GetComponent<Animator>();

        targetingController = GetComponent<CharacterTargeting>();
        healthController = GetComponent<CharacterDamagable>();

        hasWeapons = new bool[weapons.Count];

        for (int i = 0; i < weapons.Count; i++)
            hasWeapons[i] = true;

        equipWeaponIndex = 0;

        isFireReady = true;
        isReloadReady = true;

        fireDelay = 0f;

        ammo12Guage = 50;
        ammo7MM = 50;
        ammo5MM = 50;
        ammo9MM = 50;

        healthController.onDamageEvent += OnDamageEvent;
        healthController.onDieEvent += OnDieEvent;
    }

    protected override void Start()
    {
        base.Start();

        equipWeapon = weapons[equipWeaponIndex];
        equipWeapon.gameObject.SetActive(true);
        bulletType = equipWeapon.bulletType;

        playerRader.SetRange(equipWeapon.range);

        ammoImage.sprite = ammo9MMImage;
        totalAmmoText.text = ammo9MM.ToString();
        curAmmoText.text = equipWeapon.curAmmo.ToString();

        healthController.SetHealth(true);
    }

    private void Update()
    {
        if (healthController.IsDie) return;

        GetInput();
        Fire();
        Swap();
        TurnOnOffMiniMap();
    }

    private void GetInput()
    {
        sDown1 = Input.GetButtonDown("Swap1");
        sDown2 = Input.GetButtonDown("Swap2");
        sDown3 = Input.GetButtonDown("Swap3");
        mDown = Input.GetButtonDown("Map");
    }

    private void Fire()
    {
        if (equipWeapon == null) return;

        fireDelay += Time.deltaTime;
        isFireReady = equipWeapon.rate < fireDelay;
        
        if (!isReloadReady) return;

        if (equipWeapon.curAmmo <= 0)
        {
            Reload();
            return;
        }
        
        if (!isFireReady) return;

        targetingController.Target = playerRader.Target;
        if (targetingController.Target == null)
        {
            targetingController.IsTargeting = false;
            return;
        }

        targetingController.IsTargeting = true;

        if (moveController.IsMove) return;

        equipWeapon.GetComponent<PlayerWeapon>().Shot();
        curAmmoText.text = equipWeapon.curAmmo.ToString();
        fireDelay = 0;
    }

    private void Reload()
    {
        if (equipWeapon == null) return;
        if (!isReloadReady) return;

        reloadCo = StartCoroutine(ReloadCo(equipWeapon.bulletType, equipWeapon.reloadTime));
    }

    private IEnumerator ReloadCo(EBulletType p_bulletType, float p_reloadTime)
    {
        isReloadReady = false;

        yield return new WaitForSeconds(p_reloadTime);

        switch (p_bulletType)
        {
            case EBulletType.FIVEMM:
                if (ammo5MM <= 0) break;
                ammo5MM -= equipWeapon.Reload(ammo5MM);
                totalAmmoText.text = ammo5MM.ToString();
                break;

            case EBulletType.SEVENMM:
                if (ammo7MM <= 0) break;
                ammo7MM -= equipWeapon.Reload(ammo7MM);
                totalAmmoText.text = ammo7MM.ToString();
                break;

            case EBulletType.NINEMM:
                if (ammo9MM <= 0) break;
                ammo9MM -= equipWeapon.Reload(ammo9MM);
                totalAmmoText.text = ammo9MM.ToString();
                break;

            case EBulletType.TWELVEGAUGE:
                if (ammo12Guage <= 0) break;
                ammo12Guage -= equipWeapon.Reload(ammo12Guage);
                totalAmmoText.text = ammo12Guage.ToString();
                break;
        }
        curAmmoText.text = equipWeapon.curAmmo.ToString();

        reloadCo = null;
        isReloadReady = true;
    }

    private void Swap()
    {
        if (!sDown1 && !sDown2 && !sDown3) return;
        if (sDown1 && (equipWeaponIndex == 0 || !hasWeapons[0])) return;
        if (sDown2 && (equipWeaponIndex == 1 || !hasWeapons[1])) return;
        if (sDown3 && (equipWeaponIndex == 2 || !hasWeapons[2])) return;

        if (reloadCo != null)
        {
            StopCoroutine(reloadCo);
            isReloadReady = true;
        }
        if (equipWeapon != null) equipWeapon.gameObject.SetActive(false);

        if (sDown1) equipWeaponIndex = 0;
        if (sDown2) equipWeaponIndex = 1;
        if (sDown3) equipWeaponIndex = 2;

        equipWeapon = weapons[equipWeaponIndex];
        equipWeapon.gameObject.SetActive(true);

        bulletType = equipWeapon.bulletType;

        playerRader.SetRange(equipWeapon.range);

        switch (bulletType)
        {
            case EBulletType.FIVEMM:
                ammoImage.sprite = ammo5MMImage;
                totalAmmoText.text = ammo5MM.ToString();
                break;

            case EBulletType.SEVENMM:
                ammoImage.sprite = ammo7MMImage;
                totalAmmoText.text = ammo7MM.ToString();
                break;

            case EBulletType.NINEMM:
                ammoImage.sprite = ammo9MMImage;
                totalAmmoText.text = ammo9MM.ToString();
                break;

            case EBulletType.TWELVEGAUGE:
                ammoImage.sprite = ammo12GuageImage;
                totalAmmoText.text = ammo12Guage.ToString();
                break;
        }

        curAmmoText.text = equipWeapon.curAmmo.ToString();
    }
    
    private void GetItem(EItemType p_itemType, int p_num)
    {
        var t_itemTextPrefab = Instantiate(itemTextPrefab, itemAcqView.transform).GetComponent<ItemText>();
        t_itemTextPrefab.SetText(p_itemType, p_num);

        switch (p_itemType)
        {
            case EItemType.AMMO12:
                ammo12Guage += p_num;
                if (equipWeapon.bulletType == EBulletType.TWELVEGAUGE)
                    totalAmmoText.text = ammo12Guage.ToString();
                break;

            case EItemType.AMMO7:
                ammo7MM += p_num;
                if (equipWeapon.bulletType == EBulletType.SEVENMM)
                    totalAmmoText.text = ammo7MM.ToString();
                break;

            case EItemType.AMMO5:
                ammo5MM += p_num;
                if (equipWeapon.bulletType == EBulletType.FIVEMM)
                    totalAmmoText.text = ammo5MM.ToString();
                break;

            case EItemType.AMMO9:
                ammo9MM += p_num;
                if (equipWeapon.bulletType == EBulletType.NINEMM)
                    totalAmmoText.text = ammo9MM.ToString();
                break;

            case EItemType.HEAL:
                //curHealth += p_num;
                //healthBar.SetValue(curHealth);
                break;

            case EItemType.GRENADE:
                break;

            case EItemType.WEAPON:
                if (p_num == (int)EWeaponType.PISTOL)
                {

                }
                else if (p_num == (int)EWeaponType.RIFLE)
                {

                }
                else if (p_num == (int)EWeaponType.SHOTGUN)
                { 
                
                }
                break;

            default:
                break;
        }
    }

    private void OnDamageEvent()
    {
        StartCoroutine(OnDamageCo());
    }

    private IEnumerator OnDamageCo()
    {
        gameObject.layer = 7;
        int t_countTime = 0;

        while (t_countTime < 10)
        {
            if (t_countTime % 2 == 0)
                colorController.SetAlpha(0.3f);
            else
                colorController.SetAlpha(0.6f);

            yield return new WaitForSeconds(0.2f);

            t_countTime++;
        }

        colorController.SetAlpha(1f);

        gameObject.layer = 6;
    }

    private void OnDieEvent()
    {
        if (equipWeapon != null)
        {
            equipWeapon.gameObject.SetActive(false);
        }

        gameObject.layer = 7;
        CanMove = false;

        anim.SetTrigger("Die");
    }

    private void TurnOnOffMiniMap()
    {
        if (!mDown) return;

        miniMap.SetActive(!miniMap.activeSelf);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("EnemyBullet"))
        {
            var damage = collision.GetComponent<Bullet>().damage;
            healthController.OnDamage(damage);
            ObjectPooling.ReturnObject(collision.gameObject);
        }

        if (collision.CompareTag("Item"))
        {
            EItemType t_itemType; int t_num;
            collision.GetComponent<Item>().GetItemValue(out t_itemType, out t_num);
            GetItem(t_itemType, t_num);
            ObjectPooling.ReturnObject(collision.gameObject);
        }
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZObjectPools;
using Cysharp.Threading.Tasks;

public class Player : MonoBehaviour
{
    public Material skyMaterial;
    public BoxCollider2D boxColliderAttractor;
    [HideInInspector]
    public Ship ship;
    public bool isGodMode = false;
    bool isActive = false;
    float timeLastShoot = -2f;
    EZObjectPool bulletPool;
    bool touchingLaserEnemy = false;
    Camera cam;
    Rigidbody2D rb;
    Vector2 limitPos;
    int level = 1;
    int energy = 0, coins = 0;

    public void Spawn(Ship ship)
    {
        transform.position = Vector2.zero;
        isActive = true;
        this.ship = ship;
        bulletPool = EZObjectPool.CreateObjectPool(ship.bullet, "player bullets", 150, true, true, true);
        limitPos = GameManager.boundaries;
        boxColliderAttractor.size = Helpers.SizeAttractorClosed();
        transform.Find("CoinAttractor").SetParent(Camera.main.transform);
        gameObject.SetActive(true);
        Helpers.PlaySFX(Helpers.tables.startGame);
    }

    void Awake()
    {
        cam = Camera.main;
        rb = GetComponent<Rigidbody2D>();
        skyMaterial.SetTextureOffset("_FrontTex", Vector2.zero);
    }

    void FixedUpdate()
    {
        Move(new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")));
        LookToMouse();
    }

    void Update()
    {
        if (isActive) Controls();
    }

    void Controls()
    {
        if (Input.GetMouseButton(0)) Shoot();
        if (Input.GetMouseButtonDown(0)) boxColliderAttractor.size = Helpers.SizeAttractorClosed();
        if (Input.GetMouseButtonUp(0)) boxColliderAttractor.size = Helpers.SizeAttractorOpened(ship);
    }

    void Move(Vector2 direction)
    {
        rb.AddForce(direction.normalized * (ship.stats.movility * 0.25f), ForceMode2D.Impulse);
        //rb.position += direction * ship.stats.movility * Time.deltaTime;
        rb.position = new Vector2(Mathf.Clamp(rb.position.x, -limitPos.x, limitPos.x), Mathf.Clamp(rb.position.y, -limitPos.y, limitPos.y));
        skyMaterial.SetTextureOffset("_FrontTex", rb.position / 100);
    }

    void Shoot()
    {
        if (!IsShootAvailable()) return;
        timeLastShoot = Time.time;
        if (bulletPool.TryGetNextObject(GetNextShootPosition(), transform.rotation, out GameObject go))
        {
            go.GetComponent<Rigidbody2D>().velocity = transform.right * ship.stats.bulletSpeed * 5;
            Helpers.PlaySFX(Helpers.tables.shot1, 0.1f);
        }
    }

    Vector3 GetNextShootPosition()
    {
        var cannonsT = transform.Find("Cannons");
        var count = cannonsT.childCount;
        return transform.GetChild(Random.Range(0, count)).position;
    }

    bool IsShootAvailable()
    {
        return timeLastShoot + BulletCooldown() < Time.time;
    }

    float BulletCooldown()
    {
        return 0.2f - ship.stats.attackSpeed * 0.02f;
    }

    void LookToMouse()
    {
        var dir = Input.mousePosition - (Vector2)cam.WorldToScreenPoint(transform.position);
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        rb.MoveRotation(angle);
    }

    void LevelUp()
    {
        AddStats(new Stats { attackSpeed = 1, bulletDamage = 1, bulletSpeed = 1, health = 1, movility = 1 });
        energy = 0;
        level++;
        Helpers.PlaySFX(Helpers.tables.startGame);
    }

    void AddStats(Stats stats)
    {
        ship.stats = new Stats
        {
            attackSpeed = ship.stats.attackSpeed + stats.attackSpeed,
            bulletDamage = ship.stats.bulletDamage + stats.bulletDamage,
            bulletSpeed = ship.stats.bulletSpeed + stats.bulletSpeed,
            health = ship.stats.health + stats.health,
            movility = ship.stats.movility + stats.movility
        };
    }

    void GrabPowerUp(GameObject powerUpGO)
    {
        powerUpGO.SetActive(false);
        Helpers.PlaySFX(Helpers.tables.startGame);
        var powerUpName = powerUpGO.name.ToUpper();
        print(powerUpName);
        if (powerUpName.Contains("ATTACK SPEED"))
        {
            AddStats(new Stats { attackSpeed = 5 });
        }
    }

    void GrabCoin(GameObject coinGO)
    {
        var amount = int.Parse(coinGO.gameObject.name.Split(' ')[1]);
        coins += amount;
        coinGO.SetActive(false);
        Helpers.PlaySFX(Helpers.tables.coinGrab);
    }

    void GrabEnergy(GameObject energyGO)
    {
        var amount = int.Parse(energyGO.gameObject.name.Split(' ')[1]);
        print(amount);
        energy += amount;
        energyGO.SetActive(false);
        Helpers.PlaySFX(Helpers.tables.energyGrab);
        if (100 <= energy) LevelUp();
    }

    void GetDamage(int damage)
    {
        ship.stats.health -= damage;
        if (ship.stats.health <= 0) Die();
    }

    void Die()
    {
        if (isGodMode) return;
        GameManager.Instance.GameOver();
        isActive = false;
        gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Energy")) GrabEnergy(other.gameObject);
        else if (other.CompareTag("Enemy")) Die();
        else if (other.CompareTag("PowerUp")) GrabPowerUp(other.gameObject);
    }

}

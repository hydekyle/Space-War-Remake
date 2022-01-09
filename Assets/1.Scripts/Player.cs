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
    bool isActive = false;
    float timeLastShoot = -2f;
    EZObjectPool bulletPool;
    int level = 1;
    bool touchingLaserEnemy = false;
    Camera cam;
    Rigidbody2D rb;
    Vector2 limitPos;

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
        rb.AddForce(direction.normalized * ship.stats.movility, ForceMode2D.Impulse);
        //rb.position += direction * ship.stats.movility * Time.deltaTime;
        rb.position = new Vector2(Mathf.Clamp(rb.position.x, -limitPos.x, limitPos.x), Mathf.Clamp(rb.position.y, -limitPos.y, limitPos.y));
        skyMaterial.SetTextureOffset("_FrontTex", rb.position / 100);
    }

    void Shoot()
    {
        if (!IsShootAvailable()) return;
        timeLastShoot = Time.time;
        if (bulletPool.TryGetNextObject(GetNextCannonPos(), transform.rotation, out GameObject go))
        {
            go.GetComponent<Rigidbody2D>().velocity = transform.right * ship.stats.bulletSpeed;
            Helpers.PlaySFX(Helpers.tables.shot1, 0.1f);
        }
    }

    Vector3 GetNextCannonPos()
    {
        var cannonsT = transform.Find("Cannons");
        var count = cannonsT.childCount;
        return transform.GetChild(Random.Range(0, count)).position;
    }

    bool IsShootAvailable()
    {
        return timeLastShoot + ship.stats.bulletCooldown < Time.time;
    }

    void LookToMouse()
    {
        var dir = Input.mousePosition - (Vector2)cam.WorldToScreenPoint(transform.position);
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        rb.MoveRotation(angle);
    }

    void GrabCoin(GameObject coin)
    {
        coin.SetActive(false);
        Helpers.PlaySFX(Helpers.tables.coinGrab);
    }

    void GrabEnergy(GameObject energy)
    {
        energy.SetActive(false);
        Helpers.PlaySFX(Helpers.tables.energyGrab);
    }

    void GetDamage(int damage)
    {
        ship.stats.health -= damage;
        if (ship.stats.health <= 0) Die();
        print("Me muero " + ship.stats.health);
    }

    void Die()
    {
        GameManager.Instance.GameOver();
        isActive = false;
        gameObject.SetActive(false);
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Energy")) GrabCoin(other.gameObject);
        else if (other.CompareTag("Enemy")) Die();
    }

}

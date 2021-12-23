using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EZObjectPools;

public class Player : MonoBehaviour
{
    Camera cam;
    Rigidbody2D rb;
    Ship ship;
    bool isActive = false;
    float timeLastShoot = -2f;
    EZObjectPool bulletPool;
    int level = 1;

    public void Spawn(Ship ship)
    {
        transform.position = Vector2.zero;
        isActive = true;
        this.ship = ship;
        bulletPool = EZObjectPool.CreateObjectPool(ship.bullet, "player bullets", 150, true, true, true);
        gameObject.SetActive(true);
    }

    void Awake()
    {
        cam = Camera.main;
        rb = GetComponent<Rigidbody2D>();
    }

    void Update()
    {
        if (isActive) Controls();
    }

    void LateUpdate()
    {
        LookToMouse();
    }

    void Controls()
    {
        if (Input.GetMouseButton(0)) Shoot();
        rb.position += new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * ship.stats.speedMovement * Time.deltaTime;
        rb.position = new Vector2(Mathf.Clamp(rb.position.x, -12, 12), Mathf.Clamp(rb.position.y, -7, 7));
    }

    void Shoot()
    {
        if (!IsShootAvailable()) return;
        timeLastShoot = Time.time;
        if (bulletPool.TryGetNextObject(GetNextCannonPos(), transform.rotation, out GameObject go))
        {
            go.GetComponent<Rigidbody2D>().velocity = transform.right * ship.stats.speedShoot;
            print(ship.stats.speedShoot);
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
        return timeLastShoot + ship.stats.cooldownShoot < Time.time;
    }

    private void LookToMouse()
    {
        var dir = Input.mousePosition - cam.WorldToScreenPoint(transform.position);
        var angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.AngleAxis(angle, transform.forward), Time.deltaTime * ship.stats.speedMovement);
    }
}

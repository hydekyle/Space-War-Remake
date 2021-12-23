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

    void Controls()
    {
        ApuntarRaton();
        if (Input.GetMouseButton(0)) Shoot();
        rb.velocity = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical")) * ship.stats.speedMovement;
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

    void ApuntarRaton()
    {
        float camDis = cam.transform.position.y - transform.position.y;
        Vector3 mouse = cam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, camDis));
        float AngleRad = Mathf.Atan2(mouse.y - transform.position.y, mouse.x - transform.position.x);
        float angle = (180 / Mathf.PI) * AngleRad;
        rb.rotation = angle;
    }
}

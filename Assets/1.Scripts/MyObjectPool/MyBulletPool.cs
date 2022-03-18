using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cysharp.Threading.Tasks;

public class MyBulletPool : MonoBehaviour
{
    // He creado este ObjectPool para poolear la estructura Bullet, en la que se cachea
    // el Rigidbody2D para evitar hacer GetComponents sobre el GameObject

    public static MyBulletPool Instance;
    public List<Bullet> availableBullets = new List<Bullet>();
    public bool resizable;
    GameObject _prefab;
    Transform parentPool;

    void Awake()
    {
        if (Instance) Destroy(this);
        Instance = this;
        parentPool = new GameObject("Bullets Pool").transform;
    }

    public void CreateBulletPool(GameObject prefab, int amount)
    {
        _prefab = prefab; // Reference for resizable pools
        for (var x = 0; x < amount; x++)
            GenerateAvailableBullet(prefab);
    }

    Bullet GenerateAvailableBullet(GameObject prefab)
    {
        // Crear objeto y dejarlo desactivado
        var go = Instantiate(prefab);
        go.SetActive(false);
        go.transform.SetParent(parentPool);
        // Crear y asignar la struct Bullet
        var bullet = new Bullet()
        {
            myRB = go.GetComponent<Rigidbody2D>(),
            myGO = go
        };
        // Añadir script para detectar cuando la bala se desactive
        var pooledBullet = go.AddComponent<MyPooledBullet>();
        pooledBullet.bulletRef = bullet;
        pooledBullet.parentPool = this;
        availableBullets.Add(bullet);
        return bullet;
    }

    async void SpawnBullet(Bullet bullet, Vector3 bulletVelocity)
    {
        await UniTask.NextFrame();
        bullet.myGO.SetActive(true);
        bullet.myRB.velocity = bulletVelocity;
    }

    public bool TryGetNextBullet(Vector3 pos, Quaternion rot, Vector3 bulletVelocity, out Nullable<Bullet> bullet)
    {
        if (availableBullets.Count > 0)
        {
            var nextBullet = availableBullets[0];
            nextBullet.myGO.transform.position = pos;
            nextBullet.myGO.transform.rotation = rot;
            SpawnBullet(nextBullet, bulletVelocity);
            bullet = nextBullet;
            availableBullets.Remove(nextBullet);
            return true;
        }
        else
        {
            if (resizable)
            {
                bullet = GenerateAvailableBullet(_prefab);
                return true;
            }
            bullet = null;
            return false;
        }
    }

    public void AddToAvailableBullets(Bullet bullet)
    {
        availableBullets.Add(bullet);
    }

    public void ClearPool()
    {
        availableBullets.Clear();
        foreach (Transform child in parentPool) Destroy(child);
    }

}
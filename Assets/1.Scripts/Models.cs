using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cysharp.Threading.Tasks;
using UnityObservables;

[System.Serializable]
public class HealthObs : Observable<int> { };

[System.Serializable]
public class EnergyObs : Observable<int> { };

[Serializable]
public struct Ship
{
    public string name;
    public Sprite sprite;
    public GameObject bullet;
    public Stats stats, statsPerLevel;
}

[Serializable]
public struct Stats
{
    public int health, bulletDamage, attackSpeed, bulletSpeed, movility;
}

public class Enemy : MonoBehaviour
{
    [Range(1, 10)]
    public int frequency;
    public String enemyName;
    public SpriteRenderer spriteRenderer;
    public Stats stats;
    public bool isActive = true;
    Color myColor;

    private void Awake()
    {
        try
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
            myColor = spriteRenderer.color;
        }
        catch { }
    }

    public void LateUpdate()
    {
        try
        {
            spriteRenderer.color = Color.Lerp(spriteRenderer.color, myColor, Time.deltaTime * 10);
        }
        catch { }
    }

    public void ReceiveDamage(int damage)
    {
        stats.health -= damage;
        if (stats.health <= 0) Die();
        else spriteRenderer.color = Color.red;
    }

    public void Sleep()
    {
        isActive = false;
    }

    public void Die()
    {
        gameObject.SetActive(false);
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            ReceiveDamage(GameManager.Instance.player.ship.stats.bulletDamage);
            other.gameObject.SetActive(false);
        }
    }
}
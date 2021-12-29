using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using Cysharp.Threading.Tasks;

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
    public int health, bulletDamage;
    public float movility, bulletSpeed, bulletCooldown;
}

public class Enemy : MonoBehaviour
{
    [Range(1, 10)]
    public int frequency;
    public String enemyName;
    public SpriteRenderer spriteRenderer;
    public Stats stats;
    Color myColor;

    private void Awake()
    {
        myColor = spriteRenderer.color;
    }

    public void LateUpdate()
    {
        spriteRenderer.color = Color.Lerp(spriteRenderer.color, myColor, Time.deltaTime * 10);
    }

    public void ReceiveDamage(int damage)
    {
        stats.health -= damage;
        if (stats.health <= 0) Die();
        else spriteRenderer.color = Color.red;
    }

    void Die()
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
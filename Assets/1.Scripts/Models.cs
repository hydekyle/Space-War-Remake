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
    public float speedMovement, speedShoot, cooldownShoot;
}

public class Enemy : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public Stats stats;

    public void LateUpdate()
    {
        spriteRenderer.color = Color.Lerp(spriteRenderer.color, Color.white, Time.deltaTime * 10);
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

    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Bullet"))
        {
            ReceiveDamage(GameManager.Instance.player.ship.stats.bulletDamage);
            other.gameObject.SetActive(false);
        }
    }
}
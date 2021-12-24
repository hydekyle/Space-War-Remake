using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncerEnemy : Enemy
{
    float xMaxPos, yMaxPos;
    Vector3 direction;
    float lastTimeBounced = -1f;

    void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        xMaxPos = GameManager.Instance.xMaxPos;
        yMaxPos = GameManager.Instance.yMaxPos;
        direction = new Vector3(Random.Range(-10, 10), Random.Range(-10, 10), 0).normalized;
    }

    void Update()
    {
        Movement();
        BounceCheck();
    }

    void Movement()
    {
        transform.position += direction * Time.deltaTime * stats.speedMovement;
        transform.Rotate(Vector3.forward * stats.speedMovement * direction.x);
    }

    void BounceCheck()
    {
        if (lastTimeBounced + 0.1f < Time.time)
        {
            if (Mathf.Abs(transform.position.x) > xMaxPos) ReflectDirectionX();
            if (Mathf.Abs(transform.position.y) > yMaxPos) ReflectDirectionY();
        }
    }

    void ReflectDirectionX()
    {
        direction = Vector3.Reflect(direction, Vector3.right);
        lastTimeBounced = Time.time;
    }

    void ReflectDirectionY()
    {
        direction = Vector3.Reflect(direction, Vector3.up);
        lastTimeBounced = Time.time;
    }
}

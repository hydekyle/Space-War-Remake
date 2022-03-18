using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyPooledBullet : MonoBehaviour
{
    // Este Script es añadido sobre los GameObjects por MyBulletPool en runtime

    public MyBulletPool parentPool;
    public Bullet bulletRef;

    void OnDisable()
    {
        transform.position = Vector3.zero;
        parentPool.AddToAvailableBullets(this.bulletRef);
    }
}

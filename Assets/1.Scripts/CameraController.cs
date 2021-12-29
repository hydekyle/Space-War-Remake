using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public Transform target;
    public float velocity;
    Vector3 currentVelocity;

    void Update()
    {
        var targetPos = new Vector3(target.position.x, target.position.y, -10);
        if (Application.isMobilePlatform)
        {
            transform.position = Vector3.SmoothDamp(transform.position, targetPos, ref currentVelocity, velocity);
        }
        else
        {
            //TO-DO: Look to mouse make player's rotation clunchy while camera is moving so we remove SmoothDamp for desktop/webGL ATM
            transform.position = targetPos;
        }
    }
}

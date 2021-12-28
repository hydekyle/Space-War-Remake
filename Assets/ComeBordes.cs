using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ComeBordes : Enemy
{
    List<Vector3> targetPoints = new List<Vector3>();
    Vector2 limitPos;
    float rotationSpeed;
    int indexPosition;
    public bool isReversed;

    void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        isReversed = Random.Range(0, 2) == 1 ? true : false;
        limitPos = GameManager.boundaries;
        rotationSpeed = isReversed ? stats.speedMovement * 20 : stats.speedMovement * -20;
        targetPoints.Add(new Vector2(limitPos.x, limitPos.y));
        targetPoints.Add(new Vector2(-limitPos.x, limitPos.y));
        targetPoints.Add(new Vector2(-limitPos.x, -limitPos.y));
        targetPoints.Add(new Vector2(limitPos.x, -limitPos.y));
        indexPosition = Random.Range(0, targetPoints.Count);
        if (isReversed) targetPoints.Reverse();
    }

    void Update()
    {
        Move();
        Rotate();
    }

    void Move()
    {
        var target = targetPoints[indexPosition];
        transform.position = Vector3.MoveTowards(transform.position, target, Time.deltaTime * stats.speedMovement);
        if (transform.position == target) NextTarget();
    }

    void Rotate()
    {
        transform.Rotate(Vector3.forward * Time.deltaTime * rotationSpeed);
    }

    void NextTarget()
    {
        if (indexPosition == targetPoints.Count - 1) indexPosition = 0;
        else indexPosition++;
    }

}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public struct Ship
{
    public string name;
    public Sprite sprite;
    public GameObject bullet;
    public Statistics stats, statsPerLevel;
}

[Serializable]
public struct Statistics
{
    public float speedMovement, speedShoot, cooldownShoot;
}
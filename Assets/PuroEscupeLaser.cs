using System;
using System.Collections;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using UnityEngine;

public class PuroEscupeLaser : Enemy
{
    Transform cannon;
    GameObject[] lasers;

    private void Start()
    {
        Initialize();
    }

    void Initialize()
    {
        var parentLasers = transform.Find("Lasers");
        var amountLasers = parentLasers.childCount;
        lasers = new GameObject[amountLasers];
        for (var x = 0; x < amountLasers; x++)
        {
            lasers[x] = parentLasers.GetChild(x).gameObject;
        }
        isActive = true;
        LasersRoutine();
    }

    async void LasersRoutine()
    {
        var index = 0;
        do
        {
            LaserActivation(index++);
            await UniTask.Delay(TimeSpan.FromSeconds(5));
            if (index == lasers.Length) index = 0;
        } while (isActive);
    }

    async void LaserActivation(int laserIndex)
    {
        lasers[laserIndex].SetActive(false); // Easy restart animation
        await UniTask.NextFrame();
        lasers[laserIndex].SetActive(true);
    }

}

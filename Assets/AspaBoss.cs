using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AspaBoss : MonoBehaviour
{
    public Transform aspa1, aspa2;
    public float rotationSpeed = 3f;
    void Update()
    {
        aspa1.Rotate(Vector3.forward * rotationSpeed * Time.deltaTime);
        aspa2.Rotate(Vector3.forward * -rotationSpeed * Time.deltaTime);
    }
}

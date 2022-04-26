using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rotator : MonoBehaviour
{
    public float rotationSpeed;
    private float rotation;

    void Update()
    {
        rotation += rotationSpeed * Time.deltaTime;
        transform.eulerAngles = new Vector3(0, 0, rotation);
    }
}

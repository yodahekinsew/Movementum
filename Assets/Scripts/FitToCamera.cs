using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FitToCamera : MonoBehaviour
{
    void Start()
    {
        float halfHeight = Camera.main.orthographicSize;
        float halfWidth = Camera.main.aspect * halfHeight;
        transform.localScale = new Vector3(
            2 * halfWidth,
            2 * halfHeight,
            1
        );
    }
}

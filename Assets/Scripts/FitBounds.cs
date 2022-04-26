using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FitBounds : MonoBehaviour
{
    public Transform topBound;
    public Transform bottomBound;
    public Transform leftBound;
    public Transform rightBound;
    void Start()
    {
        float halfHeight = Camera.main.orthographicSize;
        float halfWidth = Camera.main.aspect * halfHeight;

        topBound.position = new Vector3(
            0,
            halfHeight + .5f,
            0
        );
        bottomBound.position = new Vector3(
            0,
            -halfHeight - .5f,
            0
        );
        rightBound.position = new Vector3(
            halfWidth + .5f,
            0,
            0
        );
        leftBound.position = new Vector3(
            -halfWidth - .5f,
            0,
            0
        );
        
    }
}

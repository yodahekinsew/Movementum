using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FitRectToCamera : MonoBehaviour
{
    public RectTransform rect;

    // Start is called before the first frame update
    void Start()
    {
        float halfHeight = Camera.main.orthographicSize;
        float halfWidth = Camera.main.aspect * halfHeight;
        rect.sizeDelta = new Vector2(
            1.5f * halfWidth,
            2.0f * halfHeight
        );
    }
}

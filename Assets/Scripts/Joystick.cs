using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Joystick : MonoBehaviour
{
    public Transform cursor;

    public SpriteRenderer cursorSprite;
    public SpriteRenderer baseSprite;

    public float magnitudeRange;
    private Vector3 joystickDir;
    private Vector3 touchDown;
    private bool touching;

    private bool activated = false;

    private Camera mainCamera;

    void Start()
    {
        mainCamera = Camera.main;
        float halfHeight = mainCamera.orthographicSize;
        transform.position = new Vector3(
            0,
            -.75f * halfHeight,
            0
        );
    }

    void Update()
    {
        if (!activated) return;

        if (Input.GetMouseButtonDown(0))
        {
            touchDown = Input.mousePosition;
            Vector3 mouseWorldPos = mainCamera.ScreenToWorldPoint(Input.mousePosition);
            transform.position = new Vector3(
                mouseWorldPos.x,
                mouseWorldPos.y,
                transform.position.z
            );
            touching = true;
            ShowJoystick();
        }
        if (Input.GetMouseButtonUp(0))
        {
            joystickDir = Vector3.zero;
            touching = false;
            HideJoystick();
        }

        if (touching)
        {
            joystickDir = Input.mousePosition - touchDown;
            float distance = joystickDir.magnitude;
            float maxDistance = Mathf.Sqrt(Mathf.Pow(.2f * Screen.width, 2) + Mathf.Pow(.2f * Screen.height, 2));
            float scaledMagnitude = Mathf.Min(distance / maxDistance, 1) * magnitudeRange;
            joystickDir = joystickDir.normalized * scaledMagnitude;
        }

        cursor.localPosition = Vector3.Lerp(cursor.localPosition, joystickDir, 5 * Time.deltaTime);
    }

    private void ShowJoystick()
    {
        var cursorColor = cursorSprite.color;
        cursorColor.a = 1;
        cursorSprite.color = cursorColor;

        var baseColor = baseSprite.color;
        baseColor.a = .25f;
        baseSprite.color = baseColor;
    }

    private void HideJoystick()
    {
        joystickDir = Vector3.zero;
        touching = false;

        var cursorColor = cursorSprite.color;
        cursorColor.a = 0;
        cursorSprite.color = cursorColor;

        var baseColor = baseSprite.color;
        baseColor.a = 0;
        baseSprite.color = baseColor;
    }

    public Vector3 GetDir()
    {
        return joystickDir;
    }

    public void Activate()
    {
        activated = true;
        cursorSprite.enabled = true;
        baseSprite.enabled = true;
    }

    public void Deactivate()
    {
        activated = false;
        HideJoystick();
        cursorSprite.enabled = false;
        baseSprite.enabled = false;
    }
}

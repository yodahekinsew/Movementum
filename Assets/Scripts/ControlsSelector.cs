using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ControlsSelector : MonoBehaviour
{
    [Header("Button")]
    public Image joystickButton;
    public Image tiltButton;
    public Image calibrateButton;
    public Image dragButton;

    [Header("Text")]
    public TextMeshProUGUI joystickText;
    public TextMeshProUGUI tiltText;
    public TextMeshProUGUI calibrateText;
    public TextMeshProUGUI dragText;

    private Controls currentScheme;

    private void Start()
    {
        StartCoroutine(SynchronizeFontSize());
    }

    IEnumerator SynchronizeFontSize()
    {
        yield return new WaitForSeconds(.25f);

        float minFontSize = joystickText.fontSize;
        if (tiltText.fontSize < minFontSize) minFontSize = tiltText.fontSize;
        // if (calibrateText.fontSize < minFontSize) calibrateText.fontSize = minFontSize;
        if (dragText.fontSize < minFontSize) minFontSize = dragText.fontSize;

        joystickText.enableAutoSizing = false;
        tiltText.enableAutoSizing = false;
        // calibrateText.enableAutoSizing = false;
        dragText.enableAutoSizing = false;

        joystickText.fontSize = minFontSize;
        tiltText.fontSize = minFontSize;
        // calibrateText.fontSize = minFontSize;
        dragText.fontSize = minFontSize;
    }

    void Update()
    {
        if (Player.controlScheme != currentScheme)
        {
            currentScheme = Player.controlScheme;
            Color c;
            switch (currentScheme)
            {
                case Controls.Drag:
                    c = dragButton.color;
                    c.a = 1;
                    dragButton.color = c;

                    c.a = .75f;
                    tiltButton.color = c;
                    calibrateButton.color = c;
                    joystickButton.color = c;
                    break;
                case Controls.Tilt:
                    c = tiltButton.color;
                    c.a = 1;
                    tiltButton.color = c;
                    calibrateButton.color = c;

                    c.a = .75f;
                    joystickButton.color = c;
                    dragButton.color = c;
                    break;
                case Controls.Joystick:
                    c = joystickButton.color;
                    c.a = 1;
                    joystickButton.color = c;

                    c.a = .75f;
                    tiltButton.color = c;
                    calibrateButton.color = c;
                    dragButton.color = c;
                    break;
            }
        }
    }
}

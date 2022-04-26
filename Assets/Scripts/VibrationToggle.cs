using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class VibrationToggle : MonoBehaviour
{
    [Header("Button")]
    public Image onButton;
    public Image offButton;

    [Header("Text")]
    public TextMeshProUGUI onText;
    public TextMeshProUGUI offText;

    private void Start()
    {
        if (!PlayerPrefs.HasKey("VibrationDisabled")) PlayerPrefs.SetInt("VibrationDisabled", 0);
        else
        {
            if (PlayerPrefs.GetInt("VibrationDisabled") == 0) EnableVibration();
            else DisableVibration();
        }
        StartCoroutine(SynchronizeFontSize());
    }
    IEnumerator SynchronizeFontSize()
    {
        yield return new WaitForSeconds(.25f);

        float minFontSize = onText.fontSize;
        if (offText.fontSize < minFontSize) minFontSize = offText.fontSize;

        onText.enableAutoSizing = false;
        offText.enableAutoSizing = false;

        onText.fontSize = minFontSize;
        offText.fontSize = minFontSize;
    }

    public void EnableVibration()
    {
        Vibration.disabled = false;
        PlayerPrefs.SetInt("VibrationDisabled", 0);

        Color buttonColor = new Color(1, 1, 1, 1);
        onButton.color = buttonColor;

        buttonColor.a = .75f;
        offButton.color = buttonColor;
    }

    public void DisableVibration()
    {
        Vibration.disabled = true;
        PlayerPrefs.SetInt("VibrationDisabled", 1);

        Color buttonColor = new Color(1, 1, 1, 1);
        offButton.color = buttonColor;

        buttonColor.a = .75f;
        onButton.color = buttonColor;
    }
}

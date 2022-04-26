using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class StreakCounter : MonoBehaviour
{
    public Transform streakSlider;
    public TextMeshPro streakNumber;
    public Animator anim;
    private float streakFill = 0; // Range from 0 to 1
    private int streakAmount = 1;
    private bool activated = false;

    public int GetStreak()
    {
        return streakAmount;
    }

    public void FillStreak(float fillAmount)
    {
        streakFill += fillAmount;

        if (fillAmount < 0 && streakFill < 0) streakFill = 0;
        if (fillAmount > 0 && streakFill > 1) streakFill = 1;

        streakSlider.localPosition = new Vector3(-1.5f * (1 - streakFill), 0, 0);
        streakSlider.localScale = new Vector3(streakFill * 3, 1, 1);

        if (fillAmount > 0 && streakFill >= 1)
        {
            streakFill = .25f;
            streakAmount++;
            streakNumber.text = "" + streakAmount;
        }

        if (fillAmount < 0 && streakFill <= 0)
        {
            streakFill = 0;
            streakAmount = 1;
            streakNumber.text = "" + streakAmount;
        }
    }

    public void Show()
    {
        activated = true;
        anim.SetTrigger("ShowCounter");
    }

    public void Hide()
    {
        activated = false;
        anim.SetTrigger("HideCounter");

        streakFill = 0;
        streakAmount = 1;
    }

    public void PlayAgain()
    {
        activated = true;
        anim.SetTrigger("PlayAgain");
    }
}

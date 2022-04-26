using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class Instructions : MonoBehaviour
{
    public List<TextMeshPro> texts;
    private float timeToHide;
    private bool showing = false;

    private void Update()
    {
        if (showing && Time.time > timeToHide) Hide();
    }

    public void Show()
    {
        showing = true;
        timeToHide = Time.time + 5.5f;
        foreach (TextMeshPro text in texts)
        {
            text.DOColor(new Color(1, 1, 1, 1), 1f);
        }
    }

    public void Hide()
    {
        if (!showing) return;

        showing = false;
        foreach (TextMeshPro text in texts)
        {
            text.DOColor(new Color(1, 1, 1, 0), .5f);
        }
    }
}

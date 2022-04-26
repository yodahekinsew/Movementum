using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Shield : MonoBehaviour
{
    public Vector3 shownScale;

    public void Show()
    {
        transform.DOScale(shownScale, .5f);
    }

    public void Hide()
    {
        transform.DOScale(Vector3.zero, .15f);
    }
}

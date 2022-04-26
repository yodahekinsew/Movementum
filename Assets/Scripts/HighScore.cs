using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HighScore : MonoBehaviour
{
    public TextMeshProUGUI scoreText;
    public void UpdateHighscore()
    {
        scoreText.text = "" + PlayerPrefs.GetInt("HighScore");
    }
}

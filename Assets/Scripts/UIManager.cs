using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public Animator anim;
    public HighScore highScore;

    public void StartPlaying()
    {
        anim.SetTrigger("StartPlaying");
    }

    public void StopPlaying()
    {
        anim.SetTrigger("StopPlaying");
    }
    
    public void ShowMenu()
    {
        if (GameManager.state == GameState.Start) anim.SetTrigger("ShowStartMenu");
        if (GameManager.state == GameState.Play) anim.SetTrigger("ShowPlayMenu");
    }

    public void HideMenu()
    {
        if (GameManager.state == GameState.Start) anim.SetTrigger("HideStartMenu");
        if (GameManager.state == GameState.Pause) anim.SetTrigger("HidePlayMenu");
    }

    public void EndGame()
    {
        highScore.UpdateHighscore();
        anim.SetTrigger("EndGame");
    }

    public void PlayAgain()
    {
        anim.SetTrigger("PlayAgain");
    }
}

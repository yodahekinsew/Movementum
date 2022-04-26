using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ScoreCounter : MonoBehaviour
{
    public TextMeshPro text;
    public Animator anim;
    public Leaderboard leaderboard;
    public StreakCounter streak;
    public int coinPoints;

    private int score = 0;
    private int highScore = 0;

    private void Start()
    {
        if (!PlayerPrefs.HasKey("HighScore")) PlayerPrefs.SetInt("HighScore", 0);
        else highScore = PlayerPrefs.GetInt("HighScore");
        ShowHighscore();
    }

    public void ShowHighscore()
    {
        text.text = "" + highScore;
    }

    public void ShowScore()
    {
        text.text = "" + score;
    }

    public void ResetScore()
    {
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
        }
        score = 0;
    }

    public void Increment()
    {
        anim.SetTrigger("Increment");
    }

    public void IncrementCoin()
    {
        anim.SetTrigger("IncrementCoin");
    }

    public void AddCoin()
    {
        score += coinPoints * streak.GetStreak();
        text.text = "" + score;
    }

    public void AddOne()
    {
        score += streak.GetStreak();
        text.text = "" + score;
        CheckHighscore();
    }

    private void CheckHighscore()
    {
        if (score > highScore)
        {
            highScore = score;
            PlayerPrefs.SetInt("HighScore", highScore);
        }
    }

    public void MoveEndPosition()
    {
        anim.SetTrigger("MoveEnd");
    }

    public void MoveNormalPosition()
    {
        anim.SetTrigger("MoveNormal");
    }

    public void PlayAgain()
    {
        anim.SetTrigger("PlayAgain");
    }

    public void SecondChance()
    {
        anim.SetTrigger("SecondChance");
    }

    public void StopPlaying()
    {
        anim.SetTrigger("StopPlaying");
    }

    public void StartPlaying()
    {
        anim.SetTrigger("StartPlaying");
    }

    public void EndGame()
    {
        leaderboard.UpdateLeaderboard(highScore);
        MoveEndPosition();
    }
}

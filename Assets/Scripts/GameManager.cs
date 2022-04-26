using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Advertisements;

public enum GameState
{
    Start,
    Play,
    Pause,
    End
}

public class GameManager : MonoBehaviour
{
    const float UNPAUSE_DELAY = 1;
    public static GameState state = GameState.Start;
    public static float difficulty;

    public UIManager ui;
    public AudioManager audio;
    public Player player;
    public Instructions instructions;
    public RewardedAdsButton rewardedAdsButton;
    private bool usedSecondChance = false;

    [Header("Difficulty")]
    public float difficultyRate;

    [Header("Game Pieces")]
    public ScoreCounter scoreCounter;
    public StreakCounter streakCounter;
    public EnemySpawner enemySpawner;
    public BulletPool bulletPool;
    public Coin coin;
    public PowerupManager powerups;

    private delegate void func();

    void Start()
    {
        Application.targetFrameRate = 60;
        difficulty = 0;
    }

    private void Update()
    {
        if (state == GameState.Play)
        {
            difficulty += difficultyRate * Time.deltaTime;
            difficulty = Mathf.Clamp(difficulty, 0, 1);
        }
    }

    public float GetDifficulty()
    {
        return difficulty;
    }

    public void ResetDifficulty()
    {
        difficulty = 0;
    }

    public void LowerDifficulty()
    {
        difficulty = Mathf.Max(difficulty - .1f, 0);
    }

    public void StartPlaying()
    {
        state = GameState.Play;
        ui.StartPlaying();
        audio.StartPlaying();

        // Activating Game Pieces
        scoreCounter.StartPlaying();
        streakCounter.Show();
        player.ActivateControls();
        enemySpawner.Activate();
        coin.Activate();
        powerups.Activate();

        instructions.Show();
    }

    public void EndGame()
    {
        audio.EndGame();
        instructions.Hide();
        StartCoroutine(WaitToCall(() =>
        {
            state = GameState.End;
            ui.EndGame();

            // Deactivating Game Pieces
            streakCounter.Hide();
            player.DeactivateControls();
            scoreCounter.EndGame();
            bulletPool.StopBullets();
            enemySpawner.DestroyEnemies();
            enemySpawner.Deactivate();
            coin.Deactivate();
            powerups.Deactivate();

            // Advertisements
            if (usedSecondChance) rewardedAdsButton.rewardButton.interactable = false;
            else rewardedAdsButton.rewardButton.interactable = true;
        }, .5f));
    }

    public void StopPlaying()
    {
        usedSecondChance = false;

        state = GameState.Start;
        ui.StopPlaying();
        audio.StopPlaying();

        scoreCounter.StopPlaying();
        player.Appear();

        ResetDifficulty();
    }

    public void PlayAgain()
    {
        usedSecondChance = false;

        state = GameState.Play;
        ui.PlayAgain();
        audio.PlayAgain();
        scoreCounter.PlayAgain();

        // Activating Game Pieces
        streakCounter.PlayAgain();
        player.Appear();
        player.ActivateControls();
        enemySpawner.Activate();
        coin.Activate();
        powerups.Activate();

        LowerDifficulty();
    }

    public void Continue()
    {
        usedSecondChance = true;

        state = GameState.Play;
        ui.PlayAgain();
        audio.PlayAgain();
        scoreCounter.SecondChance();
        scoreCounter.ShowScore();

        // Activating Game Pieces
        streakCounter.PlayAgain();
        player.Appear();
        player.ActivateControls();
        enemySpawner.Activate();
        coin.Activate();
        powerups.Activate();

        LowerDifficulty();
    }

    public void ShowMenu()
    {
        ui.ShowMenu();
        if (state == GameState.Play) PauseGame();
    }

    public void HideMenu()
    {
        ui.HideMenu();
        if (state == GameState.Pause) StartCoroutine(WaitToCall(UnpauseGame, UNPAUSE_DELAY));
    }

    public void PauseGame()
    {
        state = GameState.Pause;

        // Deactivating Game Pieces
        player.DeactivateControls();
        enemySpawner.Deactivate();
        coin.Deactivate();
        powerups.Deactivate();
    }

    public void UnpauseGame()
    {
        state = GameState.Play;

        // Activating Game Pieces
        player.ActivateControls();
        enemySpawner.Activate();
        coin.Activate();
        powerups.Activate();
    }

    private IEnumerator WaitToCall(func functionToCall, float timeToWait)
    {
        yield return new WaitForSeconds(timeToWait);
        functionToCall();
    }
}

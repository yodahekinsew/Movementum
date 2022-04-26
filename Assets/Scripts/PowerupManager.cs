using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PowerupManager : MonoBehaviour
{
    public Player player;
    public List<Powerup> powerups;
    public float powerupSpawnDelay; // Time in between powerups
    private float timeSincePowerup;
    private int enemiesKilledSincePowerup = 0;
    private bool powerupSpawned = false;
    private Powerup currentPowerup;
    private bool activated = false;

    void Update()
    {
        if (!activated) return;
        if (!powerupSpawned) timeSincePowerup += Time.deltaTime;
    }

    public void TrySpawnPowerup()
    {
        bool canSpawnPowerup = !powerupSpawned
            && timeSincePowerup >= powerupSpawnDelay
            && (Random.value >= .8f || enemiesKilledSincePowerup >= 10);

        if (canSpawnPowerup)
        {
            print("SPAWNING A POWERUP");
            powerupSpawned = true;
            enemiesKilledSincePowerup = 0;
            if (player.hasShield) currentPowerup = powerups[Random.Range(1, powerups.Count)];
            else currentPowerup = powerups[Random.Range(0, powerups.Count)];
            currentPowerup.Activate();
        }
        else enemiesKilledSincePowerup++;
    }

    public void Activate()
    {
        activated = true;
    }

    public void Deactivate()
    {
        activated = false;
        if (powerupSpawned) currentPowerup.Deactivate();
        enemiesKilledSincePowerup = 0;
    }

    public void PowerupDeactivated()
    {
        print("POWERUP HAS BEEN DEACTIVED");
        timeSincePowerup = 0;
        powerupSpawned = false;
    }
}

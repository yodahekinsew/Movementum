using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class TimePowerup : Powerup
{
    public override void ActivatePowerup()
    {
        if (powerupActivated) return;
        print("Activating Time Powerup");
        powerupActivated = true;
        Time.timeScale = .5f;
        HidePowerup();
    }

    public override void DeactivatePowerup()
    {
        if (!powerupActivated) return;
        print("Deactivating Time Powerup");
        powerupActivated = false;
        Time.timeScale = 1f;
        Deactivate();
    }
}

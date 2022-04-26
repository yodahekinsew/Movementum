using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShieldPowerup : Powerup
{
    public Player player;

    public override void ActivatePowerup()
    {
        if (powerupActivated) return;
        print("Activating Shield Powerup");
        powerupActivated = true;
        player.ActivateShield();
        HidePowerup();
    }

    public override void DeactivatePowerup()
    {
        if (!powerupActivated) return;
        print("Deactivating Shield Powerup");
        powerupActivated = false;
        Deactivate();
    }
}

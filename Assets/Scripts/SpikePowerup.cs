using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikePowerup : Powerup
{
    public Player player;

    public override void ActivatePowerup()
    {
        if (powerupActivated) return;
        print("Activating Spike Powerup");
        powerupActivated = true;
        player.ActivateSpike();
        HidePowerup();
    }

    public override void DeactivatePowerup()
    {
        if (!powerupActivated) return;
        print("Deactivating Spike Powerup");
        powerupActivated = false;
        player.DeactivateSpike();
        Deactivate();
    }
}

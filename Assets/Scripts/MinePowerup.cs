using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class MinePowerup : Powerup
{
    public float explosionRadius;
    public Player player;
    public EnemySpawner enemySpawner;
    public BulletPool bulletPool;
    public Transform explosion;
    public SpriteRenderer explosionRing;
    public SpriteRenderer minePickup;

    public override void ActivatePowerup()
    {
        if (powerupActivated) return;
        powerupActivated = true;
        player.SetInvincible(true);
        minePickup.sortingOrder = 10;
        HidePowerup();

        explosion.DOScale(Vector3.one * .6f * explosionRadius, .6f).OnComplete(() =>
        {
            explosionRing.DOColor(new Color(1, 1, 1, 0), .2f);
            explosion.DOScale(Vector3.one * explosionRadius, .2f).OnComplete(() =>
            {
                player.SetInvincible(false);
                bulletPool.StopBullets();
                enemySpawner.DestroyEnemies();
                // Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, explosionRadius, LayerMask.GetMask("Enemy"));
                // foreach (Collider2D collider in colliders) collider.gameObject.GetComponent<Enemy>().Kill();
                explosion.localScale = Vector3.zero;
                explosionRing.color = new Color(1, 1, 1, 1);
                Deactivate();
            });
        });
    }

    public override void DeactivatePowerup()
    {
        if (!powerupActivated) return;
        powerupActivated = false;
    }
}

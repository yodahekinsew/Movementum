using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Powerup : MonoBehaviour
{
    public Transform playerTransform;
    public PowerupManager manager;
    public float powerupDuration;

    private SpriteRenderer sprite;
    private BoxCollider2D collider;
    private bool activated = false;
    protected bool powerupActivated = false;
    private float powerupTimer;
    private bool showing;
    private float halfHeight;
    private float halfWidth;

    void Start()
    {
        halfHeight = Camera.main.orthographicSize;
        halfWidth = Camera.main.aspect * halfHeight;

        sprite = GetComponent<SpriteRenderer>();
        collider = GetComponent<BoxCollider2D>();

        sprite.color = new Color(1, 1, 1, 0);
    }

    void Update()
    {
        if (!activated) return;

        if (showing)
        {
            Collider2D checkPlayer = Physics2D.OverlapCircle(transform.position, 1, LayerMask.GetMask("Player"));
            if (checkPlayer != null)
            {
                powerupTimer = powerupDuration;
                ActivatePowerup();
            }
        }

        if (!powerupActivated) return;
        powerupTimer -= Time.unscaledDeltaTime;
        if (powerupTimer < 0) DeactivatePowerup();
    }

    public void Activate()
    {
        activated = true;
        float spawnY = 0;
        if (playerTransform.position.y < 0) spawnY = Random.Range(2, halfHeight - 4f);
        if (playerTransform.position.y > 0) spawnY = Random.Range(-halfHeight + 2.5f, -2);
        transform.position = new Vector3(
            Random.Range(-halfWidth + 1.5f, halfWidth - 1.5f),
            spawnY,
            transform.position.z
        );
        ShowPowerup();
    }

    public void ShowPowerup()
    {
        showing = true;
        sprite.DOColor(new Color(1, 1, 1, 1), .5f).SetUpdate(true);
    }

    public void HidePowerup()
    {
        showing = false;
        collider.enabled = false;
        sprite.DOColor(new Color(1, 1, 1, 0), .5f).SetUpdate(true);
    }

    public void Deactivate()
    {
        activated = false;
        HidePowerup();
        if (powerupActivated) DeactivatePowerup();
        manager.PowerupDeactivated();
    }

    // Powerup specific methods below
    public virtual void ActivatePowerup()
    {
        if (powerupActivated) return;
        powerupActivated = true;
    }

    public virtual void DeactivatePowerup()
    {
        if (!powerupActivated) return;
    }
}

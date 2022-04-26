using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    private Transform player;
    private ScoreCounter scoreCounter;
    private PowerupManager powerups;
    public float speed;
    public SpriteRenderer sprite;
    public CircleCollider2D collider;
    public ParticleSystem explosion;
    private bool killed = false;
    public AudioClip explosionEffect;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.Find("/Player").transform;
        scoreCounter = GameObject.Find("/ScoreCounter").GetComponent<ScoreCounter>();
        powerups = GameObject.Find("/Powerups").GetComponent<PowerupManager>();
    }

    // Update is called once per frame
    void Update()
    {
        if (GameManager.state != GameState.Play) return;
        if (killed) return;

        Vector3 oldPosition = transform.position;
        transform.position = Vector3.MoveTowards(transform.position, player.position, speed * Time.deltaTime);
        float distanceMoved = Vector3.Distance(oldPosition, transform.position);
        transform.Rotate(Vector3.forward * 180 * Time.deltaTime);

        RaycastHit2D hitPlayer = Physics2D.Raycast(oldPosition, transform.up, distanceMoved, LayerMask.GetMask("Player"));
        if (hitPlayer.collider != null)
        {
            Player playerObj = player.GetComponent<Player>();
            if (playerObj.hasShield)
            {
                playerObj.DeactivateShield();
                Kill();
            }
            else playerObj.Kill();
        }
    }

    public void Kill()
    {
        AudioManager.instance.PlaySFX(explosionEffect, .5f);
        killed = true;
        collider.enabled = false;
        sprite.enabled = false;
        explosion.Play();
        scoreCounter.Increment();
        powerups.TrySpawnPowerup();
        Destroy(gameObject, .5f);
    }
}

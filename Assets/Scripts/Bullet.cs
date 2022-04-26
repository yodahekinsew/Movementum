using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    public SpriteRenderer sprite;
    public BoxCollider2D collider;
    public ParticleSystem explosion;
    public ParticleSystem blackExplosion;
    public Transform shadow;
    public SpriteRenderer shadowSprite;

    private bool switched = false;
    private bool firing = false;

    void Update()
    {
        if (GameManager.state != GameState.Play) return;

        if (firing)
        {
            float distance = 10 * Time.deltaTime;

            // Check for hitting the player if switched
            if (switched)
            {
                RaycastHit2D hitPlayer = Physics2D.Raycast(transform.position, transform.up, distance, LayerMask.GetMask("Player"));
                if (hitPlayer.collider != null)
                {
                    Player playerObj = hitPlayer.transform.GetComponent<Player>();
                    if (playerObj.hasShield) playerObj.DeactivateShield();
                    else playerObj.Kill();
                    Stop();
                    return;
                }
            }

            // Check for hitting an enemy if not switched
            if (!switched)
            {
                RaycastHit2D hitEnemy = Physics2D.Raycast(transform.position, transform.up, distance, LayerMask.GetMask("Enemy"));
                if (hitEnemy.collider != null)
                {
                    hitEnemy.transform.GetComponent<Enemy>().Kill();
                    Stop();
                    return;
                }
            }

            // Check for hitting bounds
            RaycastHit2D hitBounds = Physics2D.Raycast(transform.position, transform.up, distance, LayerMask.GetMask("Bounds"));
            if (hitBounds.collider != null)
            {
                if (!switched)
                {
                    transform.up = Vector3.Reflect(transform.up, hitBounds.normal);
                    sprite.color = Color.black;
                    switched = true;
                }
                else Stop();
            }
            transform.position += transform.up * distance;
        }

        shadow.position = transform.position + new Vector3(.1f, -.1f, .1f);
    }

    public void Fire()
    {
        var main = explosion.main;
        main.startColor = Color.white;
        sprite.enabled = true;
        collider.enabled = true;
        firing = true;
    }

    public void Stop(bool explode = true)
    {
        if (explode)
        {
            if (switched) blackExplosion.Play();
            else explosion.Play();
        }
        sprite.enabled = false;
        shadowSprite.enabled = false;
        collider.enabled = false;
        sprite.color = Color.white;
        firing = false;
        switched = false;
    }
}

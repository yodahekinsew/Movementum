using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coin : MonoBehaviour
{
    public Transform player;
    private float halfHeight;
    private float halfWidth;

    [Header("Scoring")]
    public ScoreCounter scoreCounter;

    [Header("Spawning")]
    public float initialSpawnDelay;
    public float spawnWait;
    private float spawnTimer;

    public Animator anim;

    private bool activated;
    private bool showing;
    private bool available;

    // Start is called before the first frame update
    void Start()
    {
        halfHeight = Camera.main.orthographicSize;
        halfWidth = Camera.main.aspect * halfHeight;
    }

    // Update is called once per frame
    void Update()
    {
        if (!activated) return;

        if (!showing) spawnTimer -= Time.deltaTime;
        if (!showing && spawnTimer < 0) ShowCoin();
        if (available)
        {
            Collider2D checkPlayer = Physics2D.OverlapCircle(transform.position, 1, LayerMask.GetMask("Player"));
            if (checkPlayer != null)
            {
                // Coin is collected !
                scoreCounter.IncrementCoin();
                HideCoin();
            }
        }
    }

    void ShowCoin()
    {
        showing = true;
        float spawnY = 0;
        if (player.position.y < 0) spawnY = Random.Range(1, halfHeight - 4f);
        if (player.position.y > 0) spawnY = Random.Range(-halfHeight + 2.5f, -1);
        transform.position = new Vector3(
            Random.Range(-halfWidth + 1.5f, halfWidth - 1.5f),
            spawnY,
            transform.position.z
        );
        anim.SetTrigger("ShowCoin");
    }

    void FinishedShowing()
    {
        available = true;
    }

    void HideCoin()
    {
        anim.SetTrigger("HideCoin");
        showing = false;
        available = false;
        spawnTimer = spawnWait;
    }

    public void Activate()
    {
        activated = true;
        spawnTimer = spawnWait;
    }

    public void Deactivate()
    {
        activated = false;
        HideCoin();
    }
}

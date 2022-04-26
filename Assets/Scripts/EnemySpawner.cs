using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    public GameObject enemy;
    public float spawnDelay;
    public float minSpawnRate;
    public float maxSpawnRate;

    private float nextTimeToSpawn;
    private float spawnRadiusHeight;
    private float spawnRadiusWidth;
    private List<GameObject> spawnedEnemies;

    private bool activated = false;

    void Start()
    {
        spawnRadiusHeight = 1.25f * Camera.main.orthographicSize;
        spawnRadiusWidth = spawnRadiusHeight * Screen.width / Screen.height;
    }

    void Update()
    {
        if (!activated) return;

        if (Time.time >= nextTimeToSpawn)
        {
            GameObject newEnemy = Instantiate(enemy);
            newEnemy.transform.parent = transform;
            Vector3 randomSpawn = Vector3.zero;
            if (Random.value > .5)
            {
                randomSpawn.x = Random.Range(-spawnRadiusWidth, spawnRadiusWidth);
                if (Random.value > .5) randomSpawn.y = spawnRadiusHeight;
                else randomSpawn.y = -spawnRadiusHeight;
            }
            else
            {
                randomSpawn.y = Random.Range(-spawnRadiusHeight, spawnRadiusHeight);
                if (Random.value > .5) randomSpawn.x = spawnRadiusWidth;
                else randomSpawn.x = -spawnRadiusWidth;
            }
            newEnemy.transform.position = randomSpawn;
            nextTimeToSpawn = Time.time + 1 / Mathf.Lerp(minSpawnRate, maxSpawnRate, GameManager.difficulty);
        }
    }

    public void DestroyEnemies()
    {
        foreach (Transform child in transform)
        {
            GameObject.Destroy(child.gameObject);
        }
    }

    public void Activate()
    {
        activated = true;
        nextTimeToSpawn = Time.time + spawnDelay;
    }

    public void Deactivate()
    {
        activated = false;
    }
}

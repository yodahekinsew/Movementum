using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour
{
    public GameObject bullet;
    public float maxBullets;
    private List<GameObject> bullets = new List<GameObject>();
    private int bulletIndex = 0;

    public GameObject GetBullet()
    {
        if (bullets.Count > maxBullets)
        {
            GameObject oldBullet = bullets[bulletIndex];
            bulletIndex++;
            if (bulletIndex >= bullets.Count) bulletIndex = 0;
            return oldBullet;
        }
        else
        {
            GameObject newBullet = Instantiate(bullet);
            newBullet.transform.parent = transform;
            bullets.Add(newBullet);
            return newBullet;
        }
    }

    public void StopBullets()
    {
        foreach (GameObject bulletObject in bullets)
        {
            bulletObject.GetComponent<Bullet>().Stop(false);
        }
    }
}

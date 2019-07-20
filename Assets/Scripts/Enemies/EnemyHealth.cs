using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    public int health = 1;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Bullet") && other.GetComponent<Bullet>()?.hostile == false) // note that the ?. returns a bool? so we need an explicit truth check
        {
            takeDamage(1);
            other.GetComponent<Bullet>().BulletDestroy ();
        }
    }
    public void takeDamage(int damage)
    {
        health-=damage;
        if (health <= 0) Destroy(gameObject);
    }
}

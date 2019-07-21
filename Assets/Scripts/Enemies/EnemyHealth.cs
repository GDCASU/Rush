using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : MonoBehaviour
{
    private float health = 1;
    public float maxHealth = 10;
    public delegate void damageFunction(float damageDealt, float healthAfterDamage);
    public event damageFunction onEnemyDamage;
    public event Action OnDeath;
    public void Awake() => health = maxHealth;

    public float healthPercent {
        get => (float)100 * (health / maxHealth);
    }
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Bullet") && other.GetComponent<Bullet>()?.hostile == false) // note that the ?. returns a bool? so we need an explicit truth check
        {
            takeDamage(1);
            other.GetComponent<Bullet>().BulletDestroy ();
        }
    }
    public void takeDamage(float damage)
    {
        health-=damage;
        onEnemyDamage?.Invoke(damage,health);
        if (health <= 0) {
            OnDeath?.Invoke();
            Destroy(gameObject);
        }
    }
}

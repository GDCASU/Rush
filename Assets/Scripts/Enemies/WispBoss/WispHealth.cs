using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Child of the EnemyHealth class. This is a modified version of that class
/// where the boss cannot take damage unless set to.
/// </summary>
public class WispHealth : EnemyHealth
{
    public WispBossA1 Phase2
    {
        get
        {
            return GetComponent<WispBossA1>();
        }
    }

    public bool canTakeDamage = false;

    /// <summary>
    /// Damages the boss on collision if it can take damage
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Bullet") && other.GetComponent<Bullet>()?.hostile == false) // note that the ?. returns a bool? so we need an explicit truth check
        {
            if (canTakeDamage)
            {
                takeDamage(1);
                other.GetComponent<Bullet>().BulletDestroy();
            }
        }
    }

    public void takeDamage(float damage)
    {
        base.takeDamage(damage);

        if(Phase2.actionRunning)
        {
            Phase2.OnTakenDamage(damage);
        }
    }
}

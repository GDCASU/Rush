﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Child of the EnemyHealth class. This is a modified version of that class
/// where the boss cannot take damage unless set to.
/// </summary>
public class WispHealth : EnemyHealth
{
    public bool canTakeDamage = false;

    /// <summary>
    /// Damages the boss on collision if it can take damage
    /// </summary>
    /// <param name="other"></param>
    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.CompareTag("Bullet") && other.GetComponent<Bullet>()?.hostile == false) // note that the ?. returns a bool? so we need an explicit truth check
        {
            other.GetComponent<Bullet>().BulletDestroy();

            if (canTakeDamage)
                takeDamage(1);
        }
    }
}

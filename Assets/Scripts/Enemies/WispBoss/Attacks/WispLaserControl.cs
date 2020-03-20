using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that basically let's the player take damage
/// </summary>
public class WispLaserControl : MonoBehaviour
{
    public bool canDamagePlayer;

    private void OnTriggerStay2D(Collider2D collision)
    {
        if(canDamagePlayer && collision.CompareTag("Player") && !collision.GetComponent<PlayerHealth>().inv)
        {
            collision.GetComponent<PlayerHealth>().takeDamage();
        }
    }
}

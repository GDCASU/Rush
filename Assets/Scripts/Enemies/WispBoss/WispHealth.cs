using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Child of the EnemyHealth class. This is a modified version of that class
/// where the boss cannot take damage unless set to.
/// </summary>
public class WispHealth : EnemyHealth
{
    private bool canTakeDamage = false;
    public float attackDuration = 5f;    //How long the boss can take damage before it no longer can
    private IEnumerator attackRoutine;

    /// <summary>
    /// Method that allows the boss to be damaged. In terms of the wisp this
    /// would be the sort of "imbued sword" phase
    /// </summary>
    public void SetCanTakeDamage()
    {
        //Starts a new routine where the boss can be damaged
        if(attackRoutine == null)
        {
            canTakeDamage = true;
            attackRoutine = WaitStopAttack();
            StartCoroutine(attackRoutine);
        }
        //Resets the routine of boss damage
        else
        {
            StopCoroutine(attackRoutine);
            StartCoroutine(attackRoutine);
        }
    }

    /// <summary>
    /// Coroutine that has the boss stop taking damage after
    /// the time set by the var attackDuration
    /// </summary>
    private IEnumerator WaitStopAttack()
    {
        yield return new WaitForSeconds(attackDuration);

        canTakeDamage = false;

        attackRoutine = null;

        yield return null;
    }

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

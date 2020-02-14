using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Bullet class used for the wisp boss. Adds in the ability to track
/// the player for a set amount of time
/// </summary>
public class WispBullet : Bullet
{
    public float homingDuration = 5f;

    public void EnableHoming()
    {
        StartCoroutine(HomingRoutine());
    }

    private IEnumerator HomingRoutine()
    {
        print(homingDuration);

        while(homingDuration > 0)
        {
            MoveVector = Vector3.Normalize(PlayerHealth.singleton.transform.position - transform.position) * speed;

            homingDuration -= Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }
    }
}

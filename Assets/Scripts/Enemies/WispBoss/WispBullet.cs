using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Bullet class used for the wisp boss. Adds functionality for the following:
/// Homing
/// Coupling
/// </summary>
public class WispBullet : Bullet
{
    [Header("Homing")]
    public float homingDuration = 5f;

    [Header("Coupling")]
    public GameObject[] coupledObjects;

    public void EnableHomingTurret()
    {
        StartCoroutine(HomingRoutine(homingDuration));
    }

    /// <summary>
    /// Special homing movement that lasts forevor
    /// </summary>
    public void EnableHomingBig()
    {
        StartCoroutine(HomingRoutine(-1));
    }

    /// <summary>
    /// Coroutine that handles the homing logic
    /// </summary>
    /// <param name="timeToFollow">Duration the bullet follows the player. Enter -1 for no duration</param>
    /// <returns></returns>
    private IEnumerator HomingRoutine(float timeToFollow)
    {
        while (timeToFollow == -1 || timeToFollow > 0)
        {
            MoveVector = Vector3.Normalize(PlayerHealth.singleton.transform.position - transform.position) * speed;

            if(timeToFollow != -1)
                timeToFollow -= Time.deltaTime;

            yield return new WaitForEndOfFrame();
        }
    }

    /// <summary>
    /// Enables the coupling objects and then disables the parent collider 
    /// (as it should not be used here)
    /// </summary>
    public void EnableCoupling()
    {
        foreach(GameObject obj in coupledObjects)
        {
            obj.SetActive(true);
        }

        GetComponent<CircleCollider2D>().enabled = false;
    }
}

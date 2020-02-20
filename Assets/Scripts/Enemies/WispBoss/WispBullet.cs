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

    public void EnableHoming()
    {
        StartCoroutine(HomingRoutine());
    }

    private IEnumerator HomingRoutine()
    {
        while(homingDuration > 0)
        {
            MoveVector = Vector3.Normalize(PlayerHealth.singleton.transform.position - transform.position) * speed;

            homingDuration -= Time.deltaTime;

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

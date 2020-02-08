using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Simple class on each chain that makes the chain visiable
/// as the chain moves towards it's turret
/// </summary>
public class ChainVisibleHandler : MonoBehaviour
{
    public GameObject bossObj;

    /// <summary>
    /// Makes the chain link visiable as it exits the
    /// bosses collider
    /// </summary>
    /// <param name="collision"></param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        if(collision.gameObject == bossObj)
        {
            gameObject.layer = LayerMask.GetMask("Default");
        }
    }
}

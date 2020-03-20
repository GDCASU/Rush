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
    public GameObject targetObj;

    private float bossToTargetDist; //This is the distance from the boss obj to the target obj
    private bool isVisible = false;

    private void Start()
    {
        bossToTargetDist = Vector3.Magnitude(targetObj.transform.position - bossObj.transform.position);
    }

    /// <summary>
    /// This sets this chain link to be visible if it's position is between the boss
    /// and the target obj (turret). This method helped ensure more accurate timing
    /// on visibility
    /// </summary>
    /// <param name="collision">Object colliding with</param>
    private void OnTriggerStay2D(Collider2D collision)
    {
        if(!isVisible && collision.gameObject == bossObj)
        {
            float linkToTargetDist = Vector3.Magnitude(targetObj.transform.position - transform.position);

            if(linkToTargetDist < bossToTargetDist)
            {
                gameObject.layer = LayerMask.GetMask("Default");
                isVisible = true;
            }
        }
    }
}

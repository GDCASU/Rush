using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that handles the teleportation runes for the
/// rage mage boss. This involves activating a random pre
/// located rune on the ground and teleporting to it. If the
/// player is within a certain radius of the boss after teleportation
/// they are damaged.
/// 
/// NOTE: As of 4/3/20 this class assumes that it is on a child obj
/// of the boss prefab. This means that when teleporting the boss it moves
/// this.gameObject.transform when teleporting. If this is no the case later
/// make sure to adjust this
/// </summary>
public class RageMageA4 : BossAction
{
    public Transform TeleportSpotParent;
    private Transform[] _teleportSpots;

    public float DmgRadius;
    public float TeleportDelay;
    private int _currentSpotIndex = -1;

    private void Awake()
    {
        _teleportSpots = new Transform[TeleportSpotParent.childCount];
        for(int x = 0; x < TeleportSpotParent.childCount; x++)
        {
            _teleportSpots[x] = TeleportSpotParent.GetChild(x);
        }

        //Teleport();
    }

    /// <summary>
    /// Public method that starts the teleport routine
    /// </summary>
    public void Teleport()
    {
        StartCoroutine(TeleportRoutine());
    }

    /// <summary>
    /// Routine that handles the entire action.
    /// See class comments for a detailed description
    /// </summary>
    /// <returns></returns>
    private IEnumerator TeleportRoutine()
    {
        //Activates a random teleport rune
        _currentSpotIndex = Random.Range(0, _teleportSpots.Length);
        _teleportSpots[_currentSpotIndex].gameObject.SetActive(true);

        //Wait before teleporting the boss
        yield return new WaitForSeconds(TeleportDelay);

        //Teleports the boss
        transform.parent.position = _teleportSpots[_currentSpotIndex].position;

        //Damages the player if they are close enough
        float distToPlayer = Vector3.Distance(PlayerHealth.singleton.transform.position, transform.position);
        if(distToPlayer < DmgRadius)
        {
            PlayerHealth.singleton.takeDamage();
        }

        //Deactivates the rune
        _teleportSpots[_currentSpotIndex].gameObject.SetActive(false);

        yield return null;
    }
}

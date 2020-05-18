using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Wisp basic attack that fires a burst of 3 bullets at the player
/// </summary>
public class WispBasicAttack : WispAttackModel
{
    public int bulletCount = 3;
    public float burstSpacing = 0.1f;
    public float delay = 1;

    private void Start()
    {
        _attackRoutine = Attack();
    }

    private IEnumerator Attack()
    {
        while(true)
        {
            for(int x = 0; x < bulletCount; x++)
            {
                Spawner.enabled = true;
                Spawner.enabled = false;
                yield return new WaitForSeconds(burstSpacing);
            }

            yield return new WaitForSeconds(delay);
        }
    }
}

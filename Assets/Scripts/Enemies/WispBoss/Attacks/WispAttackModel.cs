using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Sort of parent model for all wisp attacks. This unification
/// forces all attacks to be of the same type and have the attack method below
/// </summary>
public class WispAttackModel : MonoBehaviour
{
    protected IEnumerator _attackRoutine;

    protected BulletSpawner Spawner
    {
        get
        {
            return GetComponent<BulletSpawner>();
        }
    }

    public void StartAttacking()
    {
        if (_attackRoutine != null)
            StartCoroutine(_attackRoutine);
    }
}

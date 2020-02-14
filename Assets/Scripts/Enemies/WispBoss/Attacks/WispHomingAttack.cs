using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This is a custom version of the BulletSpawner class. This class
/// enables the homing capabilities when spawning in it's bullets
/// </summary>
public class WispHomingAttack : BulletSpawner
{
    private float currentOffset;

    [HideInInspector]
    private bool SpawnOn = false;
    IEnumerator SpawnLoop()
    {
        if (SpawnOn || BulletPool.singleton == null) yield break; // ensure we don't start twice due to race condition;
        SpawnOn = true;
        while (SpawnOn)
        {
            if (offsetFacesPlayer)
            {
                Vector2 VectorToPlayer = PlayerHealth.singleton.transform.position - (useOtherSpawnPoint ? alternateSpawnLocation : transform).position;
                ArcOffset = (float)(Vector2.SignedAngle(Vector2.right, VectorToPlayer) * Mathf.PI / 180.0);
            }
            for (int i = 1; i <= bulletAmount; i++)
            {
                float angle = ArcOffset + (bulletArc / bulletAmount) * ((float)(i - 1) - (bulletAmount - 1) / (float)2.0) + currentOffset;
                Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

                var sp = BulletPool.rent();
                sp.transform.position = (useOtherSpawnPoint ? alternateSpawnLocation : transform).position;
                sp.GetComponent<Bullet>().Init(direction, (bulletsFaceOutward) ? angle * (float)180.0 / Mathf.PI : 0, bulletSpeed, moveFunc, spawnFunc, bulletSprite, bulletTint, true, colliderRadius, SpawnFunctionParams);

                sp.transform.localScale = scale;
                sp.GetComponent<WispBullet>().EnableHoming();
            }
            yield return new WaitForSeconds(bulletfrequency);
        }
    }

    void OnDisable() => SpawnOn = false;

    void Start() => StartCoroutine(SpawnLoop());
    void OnEnable() => StartCoroutine(SpawnLoop());
}

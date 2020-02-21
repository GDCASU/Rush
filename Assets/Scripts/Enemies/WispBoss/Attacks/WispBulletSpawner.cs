using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// I'm sorry but I kinda gave up trying to figure out how to use BulletSpawner
/// properly so this class derives from it and has copied and pasted code
/// 
/// Note that I had to add a 2 for the remade variables or else it gets made at me but
/// it couldn't be helped because of their access modifiers :(
/// </summary>
public class WispBulletSpawner : BulletSpawner
{
    [Header("Wisp Toggles")]
    public bool enableHoming;
    public bool enableCoupling;
    public bool enableBigBullet;

    public GameObject spawnedBigBullet; 
    private float currentOffset2;

    [HideInInspector]
    private bool SpawnOn2 = false;
    IEnumerator SpawnLoop()
    {
        if (SpawnOn2 || BulletPool.singleton == null) yield break; // ensure we don't start twice due to race condition;
        SpawnOn2 = true;
        while (SpawnOn2)
        {
            //If statement that helps handle big bullet logic. Makes it so only 1 big bullet can be present at a time
            if (!(enableBigBullet && spawnedBigBullet != null))
            {
                if (offsetFacesPlayer)
                {
                    Vector2 VectorToPlayer = PlayerHealth.singleton.transform.position - (useOtherSpawnPoint ? alternateSpawnLocation : transform).position;
                    ArcOffset = (float)(Vector2.SignedAngle(Vector2.right, VectorToPlayer) * Mathf.PI / 180.0);
                }
                for (int i = 1; i <= bulletAmount; i++)
                {
                    float angle = ArcOffset + (bulletArc / bulletAmount) * ((float)(i - 1) - (bulletAmount - 1) / (float)2.0) + currentOffset2;
                    Vector2 direction = new Vector2(Mathf.Cos(angle), Mathf.Sin(angle));

                    var sp = BulletPool.rent();
                    sp.transform.position = (useOtherSpawnPoint ? alternateSpawnLocation : transform).position;
                    sp.GetComponent<Bullet>().Init(direction, (bulletsFaceOutward) ? angle * (float)180.0 / Mathf.PI : 0, bulletSpeed, moveFunc, spawnFunc, bulletSprite, bulletTint, true, colliderRadius, SpawnFunctionParams);
                    sp.transform.localScale = scale;

                    //Note: For now I have purposfully made it so that they cannot be combined
                    if (enableHoming)
                        sp.GetComponent<WispBullet>().EnableHomingTurret();
                    else if (enableCoupling)
                        sp.GetComponent<WispBullet>().EnableCoupling();
                    else if(enableBigBullet)
                    {
                        sp.GetComponent<WispBullet>().EnableHomingBig();
                        spawnedBigBullet = sp;
                    }
                }
                yield return new WaitForSeconds(bulletfrequency);
            }
            else
                yield return new WaitForEndOfFrame();
        }
    }

    void OnDisable() => SpawnOn2 = false;

    void Start() => StartCoroutine(SpawnLoop());
    void OnEnable() => StartCoroutine(SpawnLoop());
}

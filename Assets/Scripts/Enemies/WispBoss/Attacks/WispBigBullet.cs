using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// This controls the big bullet during phase 2. This bullet/attack fires a big
/// slow bullet that explodes into tiny bullets after a duration
/// </summary>
public class WispBigBullet : WispAttackModel
{
    public WispBulletSpawner WispSpawner
    {
        get
        {
            return GetComponent<WispBulletSpawner>();
        }
    }

    public float timeToExplode;
    public int explosionBulletCount;    //Count of bullets that spawn on explosion

    /**
     * A lot of bullet properties need to be set here because this
     * class swaps between firing the big bullet and it's shrapnel which
     * means the variables aren't set once for the WispBulletSpawner
     */
    [Header("Big and shrapnel modifiers")]
    public float bigBulletSpeed;
    public float shrapnelSpeed;
    public Sprite bigBulletSprite;
    public Sprite shrapnelSprite;
    public Vector3 bigBulletScale;
    public Vector3 smallBulletScale;

    private void Start()
    {
        _attackRoutine = Shoot();
    }

    private void OnDestroy()
    {
        StopCoroutine(_attackRoutine);
    }

    private IEnumerator Shoot()
    {
        while(isActiveAndEnabled)
        {
            //Setup for the big bullet
            WispSpawner.useOtherSpawnPoint = false;
            WispSpawner.bulletAmount = 1;
            WispSpawner.enableBigBullet = true;
            WispSpawner.bulletSpeed = bigBulletSpeed;
            WispSpawner.bulletSprite = bigBulletSprite;
            WispSpawner.scale = bigBulletScale;

            //Fire big bullet
            WispSpawner.enabled = true;
            WispSpawner.enabled = false;

            //Waits for the bullet to explode
            yield return new WaitForSeconds(timeToExplode);

            //Prevents shrapnel from spawning if the bullet hit the player
            if (WispSpawner.spawnedBigBullet.activeSelf)
            {
                //Setup for the shrapnel
                WispSpawner.useOtherSpawnPoint = true;
                WispSpawner.alternateSpawnLocation = WispSpawner.spawnedBigBullet.transform;
                WispSpawner.bulletAmount = explosionBulletCount;
                WispSpawner.enableBigBullet = false;
                WispSpawner.bulletSpeed = shrapnelSpeed;
                WispSpawner.bulletSprite = shrapnelSprite;
                WispSpawner.scale = smallBulletScale;

                //Fire shrapnel
                WispSpawner.enabled = true;
                WispSpawner.enabled = false;
            }

            //Destroys old big bullet
            Destroy(WispSpawner.spawnedBigBullet);
            WispSpawner.spawnedBigBullet = null;
        }
    }
}

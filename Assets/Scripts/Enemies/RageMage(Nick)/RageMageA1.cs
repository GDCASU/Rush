using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that handles the Magic Missile logic
/// for the Rage Mage boss. 
/// 
/// Description is as follows:
/// (4/2/20)
/// He will shoot out one of these magic missiles every 6 seconds. T
/// his will be a medium speed projectile that will be fired straight 
/// towards the player and ricochet off the walls two times before being 
/// destroyed. The boss will alternate between this attack and his 
/// pool of other attacks for Phase 1.
/// </summary>
public class RageMageA1 : MonoBehaviour
{
    private BulletSpawner _spawner;

    public float FireRate;
    private float _fireRateCount;

    private void Awake()
    {
        _spawner = GetComponent<BulletSpawner>();
    }

    private void Update()
    {
        //If else statement that shoots the magic missile
        if(_fireRateCount < 0)
        {
            _fireRateCount = FireRate;

            float angle = (float)(Vector2.SignedAngle(Vector2.right, PlayerHealth.singleton.transform.position - _spawner.transform.position) * Math.PI / 180.0);

            GameObject newBullet = _spawner.GenerateBullet(angle, new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)));

            //For some reason 2 components get added? and is why this if statement is here
            if (newBullet.GetComponent<RageMageMagicBullet>() == null)
                newBullet.AddComponent<RageMageMagicBullet>();

            RageMageMagicBullet magBullet = newBullet.GetComponent<RageMageMagicBullet>();
            magBullet.Bounces = 2;
        }
        else
        {
            _fireRateCount -= Time.deltaTime;
        }
    }
}

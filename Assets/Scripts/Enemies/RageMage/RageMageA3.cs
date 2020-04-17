using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Class that handles throwing potions for
/// the rage mage boss.
/// 
/// Description is as follows:
/// (4/2/20)
/// The rage mage tosses out three potions in quick succession 
/// (maybe half a second apart). Each potion is randomly chosen 
/// from the pool of potions below, each of which having a different 
/// effect upon impact with the ground. The rage mage will perform two 
/// rounds of potion tossing (three potions each, six in total), 
/// each round separated by a couple seconds, before returning to the 
/// magic missile attack.
/// 
/// Explosive Potion:
/// Upon hitting the ground, it deals a large amount of damage 
/// in one burst to the player if they are standing within range.
/// 
/// Noxious Potion:
/// Upon hitting the ground, it fills an area with noxious 
/// gasses, dealing small amounts of damage over time for as 
/// long as the player is standing within it
/// 
/// Freezing Potion:
/// Upon hitting the ground, this potion releases ice shards that 
/// travel upward from the point of impact, back downward in an arc. 
/// Areas hit by the ice shards will freeze temporarily. These spots 
/// last for 5 seconds and will slow the player if they run over them
/// </summary>
public class RageMageA3 : BossAction
{
    private BulletSpawner _spawner;

    public float ThrowDelay = 0.5f;
    public int PotionCount = 3;
    public GameObject[] Potions;

    private void Awake()
    {
        _spawner = GetComponent<BulletSpawner>();
    }

    private void Start()
    {
        actionRunning = true;
        StartCoroutine(ThrowPotionRoutine());
    }

    private IEnumerator ThrowPotionRoutine()
    {
        YieldInstruction potionDelay = new WaitForSeconds(ThrowDelay);

        for(int x = 0; x < PotionCount; x++)
        {
            //Spawns a potion spot
            RageMagePotion potion = Instantiate(Potions[UnityEngine.Random.Range(0, Potions.Length)], PlayerHealth.singleton.transform.position, PlayerHealth.singleton.transform.rotation).GetComponent<RageMagePotion>();

            //Spawns bullet that'll activate potion spot
            float angle = (float)(Vector2.SignedAngle(Vector2.right, PlayerHealth.singleton.transform.position - _spawner.transform.position) * Mathf.PI / 180.0);
            GameObject newBullet = _spawner.GenerateBullet(angle, new Vector2(Mathf.Cos(angle), Mathf.Sin(angle)));

            newBullet.GetComponent<Bullet>().onScreenDestroy = false;

            //Sets reference so that potion spot knows which bullet activates it
            potion.PotionBullet = newBullet;

            yield return potionDelay;
        }

        actionRunning = false;

        yield return null;
    }
}

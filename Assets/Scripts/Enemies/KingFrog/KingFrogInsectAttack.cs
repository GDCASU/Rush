using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingFrogInsectAttack : KingFrogParent
{
    [SerializeField]
    private GameObject FlyObject;

    [SerializeField]
    private float SpawnDelay = 0.8f; //time between each fly

    [SerializeField]
    private int spawnNumber = 3; //number to spawn per attack

    [SerializeField]
    private int maxToSpawn = 3; //max allowed on field

    [SerializeField]
    private float CoolDown = 1.0f;

    [HideInInspector]
    public int totalCount = 0; //total flies alive

    private int insectCounter; //flies spawned this attack

    private void OnEnable()
    {
        actionRunning = true;
        insectCounter = 0;

        StartAction();
    }

    void StartAction() //delays the start of the attack for a cooldown
    {
        Invoke("SpawnFlies", CoolDown);
    }

    void SpawnFlies()
    {
        if (insectCounter < spawnNumber && totalCount < maxToSpawn)
        {
            Instantiate(FlyObject);
            insectCounter++;
            Invoke("SpawnFlies", SpawnDelay);
        }
        else
        {
            actionRunning = false;
        }
    }
}

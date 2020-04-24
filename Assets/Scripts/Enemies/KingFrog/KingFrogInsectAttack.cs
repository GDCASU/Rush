using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingFrogInsectAttack : KingFrogParent
{
    [SerializeField]
    private GameObject FlyObject;

    [SerializeField]
    private float SpawnDelay = 0.8f;

    [SerializeField]
    private int spawnNumber = 3; //number to spawn per attack

    [SerializeField]
    private int maxToSpawn = 3;

    [SerializeField]
    private float CoolDown = 1.0f;

    [HideInInspector]
    public int insectCounter = 0;

    private void OnEnable()
    {
        actionRunning = true;

        StartAction();
    }

    void StartAction() //delays the start of the attack for a cooldown
    {
        Invoke("SpawnFlies", CoolDown);
    }

    void SpawnFlies()
    {
        Instantiate(FlyObject);
        insectCounter++;

        if (insectCounter < spawnNumber)
        {
            Invoke("SpawnFlies", SpawnDelay);
        }
        else
        {
            actionRunning = false;
        }
    }
}

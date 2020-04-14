using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingFrogInsectAttack : KingFrogParent
{
    [SerializeField]
    private GameObject FlyObject;

    [SerializeField]
    private float SpawnDelay = 0.2f;

    [SerializeField]
    private int maxToSpawn = 3;

    [SerializeField]
    private float CoolDown = 1.0f;

    private int counter;

    private void OnEnable()
    {
        actionRunning = true;

        counter = 0;

        Invoke("SpawnFlies", SpawnDelay);
    }

    void SpawnFlies()
    {
        Instantiate(FlyObject);
        counter++;

        if (counter < maxToSpawn)
            Invoke("SpawnFlies", SpawnDelay);
        else
            Invoke("EndAction", CoolDown);
    }

    void EndAction()
    {
        actionRunning = false;
    }
}

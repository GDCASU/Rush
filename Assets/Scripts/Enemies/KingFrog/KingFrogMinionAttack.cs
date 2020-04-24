using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingFrogMinionAttack : KingFrogParent
{
    public int minionCount;
    WaitForSeconds ws = new WaitForSeconds(1 / 60);

    [SerializeField]
    private GameObject frogObject;

    [SerializeField]
    private int maxMinionCount = 4;
    private int spawnCounter;

    [SerializeField]
    private float spawnDelay = 0.6f;

    [SerializeField]
    private float CoolDown = 1.0f;

    private void Start()
    {
        minionCount = 0;
    }

    private void OnEnable()
    {
        actionRunning = true;
        spawnCounter = 0;
        Invoke("StartAction", CoolDown);
    }

    void StartAction()
    {
        SpawnMinions();
    }

    private void SpawnMinions()
    {
        if (minionCount < maxMinionCount && spawnCounter < 2)
        {
            //spawn minion randomly near kingfrog
            Vector3 pos = transform.position + new Vector3(Random.Range(-1.2f, 1.2f), Random.Range(-1.2f, 1.2f), 0);
            Instantiate(frogObject, pos, Quaternion.identity);
            spawnCounter++;
            Invoke("SpawnMinions", spawnDelay);
        }
        else
        {
            Invoke("EndAction", CoolDown);
        }
    }
}

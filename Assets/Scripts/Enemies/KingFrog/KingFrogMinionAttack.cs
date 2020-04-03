using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingFrogMinionAttack : KingFrogParent
{
    public int minionCount;

    [SerializeField]
    private GameObject frogObject;

    //[SerializeField]
    private int maxMinionCount = 4;

    private void Start()
    {
        minionCount = 0;
    }

    private void OnEnable()
    {
        actionRunning = true;
        StartCoroutine(SpawnMinions());
    }

    IEnumerator SpawnMinions()
    {
        for(int i = 0; i < 2; i++)
        {
            if (minionCount < maxMinionCount)
            {
                Vector3 pos = transform.position + new Vector3(Random.Range(1, 1.2f), Random.Range(1, 1.2f), 0);
                Instantiate(frogObject, pos, Quaternion.identity);
            }
            yield return new WaitForEndOfFrame();
        }
        actionRunning = false;
    }
}

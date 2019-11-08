using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is really just a wrapper for a bullet spawner 
public class RatbossA1 : BossAction{
    BulletSpawner spawner;
    RatbossA0 a0;
    public void Awake() {
        spawner = GetComponent<BulletSpawner>();
        a0 = GetComponent<RatbossA0>();
    }

    public void OnEnable () => spawner.enabled = true;//StartCoroutine(Cheese()); 
    public void OnDisable () => spawner.enabled = false;//StartCoroutine(NoMoreCheese());
    IEnumerator Cheese()
    {
        yield return a0.shakedoors();
        spawner.enabled = true;
    }
    IEnumerator NoMoreCheese()
    {
        spawner.enabled = false;
        yield return a0.returnIntoDoor();
    }
    public void Update() => spawner.alternateSpawnLocation = a0.location.transform;
}
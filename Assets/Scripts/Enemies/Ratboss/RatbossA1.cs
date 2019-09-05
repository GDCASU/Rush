using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// This is really just a wrapper for a bullet spawner 
public class RatbossA1 : MonoBehaviour {
    BulletSpawner spawner;
    RatbossA0 a0;
    public void Awake() {
        spawner = GetComponent<BulletSpawner>();
        a0 = GetComponent<RatbossA0>();
    }

    public void OnEnable () => StartCoroutine(Cheese()); 
    public void OnDisable () =>  StartCoroutine(NoMoreCheese());
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

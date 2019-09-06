using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhaseReset : MonoBehaviour {

	//horrible bandaid 

    void OnEnable () {
        var a = GetComponent<RatbossA0>();
        StartCoroutine(reset(a));
    }

    IEnumerator reset (RatbossA0 a) {
        if (a.ratOut ){
            yield return a.returnIntoDoor();
        }

        a.resetRatLocations ();

        a.enabled = false;

        yield return new WaitForEndOfFrame();

        a.enabled = true;

        this.enabled = false;
    }
}

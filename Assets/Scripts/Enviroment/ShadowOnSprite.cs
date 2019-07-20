using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShadowOnSprite : MonoBehaviour {

	void Start () {
		GetComponent<Renderer>().receiveShadows = true;
	}
	
}

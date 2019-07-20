using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pulse : MonoBehaviour {
    public float speed = 10.0f;
    public Vector3 axis = new Vector3(1,1,1);
    public Vector3 offset = new Vector3(2,2,2); 
    private float ticks;
	void Update () {
        ticks += Time.deltaTime * speed;
		transform.localScale = offset + axis * Mathf.Cos(ticks);
	}
}

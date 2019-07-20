using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpinSin : MonoBehaviour {
    
    public float speedX = 5, speedY = 5, speedZ = 5;
    public float maximumX = 360, maximumY = 360, maximumZ = 360;
    private float currentX, currentY, currentZ;
	void Update () {
        currentX += speedX * Time.deltaTime; currentY += speedY * Time.deltaTime; currentZ += speedZ * Time.deltaTime;
		transform.rotation = Quaternion.Euler( new Vector3( maximumX * Mathf.Sin( currentX ) , maximumY * Mathf.Sin( currentY ) , maximumZ * Mathf.Sin( currentZ ) ) / Mathf.PI );
	}
}

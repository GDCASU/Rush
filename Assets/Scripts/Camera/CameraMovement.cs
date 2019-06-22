using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public GameObject player;
    public GameObject camera;
	// Use this for initialization
	void Start ()
    {
        camera.transform.rotation.Set(45, 0, 0, 0);
    }
	
	// Update is called once per frame
	void FixedUpdate ()
    {
        moveCamera();
    }
    void moveCamera()
    {
        camera.transform.localPosition = new Vector3(0,10, - 2.5f);
    }
}

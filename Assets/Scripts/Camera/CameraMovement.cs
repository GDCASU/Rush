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
        //camera.transform.position=
    }
	
	// Update is called once per frame
	void Update ()
    {
        moveCamera();
    }
    void moveCamera()
    {
        //camera.transform.localPosition = new Vector3(player.transform.position.x,player.transform.position.y, -10f);
        camera.transform.localPosition = new Vector3(0,0, -10f);
    }
}

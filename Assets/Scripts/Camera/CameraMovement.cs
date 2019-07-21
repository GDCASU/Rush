using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public GameObject player;
    public float tilt = 25;
	// Use this for initialization
	void Start ()
    {
        transform.rotation = Quaternion.Euler(-tilt, 0,0);
    }
	
	// Always handle rendering and camera movement in late updates to avoid visual errors
	void LateUpdate ()
    {
        moveCamera();
    }
    void moveCamera()
    {
        transform.position = new Vector3(player.transform.position.x,player.transform.position.y - tilt/10, -10f);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatbossA3 : MonoBehaviour
{
    
    GameObject location;
    GameObject spwnLocation1;
    GameObject spwnLocation2;
    GameObject spwnLocation3;
    GameObject spwnLocation4;
    GameObject spwnLocation5;
    int actions;

	void OnEnable ()
    {
        location = GetComponent<RatbossInfo>().currentLocation;
	}




	
}

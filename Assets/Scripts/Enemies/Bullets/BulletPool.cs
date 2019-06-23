using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletPool : MonoBehaviour {
    public static BulletPool singleton;
    void Awake () => singleton = this;
	public GameObject bullet; // This is our prefab
	public static void PreLoadPool(int count) 
    {
        for(int i = 0; i < count; i++) rent();
    }

    /// <summary>
    /// This method is used to grab an object from the pool
    /// </summary>
    /// <returns> A gameobject of our prefab type (which is bullets) </returns>
    public static GameObject rent ()  {
        GameObject rented;
        if(singleton.transform.childCount > 0) {
            rented = singleton.transform.GetChild(0).gameObject;
            rented.gameObject.SetActive(true);
            rented.transform.parent = null;
        }
        else {
            rented = GameObject.Instantiate<GameObject>(singleton.bullet);
        }
        return rented;
    }

    public static void recall (GameObject g) {
        g.SetActive(false);
        g.transform.parent = singleton.transform;
    }
}

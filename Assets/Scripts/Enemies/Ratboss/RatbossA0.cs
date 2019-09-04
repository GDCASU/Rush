using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatbossA0 : MonoBehaviour
{
    public Transform location_1;
    public Transform location_2;
    public Transform location_3;
    public int frames;
    private Transform location;
    private List<Transform> posLocations;
    private int NumofPhases;
    private System.Random ran = new System.Random();

    // Use this for initialization
    void Start ()
    {
        posLocations.Add(location_1);
        posLocations.Add(location_2);
        posLocations.Add(location_3);
        NumofPhases = GetComponent<BossBehaviorController>().bossPhases.Count;
	}
    IEnumerator shakedoors()
    {
        location = pick_door(2);
        GetComponent<RatbossInfo>().currentLocation = location;
        rumble_doors(NumofPhases);
        for (int i = 0; i < frames; i++)
        {

            yield return new WaitForEndOfFrame();
        }
        yield return new WaitForEndOfFrame();
    }
    private Transform pick_door(int num)
    {
        Transform curLocation = posLocations[ran.Next(num)];
        posLocations.Remove(curLocation);
        return curLocation;
    }
    public void rumble_doors(int phases)
    {
        switch (phases)
        {
            case 2:
                {
                    StartCoroutine(rumble(posLocations[ran.Next(0)]));
                    StartCoroutine(rumble(location));
                }
                break;
            case 3:
                {
                    StartCoroutine(rumble(posLocations[0]));
                    StartCoroutine(rumble(posLocations[1]));
                    StartCoroutine(rumble(location));
                } break;
            default:
                {
                    StartCoroutine(rumble(location));
                } break;
        }   
    }
    IEnumerator rumble(Transform location)
    {
        //add locgic to rumble doors
        yield return new WaitForEndOfFrame();
    }



	
}
    

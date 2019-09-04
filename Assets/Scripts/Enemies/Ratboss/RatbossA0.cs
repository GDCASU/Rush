using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatbossA0 : MonoBehaviour
{
    public GameObject location_1;
    public GameObject location_2;
    public GameObject location_3;
    public int speed;
    public int openingframes;
    private GameObject location;
    private List<GameObject> posLocations;
    private List<GameObject> activeDoors;
    private int NumofPhases;
    private int currentPhase;
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
        rumble_doors(currentPhase);

        for (int i = 0; i < openingframes+60; i++)
        {

            yield return new WaitForEndOfFrame();
        }

        location.GetComponentInChildren<SpriteRenderer>().enabled = true;
        
        


        yield return new WaitForEndOfFrame();
    }
    void moveRatForward()
    {
        string name = location.name;
        switch (name)
        {
            case "Ratboss Location Left":
                {
                    location.transform.position = Vector3.MoveTowards(location.transform.position, location.transform.position + new Vector3(2, 0, 0), speed); 
                }break;
            case "Ratboss Location Right":
                {
                    location.transform.position = Vector3.MoveTowards(location.transform.position, location.transform.position + new Vector3(-2, 0, 0), speed);
                }
                break;
            default:
                {
                    location.transform.position = Vector3.MoveTowards(location.transform.position, location.transform.position + new Vector3(0, -2, 0), speed);
                }
                break;
        }
    }
    private GameObject pick_door(int num)
    {
        GameObject curLocation = posLocations[ran.Next(num)];
        posLocations.Remove(curLocation);
        activeDoors.Add(location);
        return curLocation;
    }
    public void rumble_doors(int phase)
    {
        GameObject other_location;
        switch (phase)
        {
            case 2:
                {
                    other_location= posLocations[ran.Next(0)];
                    activeDoors.Add(other_location);
                }
                break;
            case 3:
                {
                    other_location = posLocations[0];
                    activeDoors.Add(other_location);
                    other_location = posLocations[1];
                    activeDoors.Add(location);
                } break;
            default:
                {

                } break;
        }
        foreach (GameObject loc in activeDoors)
        {
            //do rumbling and opening animation for all the doors in activeDoors
        }
    }
    public void close_doors()
    {
        foreach (GameObject loc in activeDoors)
        {
            //if(loc!=location)//do closing door animation for all the doors in activeDoors that arent the selected one
        }
    }
}
    

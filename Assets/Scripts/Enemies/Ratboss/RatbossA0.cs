using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RatbossA0 : MonoBehaviour
{ 
    [Serializable]
    private class RatLocation {
        public GameObject door;
        public GameObject rat;
    }
    public float speed;
    public int openingframes;
    public int closeFrames;
    public int cooldownFrames; // frames before the boss bursts out 
    public GameObject location;
    private GameObject correctDoor;
    [SerializeField]
    private List<RatLocation> posLocations;
    private System.Random ran = new System.Random();
    private BossBehaviorController bbc;
    private List<int> activeDoors = new List<int>();
    // Use this for initialization'
    void OnAwake() => StartCoroutine(shakedoors());
    public void Start () => bbc = GetComponent<BossBehaviorController>();
    public IEnumerator shakedoors()
    {
        var r = pick_door(bbc.currentPhase);
        location = r.rat;
        correctDoor = r.door;
        
        // rumble doors
        const int shakingFrames = 120;
        List<Vector3> orgPos = activeDoors.Select(d => posLocations[d].door.transform.position).ToList();
        for (int i = 0; i < shakingFrames; i++)
        {
            foreach (var loc in activeDoors)
            {
                var door = posLocations[loc].door;
                door.transform.position = orgPos[loc] + new Vector3( (float)ran.NextDouble(), (float)ran.NextDouble(), 0);// SHAKE
            }
            yield return new WaitForEndOfFrame();
        }
        //return doors to original position 
        foreach (var loc in activeDoors) posLocations[loc].door.transform.position = orgPos[loc];

        // open wrong doors
        if(bbc.currentPhase>1) {
            for (int i = 0; i < openingframes+60; i++)
            {
                yield return new WaitForEndOfFrame();
                foreach (var loc in activeDoors)
                {
                    if(posLocations[loc].rat == location) continue;
                    var door = posLocations[loc].door;
                    swingDoorInterp( (float) i / (float) (openingframes + 60));
                }
            }

            for (int i = 0; i < closeFrames; i++)
            {
                yield return new WaitForEndOfFrame();
                foreach (var loc in activeDoors)
                {
                    if(posLocations[loc].rat == location) continue;
                    var door = posLocations[loc].door;
                    swingDoorInterp( 1.0f - (float) i / (float) (openingframes + 60));
                }
            }
        }

        // wait for a second before the boss bursts out
        for (int i = 0; i < cooldownFrames; i++) yield return new WaitForEndOfFrame();

        // After the wait the boss is enabled and starts moving out
        location.GetComponentInChildren<SpriteRenderer>().enabled = true;
        
        const int retractFrames = 20; // time for the boss to return into the door
        for (int i = 0; i < retractFrames; i++){
            location.transform.position = Vector3.MoveTowards(location.transform.position, location.transform.position + location.transform.forward, speed);
            yield return new WaitForEndOfFrame();
        }

    }
    public IEnumerator returnIntoDoor() {
        
        const int burstFrames = 10; // time for the boss to burst out of the door
        for (int i = 0; i < burstFrames; i++){
            location.transform.position = Vector3.MoveTowards(location.transform.position, location.transform.position - location.transform.forward, speed);
            yield return new WaitForEndOfFrame();
        }
        
        location.GetComponentInChildren<SpriteRenderer>().enabled = true;
    }
    private RatLocation pick_door(int num)
    {
        /* activeDoors = new List<int>();
         var correct = ran.Next(num-1); //oh my god this is fucking stupid we can do better than this why do you let jacob commit anything// wait wut, u ok jacob?
        HashSet<int> otherDoors = new HashSet<int>{correct};
        while(otherDoors.Count < num) otherDoors.Add(ran.Next(num-1)); 
        GameObject curLocation = posLocations[correct].rat; */

        while(activeDoors.Count < num) {
            var r = ran.Next(num-1);
            if(!activeDoors.Contains(r)) activeDoors.Add(r);
        } 
        return posLocations[activeDoors.LastOrDefault()];
    }

    /// <summary>
    /// use the interpolation value f to find the current position of the door at the swing
    /// </summary>
    /// <param name="f">a value between zero and one</param>
    public void swingDoorInterp(float f)
    {
        throw new NotImplementedException();
    }
}
    

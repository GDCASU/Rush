using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//*********
// Curent glitch: sometimes a pad SpriteRenderer doesn't enable. Seems to be because of the for-loop inside CheckSpace that delays the check
// Another: If a pad a still growing, obv the new pad won't collide with it so they will overlap
//                  possible fix: give the pads a parent that stay big with no sprite
//*********

public class KingFrogLilyPadSpawner : MonoBehaviour
{
    WaitForSeconds ws = new WaitForSeconds(1 / 60);

    [SerializeField]
    private GameObject lilyPad;
    private GameObject temp;

    [SerializeField]
    private Vector3 center;

    private Vector3 padStartScale;

    [SerializeField]
    private float arenaRadius = 3.0f;
    [SerializeField]
    private float padFloatTime = 10.0f;
    private float timerFloat;

    private bool padCollided;

    private void OnEnable()
    {
        timerFloat = 0.0f;
        padStartScale = new Vector3(0.5f, 0.5f, 1);
        StartCoroutine(Beginning());
    }
    IEnumerator Beginning() //spawns pads with slight spacing that is needed.
    {
        SpawnPad();
        for (int i = 0; i < 3; i++) //wait frames
        {
            yield return ws;
        }
        SpawnPad();
        for (int i = 0; i < 3; i++) //wait frames
        {
            yield return ws;
        }
        SpawnPad();
    }

    void Update()
    {
        if(timerFloat > padFloatTime) //timer for pad life
        {
            DeletePad();
            SpawnPad();
            timerFloat = 0;
        }
        else
        {
            timerFloat += Time.deltaTime;
        }
    }

    private void DeletePad()
    {
        //find the first child and calls "sink" since that is the oldest pad
        gameObject.transform.GetChild(0).GetComponent<KingFrogLilyPad>().Sink();
    }

    private void SpawnPad()
    {
        //pos = random position inside of arena
        Vector3 pos = center + new Vector3(Random.Range(-arenaRadius, arenaRadius), Random.Range(-arenaRadius, arenaRadius), 0.31f);

        //temp = the newly spawned lilypad (with random position and angle)
        temp = Instantiate(lilyPad, pos, Quaternion.Euler(0, 0, Random.Range(0, 360)));
        temp.transform.parent = this.gameObject.transform; //set parent of new lilypad to the lilypad spawner

        StartCoroutine(CheckSpace());
    }

    //check if spawned lilypad collides with another and continue
    IEnumerator CheckSpace()
    {
        for (int i = 0; i < 2; i++) //wait frames
        {
            yield return ws;
        }

        padCollided = temp.GetComponent<KingFrogLilyPad>().GetCollided(); //check if pad with pad

        if(padCollided) //pad collided, destroy and try again
        {
            Destroy(temp.gameObject);
            SpawnPad();
        }
        else //pad did not collide. make it small, enable sprite, and make it "rise"
        {
            temp.transform.localScale = padStartScale;
            temp.GetComponent<SpriteRenderer>().enabled = true;
            temp.GetComponent<KingFrogLilyPad>().Rise();
        }
        yield return ws;
    }

    //draw arena area
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(center, arenaRadius);
    }
}

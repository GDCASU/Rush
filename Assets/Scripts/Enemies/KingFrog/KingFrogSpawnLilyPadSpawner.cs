using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KingFrogSpawnLilyPadSpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject lilyPad;
    private GameObject temp;

    [SerializeField]
    private Vector2 center;

    [SerializeField]
    private float arenaRadius = 3.0f;

    private bool padCollided;
    private int padCount;
    private int maxPadCount;

    private void OnEnable()
    {
        SpawnPad();
    }

    private void Start()
    {
        padCount = 0;
        maxPadCount = 4;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.E))
        {
            SpawnPad();
        }
    }

    private void SpawnPad()
    {
        //pos = random position inside of arena
        Vector2 pos = center + new Vector2(Random.Range(-arenaRadius, arenaRadius), Random.Range(-arenaRadius, arenaRadius));

        //temp = the newly spawned lilypad (with random position and angle)
        temp = Instantiate(lilyPad, pos, Quaternion.Euler(0, 0, Random.Range(0, 360)));
        temp.transform.parent = this.gameObject.transform; //set parent of new lilypad to the lilypad spawner

        StartCoroutine(CheckSpace());
    }

    //check if spawned lilypad collides with another and continue
    IEnumerator CheckSpace()
    {
        for(int i = 0; i < 3; i++) //wait frames
        {
            yield return new WaitForEndOfFrame();
        }

        padCollided = temp.GetComponent<KingFrogLilyPad>().collided; //check if pad collided with pad

        if(padCollided) //pad collided, destroy and try again
        {
            Destroy(temp);
            Invoke("SpawnPad", 0);
        }
        else //pad did not collide. make it small, enable sprite, and make it "rise"
        {
            temp.transform.localScale = new Vector3(0.5f, 0.5f, 1);
            temp.GetComponent<SpriteRenderer>().enabled = true;
            temp.GetComponent<KingFrogLilyPad>().Invoke("Rise", 0);
        }
    }

    //draw arena area
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(center, arenaRadius);
    }
}

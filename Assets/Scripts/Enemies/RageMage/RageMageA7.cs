using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RageMageA7 : BossAction
{
    public int attempts;
    public int followSeconds;
    public float followSpeed;
    public float chargeSeconds;
    public float chargeSpeed;
    public float throwSeconds;
    public float cooldownSeconds;
    public float framerate;
    public bool collided;
    public bool hitWall;

    public GameObject player;
    public BoxCollider2D box;
    private WaitForSeconds ws;
    void OnEnable()
    {
        hitWall = false;
        collided = false;
        actionRunning = true;
        ws = new WaitForSeconds(1 / framerate);
        attempts = 0;
        StartCoroutine("grab");
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player" && !collision.GetComponent<PlayerDash>().inDash)
        {
            collided = true;
            if(isActiveAndEnabled)StartCoroutine(throwPlayer());
        }
        else if (collision.tag == "Wall") hitWall = true;
    }
    IEnumerator grab()
    {
        int x = 0;
        while (x < followSeconds * framerate && Vector3.Distance(transform.position,player.transform.position) > 3)
        {
            transform.position += (Vector3)(player.transform.position - transform.position).normalized * (followSpeed+0.001f*x*1.5f);

            x++;
            yield return ws;
        }
        Vector3 chargePosition = (Vector3)(player.transform.position - transform.position).normalized;
        box.enabled = true;
        //tag = "Enemy";
        int y = 0;
        while (y < chargeSeconds * framerate && !collided && !hitWall)
        {
            transform.position += chargePosition * chargeSpeed;
            y++;
            yield return ws;
        }
        //tag = "Untagged";
        if (!collided) //StartCoroutine(throwPlayer());
        {
            hitWall = false;
            if (attempts < 5)
            {
                attempts++;
                StartCoroutine(grab());
            }
            else
            { 
                for (int z = 0; z < cooldownSeconds * framerate; z++)yield return ws;
                actionRunning = false;
            }
            box.enabled = false;
        }
    }
    IEnumerator throwPlayer()
    {
        player.GetComponent<PlayerMovement>().enabled = false;
        player.GetComponent<PlayerBasicMelee>().enabled = false;
        player.GetComponent<PlayerBasicShot>().enabled = false;
        player.GetComponent<PlayerDash>().enabled = false;

        for (int x = 0; x < throwSeconds * framerate; x++)
        {
            yield return ws;
        }
        player.GetComponent<PlayerMovement>().enabled = true;
        player.GetComponent<PlayerBasicMelee>().enabled = true;
        player.GetComponent<PlayerBasicShot>().enabled = true;
        player.GetComponent<PlayerDash>().enabled = true;

        actionRunning = false;
    }
}

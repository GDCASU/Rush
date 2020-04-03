using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RageMageA8 : BossAction
{
    public float jumpHeight;
    public float jumpOutSeconds;
    public float inAirSeconds;
    public float downHeight;
    public float downRate;
    public float cooldownSeconds;
    public float framerate;

    public GameObject shadowPrefab;
    public BoxCollider2D box;
    void OnEnable()
    {
        StartCoroutine(elbowDrop());
    }
    IEnumerator elbowDrop()
    {
        actionRunning = true;
        WaitForSeconds ws = new WaitForSeconds(1 / framerate);
        int x = 0;
        while (x < jumpOutSeconds * framerate && transform.position.z>-jumpHeight)
        {
            transform.position=new Vector3(transform.position.x, transform.position.y, transform.position.z*1.005f) +new Vector3(0,0,-.1f*x);
            x++;
            yield return ws;
        }
        GameObject shadow = Instantiate(shadowPrefab, new Vector3(transform.position.x, transform.position.y, -.1f), Quaternion.identity);
        for (int y = 0; y < inAirSeconds * framerate; y++)
        {
            shadow.transform.position = PlayerHealth.singleton.transform.position+ new Vector3(0,0,1.9f);
            yield return ws;
        }
        transform.position = new Vector3(shadow.transform.position.x, shadow.transform.position.y,transform.position.z);
        while (transform.position.z<downHeight)
        {
            transform.position += new Vector3(0, 0, downRate * x);
            shadow.GetComponent<SpriteRenderer>().color = new Color(1, 1, 1, shadow.GetComponent<SpriteRenderer>().color.a + .05f);
            shadow.transform.localScale += new Vector3(.001f, .001f, 0);
            if (transform.position.z <= downHeight)
            {
                box.enabled = true;
                //tag = "Enemy";
            } 
            yield return ws;
        }
        Destroy(shadow);
        //tag = "Untagged";
        for (int y = 0; y < cooldownSeconds * framerate; y++) 
        {
            yield return ws;
        }
        box.enabled = false;
        actionRunning = false; 
    }
}

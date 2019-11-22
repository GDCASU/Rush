using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stomp : BossAction
{
    private Transform player;
    public int warningFrames;
    public int endingFrames;
    private float increment;
    private float decrement;

    void Awake()
    {
        float diameter = 2 * GetComponentInParent<CircleCollider2D>().radius;
        increment = diameter / warningFrames;
        decrement = -diameter / endingFrames;
    }

    private void OnEnable() => StartCoroutine(StompAOE());

    IEnumerator StompAOE()
    {
        GetComponentInChildren<MeshRenderer>().enabled = true;
        for (int i = 0; i < warningFrames; i++)
        {
            transform.GetChild(0).transform.localScale = transform.GetChild(0).transform.localScale + new Vector3(increment, 0, increment);
            yield return new WaitForEndOfFrame();
        }
        GetComponent<CircleCollider2D>().enabled = true;
        for (int i = 0; i < endingFrames; i++)
        {
            transform.GetChild(0).transform.localScale = transform.GetChild(0).transform.localScale + new Vector3(decrement, 0, decrement);
            yield return new WaitForEndOfFrame();
        }
        GetComponentInChildren<MeshRenderer>().enabled = false;
        GetComponent<CircleCollider2D>().enabled = false;
        actionRunning = false;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        player.GetComponent<PlayerHealth>().takeDamage();
    }
}

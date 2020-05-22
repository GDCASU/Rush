using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stomp : BossAction
{
    private Transform player;
    public int warningFrames;
    public int endingFrames;
    public float speed;
    private float increment;
    private float decrement;

    void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Transform>();
        float diameter = 2 * GetComponentInParent<CircleCollider2D>().radius;
        increment = diameter / warningFrames;
        decrement = -diameter / endingFrames;
    }

    private void OnEnable() => StartCoroutine(StompAOE());

    IEnumerator StompAOE()
    {
        flip();

        for (int i = 0; i < 60; i++)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(PlayerHealth.singleton.transform.position.x, PlayerHealth.singleton.transform.position.y, transform.position.z), speed*0.1f);
            flip();
            yield return new WaitForEndOfFrame();
        }

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

    void flip()
    {
        if (transform.position.x > PlayerHealth.singleton.transform.position.x)
            transform.rotation = Quaternion.Euler(transform.rotation.x, 180, transform.rotation.z);
        else
            transform.rotation = Quaternion.Euler(transform.rotation.x, 0, transform.rotation.z);
    }
}

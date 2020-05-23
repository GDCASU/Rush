using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dash : BossAction
{
    private Transform player;
    public int preDashFrames;
    public int dashFrames;
    public bool move = false;
    private Vector3 vector;
    private float distance;
    public int readjustFrames;
    public float speed = 1;
    public bool thisAction = false;

    void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Transform>();
    }

    private void OnEnable() => StartCoroutine(DashAttack());

    IEnumerator DashAttack()
    {
        thisAction = true;
        Vector3 playerPos = PlayerHealth.singleton.transform.position;

        flip();

        for (int i = 0; i < preDashFrames; i++)
        {
            transform.position = Vector3.MoveTowards(transform.position, new Vector3(0, 0, transform.position.z), speed*0.03f);
            yield return new WaitForEndOfFrame();
        }
        for (int x = 0; x < 30; x++)
        {
            transform.GetChild(1).transform.Rotate(new Vector3(0, 0, 30f / 30f));
            yield return new WaitForEndOfFrame();
        }
        GetComponent<EnemyHealth>().enabled = false;
        playerPos = PlayerHealth.singleton.transform.position;
        vector = playerPos - transform.position;
        distance = Mathf.Sqrt(vector.x * vector.x + vector.y * vector.y);

        flip();

        move = true;
        while (move)
        {
            transform.position = transform.position + new Vector3(speed * 0.2f * vector.x / distance, speed * 0.2f * vector.y / distance, 0);
            yield return new WaitForEndOfFrame();
        }
        GetComponent<EnemyHealth>().enabled = true;
        StartCoroutine(Lance());
        thisAction = false;
        actionRunning = false;
    }

    IEnumerator Lance()
    {
        for (int x = 0; x < 30; x++)
        {
            transform.GetChild(1).transform.Rotate(new Vector3(0, 0, -30f / 30f));
            yield return new WaitForEndOfFrame();
        }
    }

    private void Update()
    {
        //if (move) transform.position = transform.position + new Vector3(speed * 0.2f * vector.x / distance, speed * 0.2f * vector.y / distance, 0);
    }

    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    if (collision.gameObject.tag == "Wall" && thisAction)
    //    {
    //        move = false;
    //    }
    //}

    void flip()
    {
        if (transform.position.x > PlayerHealth.singleton.transform.position.x)
            transform.rotation = Quaternion.Euler(transform.rotation.x, 180, transform.rotation.z);
        else
            transform.rotation = Quaternion.Euler(transform.rotation.x, 0, transform.rotation.z);
    }
}

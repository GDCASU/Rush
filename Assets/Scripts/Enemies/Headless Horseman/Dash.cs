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

    void Awake()
    {
        player = GameObject.Find("Player").GetComponent<Transform>();
    }

    private void OnEnable() => StartCoroutine(DashAttack());

    IEnumerator DashAttack()
    {
        for (int i = 0; i < preDashFrames; i++)
        {
            yield return new WaitForEndOfFrame();
        }
        GetComponent<BoxCollider2D>().enabled = true;
        GetComponent<EnemyHealth>().enabled = false;
        Vector3 playerPos = PlayerHealth.singleton.transform.position;
        vector = playerPos - transform.position;
        distance = Mathf.Sqrt(vector.x * vector.x + vector.y * vector.y);
        move = true;
       //while (move)
        {
            //transform.position = transform.position + new Vector3(0.2f * vector.x / distance, 0.2f * vector.y / distance, 0);
            //OnCollisionEnter2D(player.GetComponent<Rigidbody2D>().coll);
            //yield return new WaitForEndOfFrame();
        }
        
    }

    private void Update()
    {
        if (move) transform.position = transform.position + new Vector3(0.2f * vector.x / distance, 0.2f * vector.y / distance, 0);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        Debug.Log(collision.gameObject.tag);
        //if (collision.gameObject.tag == "Player")
            //player.GetComponent<PlayerHealth>().takeDamage();
        if (collision.gameObject.tag == "Wall")
        {
            //Debug.Log("Hello");
            for (int i = 0; i < readjustFrames; i++)
            {
                transform.position = Vector3.MoveTowards(transform.position, PlayerHealth.singleton.transform.position, 0.01f);
                //yield return new WaitForEndOfFrame();
            }
            move = false;
            GetComponent<BoxCollider2D>().enabled = false;
            GetComponent<EnemyHealth>().enabled = true;
            actionRunning = false;
        }

    }
}
